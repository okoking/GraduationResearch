using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Idle, Patrol, Chase, Attack }
    public EnemyState state = EnemyState.Idle;

    private Transform player;
    private NavMeshAgent agent;

    //Patrol
    private Vector3 patrolTarget;
    private float patrolRadius = 8f;
    private float patrolWaitTime = 2f;
    private float patrolTimer = 0f;

    //Boids
    [Header("Boids")]
    [SerializeField] private float separationWeight = 1.5f;
    [SerializeField] private float alignmentWeight = 1.0f;
    [SerializeField] private float cohesionWeight = 1.0f;
    [SerializeField] private float neighborRadius = 5f;
    [SerializeField] private float maxBoidsForce = 3f;
    [SerializeField] private int boidsUpdateInterval = 3;
    private int frameCounter = 0;
    private Vector3 lastBoidsForce;

    //Alert
    [Header("Alert")]
    [SerializeField] private float alertRadius = 8f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = EnemyManager.Instance.GetPlayerTransform();
        EnemyManager.Instance.Register(this);
    }
    public void ManagedUpdate()
    {
        switch (state)
        {
            case EnemyState.Idle: Idle(); break;
            case EnemyState.Patrol: Patrol(); break;
            case EnemyState.Chase: Chase(); break;
            case EnemyState.Attack: Attack(); break;
        }
    }
    void Idle()
    {
        patrolTimer += Time.deltaTime;

        if (CanSeePlayer())
        {
            SetChase();
            return;
        }

        if (patrolTimer > patrolWaitTime)
        {
            SetRandomPatrolPoint();
            state = EnemyState.Patrol;
            patrolTimer = 0f;
        }
    }
    void Patrol()
    {
        if (CanSeePlayer())
        {
            SetChase();
            return;
        }

        //Boidsで自然に群れながらPatrol
        Vector3 toTarget = (patrolTarget - transform.position).normalized;
        Vector3 boidsForce = GetBoidsForceOptimized() * 0.5f; //Patrol中は弱め
        Vector3 moveDir = (toTarget + boidsForce).normalized;

        ApplyMovement(moveDir);

        //目的地に到達したらIdleで待機
        if (!agent.pathPending && Vector3.Distance(transform.position, patrolTarget) < 0.5f)
        {
            state = EnemyState.Idle;
            patrolTimer = 0f;
        }
    }
    void Chase()
    {
        Vector3 toPlayer = (player.position - transform.position).normalized;
        Vector3 boidsForce = GetBoidsForceOptimized();
        Vector3 moveDir = (toPlayer + boidsForce).normalized;

        ApplyMovement(moveDir);

        if (Vector3.Distance(transform.position, player.position) < 2f)
            state = EnemyState.Attack;

        if (!CanSeePlayer())
            state = EnemyState.Idle;
    }
    void Attack()
    {
        transform.LookAt(player);

        //攻撃処理はここに
        if (Vector3.Distance(transform.position, player.position) > 3f)
            SetChase();
    }
    void SetRandomPatrolPoint()
    {
        Vector3 randomDir = Random.insideUnitSphere * patrolRadius + transform.position;
        if (NavMesh.SamplePosition(randomDir, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = hit.position;
            agent.SetDestination(patrolTarget);
        }
    }
    //Player を後からセットできる
    public void SetPlayer(Transform p)
    {
        player = p;
    }
    bool CanSeePlayer()
    {
        Vector3 dir = player.position - transform.position;
        return dir.magnitude < 10f && Vector3.Angle(transform.forward, dir) < 60f;
    }
    //Boids計算（フレームスキップ付き）
    Vector3 GetBoidsForceOptimized()
    {
        frameCounter++;
        if (frameCounter >= boidsUpdateInterval)
        {
            lastBoidsForce = CalculateBoidsForce();
            frameCounter = 0;
        }
        return lastBoidsForce;
    }
    Vector3 CalculateBoidsForce()
    {
        var neighbors = EnemyManager.Instance.GetNearbyEnemies(this, neighborRadius);
        if (neighbors.Count == 0) return Vector3.zero;

        Vector3 separation = Vector3.zero;
        Vector3 alignment = Vector3.zero;
        Vector3 cohesion = Vector3.zero;

        foreach (var other in neighbors)
        {
            Vector3 diff = transform.position - other.transform.position;
            float dist = diff.magnitude;
            if (dist > 0) separation += diff.normalized / dist;
            alignment += other.agent.velocity;
            cohesion += other.transform.position;
        }

        separation /= neighbors.Count;
        alignment  /= neighbors.Count;
        cohesion = (cohesion / neighbors.Count) - transform.position;

        Vector3 boidsForce = separation * separationWeight +
                             alignment.normalized * alignmentWeight +
                             cohesion.normalized * cohesionWeight;

        return Vector3.ClampMagnitude(boidsForce, maxBoidsForce);
    }
    void ApplyMovement(Vector3 moveDir)
    {
        Vector3 targetVelocity = moveDir * agent.speed;
        agent.velocity = Vector3.Lerp(agent.velocity, targetVelocity, Time.deltaTime * 5f);
        agent.Move(agent.velocity * Time.deltaTime);
    }
    void SetChase()
    {
        state = EnemyState.Chase;
        EnemyManager.Instance.AlertNearbyEnemies(this, alertRadius);
    }
    //警報共有で呼ばれる
    public void OnAlerted()
    {
        if (state == EnemyState.Idle || state == EnemyState.Patrol)
            state = EnemyState.Chase;
    }
    void OnDestroy()
    {
        if (EnemyManager.Instance != null)
            EnemyManager.Instance.Unregister(this);
    }
}
