using System;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public enum Role { Front, Side, Back }
    public IState CurrentState { get; private set; }
    public StateType CurrentStateType => CurrentState?.Type ?? StateType.Idle;

    //設定可能フィールド（Inspector）
    //巡回状態関連 
    [Header("巡回状態関連")]
    private Vector3 patrolTarget;
    //巡回地点をランダムに選ぶ範囲の半径
    [SerializeField] private float patrolRadius = 10f;
    //待機状態で止まる時間
    [SerializeField] private float patrolWaitTime = 3f;

    //待機中の経過時間フレーム
    private float patrolTimer = 0f;
    [SerializeField] private Vector3 patrolCenter;          //巡回の中心点
    [SerializeField] private float patrolAreaRadius = 20f;  //この範囲から出ない

    //Boids群れ制御関連
    [Header("Boids群れ制御関連")]
    //他の敵との距離を保つ力の重み
    [SerializeField] private float separationWeight = 2.5f;
    //近くの敵と速度を合わせる力の重み
    [SerializeField] private float alignmentWeight = 0.1f;
    //群れの中心に向かう力の重み
    [SerializeField] private float cohesionWeight = 0.1f;
    //Boids 計算に参加する近くの敵の範囲
    [SerializeField] private float neighborRadius = 1.5f;
    //Boids 力の最大値
    [SerializeField] private float maxBoidsForce = 7f;
    //Boids 計算の更新間隔
    [SerializeField] private int boidsUpdateInterval = 3;

    //攻撃関連
    [Header("攻撃関連")]
    [SerializeField] private float attackInterval = 5.0f;   //攻撃間隔
    [SerializeField] private float attackRange = 2.5f;      //攻撃範囲
    [SerializeField] private float keepDistance = 2.0f;     //適正距離
    [SerializeField] private float retreatDistance = 1.2f;  //近すぎると下がる距離
    [SerializeField] private float attackMoveSpeed = 2.0f;  //攻撃時の移動速度
    [SerializeField] private float dashSpeed = 8f;          //突進スピード
    [SerializeField] private float dashTime = 10.0f;        //どれだけ突進するか
    [SerializeField] private float attackRadius = 0.1f;     //当たり判定の半径
    [SerializeField] private int attackPower = 20;          //攻撃力

    //HP
    [Header("HP関連")]
    [SerializeField] private int hp = 100;

    //役割
    [Header("Role & Misc")]
    [SerializeField] private Role role = Role.Front;
    [SerializeField] private float encircleSignRandomSeed;

    //ノックバック
    [SerializeField] float knockbackPower = 5.0f;
    [SerializeField] float knockbackDuration = 0.2f;

    //internal
    private NavMeshAgent agent;
    private Transform player;
    private IState currentState;
    private BoidsSteering boids;
    private AttackController attackController;
    private EnemyManager manager;
    bool isKnockback = false;
    Vector3 knockbackDir;
    float knockbackTimer;

    //フレームカウンタ
    private int frameCounter = 0;
    //前回の Boids 力を保持し、更新間隔中は再利用
    private Vector3 lastBoidsForce;

    //debug
    private LineRenderer debugLine;

    public GameObject alertPrefab;

    //「！」プレハブ
    private bool alerted = false; //1回だけ表示したい

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        boids = new BoidsSteering(this, neighborRadius, separationWeight, alignmentWeight, cohesionWeight, maxBoidsForce, boidsUpdateInterval);
        agent.radius = 0.6f;
        encircleSignRandomSeed = UnityEngine.Random.value;
        //LineRenderer for debugging
        debugLine = gameObject.AddComponent<LineRenderer>();
        debugLine.startWidth = debugLine.endWidth = 0.05f;
        debugLine.material = new Material(Shader.Find("Sprites/Default"));
        debugLine.positionCount = 2;

        //if (!agent.isOnNavMesh)
        //{
        //    Debug.LogError($"NavMesh 上にいません");
        //}
    }

    private void Start()
    {
        //Registerだけでなくセットアップも必ず行う
        EnemyManager.Instance.RegisterEnemy(this);

        //EnemyManager にプレイヤー情報がある前提
        if (EnemyManager.Instance.Player != null)
        {
            Initialize(
                EnemyManager.Instance,
                EnemyManager.Instance.Player,
                EnemyManager.Instance.AttackController
            );
        }
        else
        {
            Debug.LogError("EnemyManager に Player が設定されていません。Initialize 未実行です！");
        }
    }

    //初期化は外から呼ぶ（EnemyManager など）
    public void Initialize(EnemyManager manager, Transform player, AttackController attackController)
    {
        this.manager = manager;
        this.player = player;
        this.attackController = attackController;
        agent.speed += UnityEngine.Random.Range(-0.5f, 0.5f);
        separationWeight += UnityEngine.Random.Range(-0.5f, 0.5f);
        cohesionWeight += UnityEngine.Random.Range(-0.05f, 0.05f);
        alignmentWeight += UnityEngine.Random.Range(-0.05f, 0.05f);
        agent.avoidancePriority = UnityEngine.Random.Range(40, 90);
        patrolTarget = Vector3.zero;
        AssignRandomRole();
        ChangeState(new PatrolState(this));
    }

    void Update()
    {
        //ノックバック中は他の処理を完全停止
        if (isKnockback)
        {
            KnockbackUpdate();
            return;
        }
        //現在の状態の更新処理
        currentState?.OnUpdate();
        //巡回目的地への線を表示
        if (patrolTarget != Vector3.zero)
        {
            debugLine.SetPosition(0, transform.position + Vector3.up * 0.1f);
            debugLine.SetPosition(1, patrolTarget + Vector3.up * 0.1f);
        }
    }

    public bool CanSeePlayer(float viewDistance = 10f, float viewAngle = 60f)
    {
        if (player == null)
        {
            Debug.Log("プレイヤーが空です");
            return false;
        }

        Vector3 dir = player.position - transform.position;
        float distance = dir.magnitude;
        if (distance < viewDistance && Vector3.Angle(transform.forward, dir) < viewAngle)
        {
            if (Physics.Raycast(transform.position + Vector3.up, dir.normalized, out RaycastHit hit, distance))
            {
                return hit.transform == player;
            }
        }
        return false;
    }

    public void SetPatrolTarget(Vector3 pos)
    {
        patrolTarget = pos;
        agent.SetDestination(patrolTarget);
    }

    //ランダムなパトロール地点を設定
    public void SetRandomPatrolPoint()
    {
        //最大10回試行して安全な地点を探す
        for (int i = 0; i < 10; i++) 
        {
            //中心からランダムに取得
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere
                * patrolRadius + patrolCenter;

            randomDirection.y = transform.position.y;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit,
                patrolRadius, NavMesh.AllAreas))
            {
                //範囲チェック：巡回エリア外は無効
                if (Vector3.Distance(hit.position, patrolCenter) <= patrolAreaRadius)
                {
                    patrolTarget = hit.position; agent.SetDestination(patrolTarget);
                    Debug.Log($"新しいパトロール地点: {patrolTarget}"); 
                    return;
                }
            }
        }
        Debug.LogError("有効なパトロール地点が見つかりません");
    }

    public Vector3 EncircleDir(Vector3 toPlayer)
    {
        float sign = encircleSignRandomSeed > 0.5f ? 1f : -1f;
        return Quaternion.Euler(0, 90f * sign, 0) * toPlayer.normalized;
    }

    //状態変更
    public void ChangeState(IState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState?.OnStart();
    }

    private void AssignRandomRole()
    {
        role = (Role)UnityEngine.Random.Range(0, 3);
    }

    //デバッグギズモ
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(patrolCenter, patrolAreaRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
        if (patrolTarget != Vector3.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(patrolTarget, 0.3f);
            Gizmos.DrawLine(transform.position, patrolTarget);
        }
    }

    public void ShowAlert()
    {
        if (alertPrefab == null) return;
        Debug.Log("プレイヤーを発見しました");

        //敵の頭上に生成
        Vector3 pos = transform.position + Vector3.up * 2.0f;
        GameObject alert = Instantiate(alertPrefab, pos, Quaternion.identity);
        alert.GetComponent<BillBoard>().enemy = this.transform;
    }
    //ノックバック発生
    public void ApplyKnockback(Vector3 attackerPos)
    {
        if (isKnockback) return;

        isKnockback = true;
        knockbackTimer = knockbackDuration;

        //方向は一度だけ確定
        knockbackDir = (transform.position - attackerPos).normalized;

        //NavMeshAgentを一時停止
        agent.isStopped = true;
    }
    //ノックバック更新
    void KnockbackUpdate()
    {
        knockbackTimer -= Time.deltaTime;

        transform.position += knockbackDir * knockbackPower * Time.deltaTime;

        if (knockbackTimer <= 0f)
        {
            isKnockback = false;

            //NavMeshAgent再開
            agent.isStopped = false;
        }
    }
    //ダメージ
    public void TakeDamage(int damage, Vector3 attackerPos)
    {
        //HP減少
        hp -= damage;

        //ノックバック発生
        ApplyKnockback(attackerPos);

        //死亡判定
        if (hp <= 0)
        {
            Die();
        }
    }
    //死亡処理
    void Die()
    {
        //EnemyManagerから除外
        manager?.UnregisterEnemy(this);

        Destroy(gameObject);
    }

    //外部用の読み取り専用関数

    public Vector3 GetPatrolCenter() => patrolCenter;
    public Vector3 GetPatrolTarget() => patrolTarget;
    public float GetPatrolRadius() => patrolRadius;
    public float GetPatrolAreaRadius() => patrolAreaRadius;
    public NavMeshAgent Agent => agent;
    public Transform Player => player;
    public BoidsSteering Boids => boids;
    public AttackController AttackCtrl => attackController;
    public EnemyManager Manager => manager;
    public Role EnemyRole => role;
    public float KeepDistance => keepDistance;
    public float RetreatDistance => retreatDistance;
    public float AttackInterval => attackInterval;
    public float AttackRadius => attackRadius;
    public int AttackPower => attackPower;
    public float DashTime => dashTime;
    public float DashSpeed => dashSpeed;

    public void SetPlayer(Transform p)
    {
        player = p;
    }
    
}