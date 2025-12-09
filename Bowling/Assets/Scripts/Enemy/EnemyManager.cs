using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EnemyAI;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    private Transform playerTransform;
    private readonly List<EnemyAI> enemies = new();

    [Header("攻撃管理")]
    public int maxAttacker = 3;         //同時に攻撃できる人数
    private int currentAttacking = 0;   //今攻撃している敵の人数
    private Queue<EnemyAI> attackQueue = new();

    bool[] wasFar = new bool[2];

    [Header("攻撃テンポ調整")]
    public float globalAttackCooldown = 2.5f; //全体クールダウン
    private float globalAttackTimer = 0f;

    [Header("ラッシュ攻撃（複数同時攻撃）")]
    public float rushChance = 0.1f;     //10%の確率でラッシュ
    public float rushDuration = 4f;     //ラッシュ継続時間
    public float rushCooldown = 10f;    //ラッシュとラッシュの間隔
    public bool isRush = false;
    private float rushTimer = 0f;
    private float rushCooldownTimer = 0f;

    private int baseMaxAttacker;

    void Awake()
    {
        Instance = this;
        baseMaxAttacker = maxAttacker;
    }

    public void Register(EnemyAI enemy)
    {
        enemies.Add(enemy);
        if (playerTransform == null)
        {
           //Debug.Log("プレイヤーがありません");

           //Debug.Log("プレイヤーを取得します");
           //Instance.SetPlayer(
           //GameObject.Find("Player").transform);
        }
        //プレイヤーが既に存在していれば EnemyAI にセット
        if (playerTransform != null)
            Debug.Log("プレイヤーを取得成功");
        enemy.SetPlayer(playerTransform);
    }
    public void SetPlayer(Transform player)
    {
        playerTransform = player;

        //登録済み EnemyAI に player を通知
        foreach (var e in enemies)
            e.SetPlayer(playerTransform);
    }
    public void Unregister(EnemyAI enemy) => enemies.Remove(enemy);

<<<<<<< HEAD
<<<<<<< Updated upstream
    public Transform GetPlayerTransform() => playerTransform;
=======
        if (player == null)
        {
            Debug.Log("プレイヤーを取得できませんでした");
            Debug.Log("再度取得します。");
            var p = GameObject.FindWithTag("Player");
            if (p != null) player = p.transform;
            return;
        }
    }

    public void RegisterEnemy(EnemyAI e)
    {
        e.Initialize(this, player, attackController);

        if (!enemies.Contains(e))
            enemies.Add(e);
    }

    public void Unregister(EnemyAI e) => enemies.Remove(e);
>>>>>>> Stashed changes
=======
    public Transform GetPlayerTransform() => playerTransform;
