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
    //敵同士の距離
    [SerializeField] private float separationWeight = 0.4f;
    [SerializeField] private float alignmentWeight = 1.0f;
    [SerializeField] private float cohesionWeight = 1.0f;
    [SerializeField] private float neighborRadius = 3.5f;
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
    void Update()
    {
        //デバッグ表示（実際のゲーム画面では見えないがSceneビューで見える）
        Debug.DrawLine(transform.position, patrolTarget, Color.yellow);
        Debug.DrawRay(patrolTarget, Vector3.up * 0.5f, Color.yellow);
    }

    //状態管理用のUpdate（EnemyManagerから呼ばれる）
    public void ManagedUpdate()
    {
        //状態ごとの処理
        switch (state)
        {
            case EnemyState.Idle: Idle(); break;
            case EnemyState.Patrol: Patrol(); break;
            //case EnemyState.Chase: Chase(); break;
            //case EnemyState.Attack: Attack(); break;
        }
    }
    //待機状態
    void Idle()
    {
        //待機タイマー更新
        patrolTimer += Time.deltaTime;

        ////Player発見で追跡へ
        //if (CanSeePlayer())
        //{
        //    SetChase();
        //    return;
        //}

        //待機状態で一定時間経過したら巡回へ
        if (patrolTimer > patrolWaitTime)
        {
            SetRandomPatrolPoint();
            state = EnemyState.Patrol;
            Debug.Log("巡回状態へ");
            patrolTimer = 0f;
        }

    }
    //巡回状態
    void Patrol()
    {
        ////Player発見で追跡へ
        //if (CanSeePlayer())
        //{
        //    SetChase();
        //    return;
        //}

        //Boidsで自然に群れながらPatrol
        Vector3 toTarget = (patrolTarget - transform.position).normalized;
        Vector3 boidsForce = GetBoidsForceOptimized();/* * 0.5f; //Patrol中は弱め*/
        Vector3 moveDir = (toTarget + boidsForce).normalized;

        //移動
        ApplyMovement(moveDir);

        //目的地に到達したらIdleで待機
        if (!agent.pathPending && Vector3.Distance(transform.position, patrolTarget) < 0.5f)
        {
            state = EnemyState.Idle;
            Debug.Log("待機状態へ");
            patrolTimer = 0f;
        }
    }
    //巡回範囲と目的地をGizmosで可視化
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        //現在の目的地
        Gizmos.DrawSphere(patrolTarget, 0.3f);

        //敵の位置 → 目的地へのライン
        Gizmos.DrawLine(transform.position, patrolTarget);

        //巡回半径を可視化
        Gizmos.color = new Color(1f, 1f, 0f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
    void Chase()
    {
        if (player == null) return;

        Vector3 toPlayer = (player.position - transform.position);
        float distance = toPlayer.magnitude;
        Vector3 toPlayerDir = toPlayer.normalized;

        //プレイヤーとの距離に応じて包囲 or 接近
        Vector3 encircleDir = Quaternion.Euler(0, 90f, 0) * toPlayerDir; //プレイヤーの周囲を回る方向
        Vector3 desiredPos;

        if (distance > 4f)
        {
            //離れすぎ → 接近
            desiredPos = toPlayerDir;
        }
        else if (distance < 2.5f)
        {
            //近すぎ → 少し離れる
            desiredPos = -toPlayerDir * 0.5f + encircleDir * 0.5f;
        }
        else
        {
            //適距離 → 包囲行動
            desiredPos = encircleDir * 0.8f + toPlayerDir * 0.2f;
        }

        //Boids補正（群れ制御）
        Vector3 boidsForce = GetBoidsForceOptimized() * 0.3f;

        //最終移動方向
        Vector3 moveDir = (desiredPos.normalized + boidsForce).normalized;

        ApplyMovement(moveDir);

        //プレイヤーが近い → 攻撃へ
        if (distance < 1.8f)
        {
            state = EnemyState.Attack;
            Debug.Log("攻撃状態へ");
        }

        //見失ったらIdle
        if (!CanSeePlayer())
        {
            state = EnemyState.Idle;
            Debug.Log("見失ったので待機状態へ");
        }
    }
    void Attack()
    {
        //transform.LookAt(player);

        ////攻撃処理はここに
        //if (Vector3.Distance(transform.position, player.position) > 3f)
        //    SetChase();
    }
    //ランダムなパトロール地点を設定
    void SetRandomPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = hit.position;
            agent.SetDestination(patrolTarget);
            Debug.Log($"[EnemyAI] 新しいパトロール地点を設定しました: {patrolTarget}");
        }
        else
        {
            Debug.LogWarning($"[EnemyAI] NavMesh上に有効なパトロール地点を見つけられませんでした ({transform.position})");
        }
    }
    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 dir = player.position - transform.position;
        float distance = dir.magnitude;

        //距離と視野角の条件を満たした場合のみ
        if (distance < 10f && Vector3.Angle(transform.forward, dir) < 60f)
        {
            //レイキャストで遮蔽物チェック（プレイヤーの見える・見えないをより正確に）
            if (Physics.Raycast(transform.position + Vector3.up, dir.normalized, out RaycastHit hit, distance))
            {
                if (hit.transform == player)
                {
                    Debug.Log($"{name} がプレイヤーを発見！");
                    return true;
                }
            }
        }
        return false;
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
        //NavMeshAgentの移動先を直接制御
        Vector3 nextPos = transform.position + moveDir * agent.speed * Time.deltaTime;

        if (NavMesh.SamplePosition(nextPos, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
    void SetChase()
    {
        state = EnemyState.Chase;
        Debug.Log("追跡状態へ");
        EnemyManager.Instance.AlertNearbyEnemies(this, alertRadius);
    }
    //警報共有で呼ばれる
    public void OnAlerted()
    {
        if (state == EnemyState.Idle || state == EnemyState.Patrol)
        { 
            //ほかの敵も追跡状態へ
            state = EnemyState.Chase;
           Debug.Log("警報を受けて追跡状態へ");
        }
    }
    //Player を後からセットできる
    public void SetPlayer(Transform p)
    {
        player = p;
    }
    void OnDestroy()
    {
        if (EnemyManager.Instance != null)
            EnemyManager.Instance.Unregister(this);
    }
}
