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
    //�G���m�̋���
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
        //�f�o�b�O�\���i���ۂ̃Q�[����ʂł͌����Ȃ���Scene�r���[�Ō�����j
        Debug.DrawLine(transform.position, patrolTarget, Color.yellow);
        Debug.DrawRay(patrolTarget, Vector3.up * 0.5f, Color.yellow);
    }

    //��ԊǗ��p��Update�iEnemyManager����Ă΂��j
    public void ManagedUpdate()
    {
        //��Ԃ��Ƃ̏���
        switch (state)
        {
            case EnemyState.Idle: Idle(); break;
            case EnemyState.Patrol: Patrol(); break;
            //case EnemyState.Chase: Chase(); break;
            //case EnemyState.Attack: Attack(); break;
        }
    }
    //�ҋ@���
    void Idle()
    {
        //�ҋ@�^�C�}�[�X�V
        patrolTimer += Time.deltaTime;

        ////Player�����ŒǐՂ�
        //if (CanSeePlayer())
        //{
        //    SetChase();
        //    return;
        //}

        //�ҋ@��Ԃň�莞�Ԍo�߂����珄���
        if (patrolTimer > patrolWaitTime)
        {
            SetRandomPatrolPoint();
            state = EnemyState.Patrol;
            Debug.Log("�����Ԃ�");
            patrolTimer = 0f;
        }

    }
    //������
    void Patrol()
    {
        ////Player�����ŒǐՂ�
        //if (CanSeePlayer())
        //{
        //    SetChase();
        //    return;
        //}

        //Boids�Ŏ��R�ɌQ��Ȃ���Patrol
        Vector3 toTarget = (patrolTarget - transform.position).normalized;
        Vector3 boidsForce = GetBoidsForceOptimized();/* * 0.5f; //Patrol���͎��*/
        Vector3 moveDir = (toTarget + boidsForce).normalized;

        //�ړ�
        ApplyMovement(moveDir);

        //�ړI�n�ɓ��B������Idle�őҋ@
        if (!agent.pathPending && Vector3.Distance(transform.position, patrolTarget) < 0.5f)
        {
            state = EnemyState.Idle;
            Debug.Log("�ҋ@��Ԃ�");
            patrolTimer = 0f;
        }
    }
    //����͈͂ƖړI�n��Gizmos�ŉ���
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        //���݂̖ړI�n
        Gizmos.DrawSphere(patrolTarget, 0.3f);

        //�G�̈ʒu �� �ړI�n�ւ̃��C��
        Gizmos.DrawLine(transform.position, patrolTarget);

        //���񔼌a������
        Gizmos.color = new Color(1f, 1f, 0f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
    void Chase()
    {
        if (player == null) return;

        Vector3 toPlayer = (player.position - transform.position);
        float distance = toPlayer.magnitude;
        Vector3 toPlayerDir = toPlayer.normalized;

        //�v���C���[�Ƃ̋����ɉ����ĕ�� or �ڋ�
        Vector3 encircleDir = Quaternion.Euler(0, 90f, 0) * toPlayerDir; //�v���C���[�̎��͂�������
        Vector3 desiredPos;

        if (distance > 4f)
        {
            //���ꂷ�� �� �ڋ�
            desiredPos = toPlayerDir;
        }
        else if (distance < 2.5f)
        {
            //�߂��� �� ���������
            desiredPos = -toPlayerDir * 0.5f + encircleDir * 0.5f;
        }
        else
        {
            //�K���� �� ��͍s��
            desiredPos = encircleDir * 0.8f + toPlayerDir * 0.2f;
        }

        //Boids�␳�i�Q�ꐧ��j
        Vector3 boidsForce = GetBoidsForceOptimized() * 0.3f;

        //�ŏI�ړ�����
        Vector3 moveDir = (desiredPos.normalized + boidsForce).normalized;

        ApplyMovement(moveDir);

        //�v���C���[���߂� �� �U����
        if (distance < 1.8f)
        {
            state = EnemyState.Attack;
            Debug.Log("�U����Ԃ�");
        }

        //����������Idle
        if (!CanSeePlayer())
        {
            state = EnemyState.Idle;
            Debug.Log("���������̂őҋ@��Ԃ�");
        }
    }
    void Attack()
    {
        //transform.LookAt(player);

        ////�U�������͂�����
        //if (Vector3.Distance(transform.position, player.position) > 3f)
        //    SetChase();
    }
    //�����_���ȃp�g���[���n�_��ݒ�
    void SetRandomPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = hit.position;
            agent.SetDestination(patrolTarget);
            Debug.Log($"[EnemyAI] �V�����p�g���[���n�_��ݒ肵�܂���: {patrolTarget}");
        }
        else
        {
            Debug.LogWarning($"[EnemyAI] NavMesh��ɗL���ȃp�g���[���n�_���������܂���ł��� ({transform.position})");
        }
    }
    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 dir = player.position - transform.position;
        float distance = dir.magnitude;

        //�����Ǝ���p�̏����𖞂������ꍇ�̂�
        if (distance < 10f && Vector3.Angle(transform.forward, dir) < 60f)
        {
            //���C�L���X�g�ŎՕ����`�F�b�N�i�v���C���[�̌�����E�����Ȃ�����萳�m�Ɂj
            if (Physics.Raycast(transform.position + Vector3.up, dir.normalized, out RaycastHit hit, distance))
            {
                if (hit.transform == player)
                {
                    Debug.Log($"{name} ���v���C���[�𔭌��I");
                    return true;
                }
            }
        }
        return false;
    }
    //Boids�v�Z�i�t���[���X�L�b�v�t���j
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
        //NavMeshAgent�̈ړ���𒼐ڐ���
        Vector3 nextPos = transform.position + moveDir * agent.speed * Time.deltaTime;

        if (NavMesh.SamplePosition(nextPos, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
    void SetChase()
    {
        state = EnemyState.Chase;
        Debug.Log("�ǐՏ�Ԃ�");
        EnemyManager.Instance.AlertNearbyEnemies(this, alertRadius);
    }
    //�x�񋤗L�ŌĂ΂��
    public void OnAlerted()
    {
        if (state == EnemyState.Idle || state == EnemyState.Patrol)
        { 
            //�ق��̓G���ǐՏ�Ԃ�
            state = EnemyState.Chase;
           Debug.Log("�x����󂯂ĒǐՏ�Ԃ�");
        }
    }
    //Player ���ォ��Z�b�g�ł���
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