>>>>>>> parent of bb1ebc5 (a)

    //近くの敵を取得（Boids用）
    public List<EnemyAI> GetNearbyEnemies(EnemyAI self, float radius)
    {
        List<EnemyAI> nearby = new();
        foreach (var e in enemies)
        {
            if (e == self) continue;
            if (Vector3.Distance(e.transform.position, self.transform.position) < radius)
                nearby.Add(e);
        }
        return nearby;
    }

    //警報共有：周囲の敵に追跡を通知
    public void AlertNearbyEnemies(EnemyAI sender, float alertRadius)
    {
        foreach (var e in enemies)
        {
            if (e == sender) continue;
            if (Vector3.Distance(e.transform.position, sender.transform.position) <= alertRadius)
            {
                e.OnAlerted();
            }
        }
    }

    void Update()
    {
        //グローバルクールダウン時間経過
        if (globalAttackTimer > 0f)
            globalAttackTimer -= Time.deltaTime;

        //ラッシュ攻撃クールダウン
        if (!isRush)
        {
            rushCooldownTimer -= Time.deltaTime;

            //ラッシュ可能 & 抽選
            if (rushCooldownTimer <= 0f)
            {
                if (Random.value < rushChance)
                {
                    StartRush();
                }
                else
                {
                    rushCooldownTimer = 5f; //再抽選までの猶予
                }
            }
        }
        else
        {
            // ラッシュ継続時間
            rushTimer -= Time.deltaTime;
            if (rushTimer <= 0f)
                EndRush();
        }

        if (playerTransform == null)
        {
            Debug.Log("プレイヤー情報がないため処理を終了しました");
            SetPlayer(GameObject.Find("Player").transform);

            if(playerTransform != null)
                Debug.Log("プレイヤー情報を取得しました");
            else
                Debug.Log("プレイヤー情報を取得できませんでした");
            return;
        }

        foreach (var e in enemies)
        {
            float dist = Vector3.Distance(playerTransform.position, e.transform.position);
            if (dist < 15f) e.ManagedUpdate();       //近距離は毎フレーム
            else if (dist < 30f) { if (Time.frameCount % 2 == 0) e.ManagedUpdate(); }  //中距離
            else { if (Time.frameCount % 5 == 0) e.ManagedUpdate(); }                  //遠距離
        }
    }
    public bool IsFrontEnemyAttacking(Transform enemy, Transform player, float frontAngle = 1f)
    {
        foreach (var e in enemies)
        {
            if (e == null || e == enemy) continue;
            if (e.state != EnemyAI.EnemyState.Attack) continue;

            //この敵が player 方向の前方にいるかチェック
            Vector3 toThisEnemy = e.transform.position - player.position;
            Vector3 toQuery = enemy.position - player.position;

            float angle = Vector3.Angle(toThisEnemy, toQuery);
            if (angle < frontAngle)
            {
                //同じ方向（前方ライン上）に攻撃中の敵がいる
                return true;
            }
        }
        return false;
    }
    public int IsEnemyAttacking(Transform enemy, Transform player)
    {
        int attackingEnemyCount = 0;

        foreach (var e in enemies)
        {
            if (e == null) continue;

            //e.ManagedUpdate();

            // 攻撃状態の敵をカウント
            if (e.state == EnemyState.Attack)
            {
                attackingEnemyCount++;
            }
        }

        return attackingEnemyCount;
    }
    public bool TryRequestAttack(EnemyAI requester)
    {
        //グローバルクールダウン中なら誰も攻撃できない
        if (!isRush && globalAttackTimer > 0f)
        {
            return false;
        }

        //ラッシュ時はクールダウン無効 & maxAttacker を拡張

        //キューに未登録なら入れる
        if (!attackQueue.Contains(requester))
            attackQueue.Enqueue(requester);

        //上限チェック
        if (currentAttacking >= maxAttacker)
            return false;

        //自分の番でなければ待機
        if (attackQueue.Peek() != requester)
            return false;

        //攻撃を許可
        attackQueue.Dequeue();
        currentAttacking++;

        //攻撃発動 → 全体クールダウンスタート（ラッシュ以外）
        if (!isRush)
            globalAttackTimer = globalAttackCooldown;

        return true;
    }

    //攻撃終了（枠を戻す）
    public void EndAttack(EnemyAI enemy)
    {
        currentAttacking = Mathf.Max(0, currentAttacking - 1);

        //次の攻撃者に枠を譲るため、自動的に処理
        if (attackQueue.Count > 0)
        {
            //次の敵に攻撃資格が生まれる
            //EnemyAI 自身の Update で attackRange に入っていたら攻撃が始まる
        }
    }

    //条件に入った瞬間だけ trueを返す
    bool CheckOneShot(ref bool flag, bool condition)
    {
        if (condition && !flag)
        {
            flag = true;
            return true;
        }
        if (!condition)
        {
            flag = false;
        }
        return false;
    }
    void StartRush()
    {
        Debug.Log("🔥 ラッシュ攻撃開始！");

        isRush = true;
        rushTimer = rushDuration;

        //maxAttacker を増やす
        maxAttacker = baseMaxAttacker;

        //次のラッシュまでのクールダウン
        rushCooldownTimer = rushCooldown;

        //グローバルクールダウンは無効化
        globalAttackTimer = 0f;
    }

    void EndRush()
    {
        Debug.Log("ラッシュ攻撃終了");

        isRush = false;

        //maxAttacker を元に戻す
        maxAttacker = baseMaxAttacker;
    }
}
