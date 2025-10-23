using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //状態管理関連
    public enum EnemyState { Idle, Patrol, Chase, Attack }
    public EnemyState state;

    //プレイヤーの座標
    private Transform player;
    //NavMeshAgentコンポーネント
    private NavMeshAgent agent;

    //巡回状態関連
    private Vector3 patrolTarget;
    //巡回地点をランダムに選ぶ範囲の半径
    [SerializeField] private float patrolRadius = 10f;
    //待機状態で止まる時間
    [SerializeField] private float patrolWaitTime = 2f;
    //待機中の経過時間フレーム
    private float patrolTimer = 0f;
    [SerializeField] private Vector3 patrolCenter; //巡回の中心点
    [SerializeField] private float patrolAreaRadius = 80f; //この範囲から出ない

    //Boids群れ制御関連
    [Header("Boids")]
    //他の敵との距離を保つ力の重み
    [SerializeField] private float separationWeight = 1.0f;
    //近くの敵と速度を合わせる力の重み
    [SerializeField] private float alignmentWeight = 0.5f;
    //群れの中心に向かう力の重み
    [SerializeField] private float cohesionWeight = 0.7f;
    //Boids 計算に参加する近くの敵の範囲
    [SerializeField] private float neighborRadius = 3.5f;
    //Boids 力の最大値
    [SerializeField] private float maxBoidsForce = 8f;
    //Boids 計算の更新間隔
    [SerializeField] private int boidsUpdateInterval = 3;
    //フレームカウンタ
    private int frameCounter = 0;
    //前回の Boids 力を保持し、更新間隔中は再利用
    private Vector3 lastBoidsForce;

    //移動線
    private LineRenderer line;

    //Alert
    [Header("Alert")]
    [SerializeField] private float alertRadius = 8f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = EnemyManager.Instance.GetPlayerTransform();
        EnemyManager.Instance.Register(this);
        state = EnemyState.Idle;
        Debug.Log("最初は待機状態へ");
        //LineRenderer 設定
        line = gameObject.AddComponent<LineRenderer>();
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.positionCount = 2;
        line.startColor = Color.red;
        line.endColor = Color.red;
        patrolWaitTime = Random.Range(2, 10);
        agent = GetComponent<NavMeshAgent>();
        agent.avoidancePriority = Random.Range(10, 90); // 0〜99 の範囲（小さいほど優先）
    }
    void Update()
    {
        ////デバッグ表示（実際のゲーム画面では見えないがSceneビューで見える）
        //Debug.DrawLine(transform.position, patrolTarget, Color.yellow);
        //Debug.DrawRay(patrolTarget, Vector3.up * 0.5f, Color.yellow);

        // 巡回目的地への線をゲーム内で表示
        if (patrolTarget != Vector3.zero)
        {
            line.SetPosition(0, transform.position + Vector3.up * 0.1f);
            line.SetPosition(1, patrolTarget + Vector3.up * 0.1f);
        }
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

        ////待機中は速度をゼロに(慣性をリセット）
        //agent.velocity = Vector3.zero;

        //待機状態で一定時間経過したら巡回へ
        if (patrolTimer > patrolWaitTime)
        {
            SetRandomPatrolPoint();
            state = EnemyState.Patrol;
            patrolTimer = 0f;
            patrolWaitTime = Random.Range(2, 10);
            Debug.Log("巡回状態へ");
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

        //経路計算中は待機
        if (agent.pathPending) return;

        //目的地に到達したら Idle に戻る
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            state = EnemyState.Idle;
            patrolTimer = 0f;
            Debug.Log("待機状態へ");
            return;
        }

        //Boids補正
        Vector3 boidsForce = GetBoidsForceOptimized() * 0.8f;

        ////NavMeshAgentが目指す目標方向を補正
        //Vector3 targetDir = (patrolTarget - transform.position).normalized;
        //Vector3 adjustedTarget = transform.position + (targetDir + boidsForce).normalized * 2f;

        //agent.SetDestination(adjustedTarget);

        // 次の目標位置をBoids補正で微調整
        Vector3 direction = (patrolTarget - transform.position).normalized + boidsForce;
        Vector3 adjustedPos = transform.position + direction.normalized * 2f;

        if (NavMesh.SamplePosition(adjustedPos, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
    //巡回範囲と目的地をGizmosで可視化
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(patrolCenter, patrolAreaRadius); // 巡回エリア

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, patrolRadius); // 1回分のランダム半径

        if (patrolTarget != Vector3.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(patrolTarget, 0.3f);
            Gizmos.DrawLine(transform.position, patrolTarget);
        }
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

        ////見失ったらIdle
        //if (!CanSeePlayer())
        //{
        //    state = EnemyState.Idle;
        //    Debug.Log("見失ったので待機状態へ");
        //}
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
        //中心からランダムに取得
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius + patrolCenter;
        randomDirection.y = transform.position.y; // 高さを固定（地面対応）

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = hit.position;
            agent.SetDestination(patrolTarget); //ここで一度だけセット
            Debug.Log($"新しいパトロール地点: {patrolTarget}");
        }
        else
        {
            Debug.LogWarning("有効なパトロール地点が見つかりません");
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
    //群れの近くの敵から
    Vector3 CalculateBoidsForce()
    {
        var neighbors = EnemyManager.Instance.GetNearbyEnemies(this, neighborRadius);
        if (neighbors.Count == 0) return Vector3.zero;

        Vector3 separation = Vector3.zero;//（距離保持）
        Vector3 alignment = Vector3.zero;//（速度合わせ）
        Vector3 cohesion = Vector3.zero;//（群れ中心へ）

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
