using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Transform player;
    [Header("最大同時攻撃数")]
    [SerializeField] private int baseMaxAttacker = 3;
    private List<EnemyAI> enemies = new List<EnemyAI>();
    private AttackController attackController;

    void Awake()
    {
        
    }
    void Start()
    {
        if (player == null)
        {
            var p = GameObject.FindWithTag("Player");
            if (p != null)
            {
                Debug.Log("プレイヤーが見つかりました");
                player = p.transform;
            }
            else
                Debug.Log("プレイヤーが見つかりません");
        }

        if (Instance == null) Instance = this;
        //Awake でプレイヤー探索（Start より早い）
        if (player == null)
        {
            var p = GameObject.FindWithTag("Player");
            if (p != null)
            {
                player = p.transform;
                Debug.Log("[EnemyManager] Player found in Awake.");
            }
        }
        attackController = new AttackController();
    }
    void Update()
    {
        attackController.Update();
        //if (player == null)
        //{
        //    Debug.Log("プレイヤーを取得できませんでした");
        //    Debug.Log("再度取得します。");
        //    var p = GameObject.FindWithTag("Player");
        //    if (p != null) player = p.transform;
        //    return;
        //}

        //if (player == null)
        //{
        //    Debug.Log("プレイヤー情報がないため処理を終了しました");
        //    SetPlayer(GameObject.Find("Player").transform);
        //    if (player != null)
        //        Debug.Log("プレイヤー情報を取得しました");
        //    else
        //        Debug.Log("プレイヤー情報を取得できませんでした");
        //    return;
        //}

       
    }

    //近くにいる敵に警告をだす
    public void AlertNearby(EnemyAI sender, float alertRadius) 
    {
        foreach (var e in enemies) 
        { 
            if (e == sender || e == null) continue;
            if (Vector3.Distance(e.transform.position,
                sender.transform.position) <= alertRadius)
            {
                //ビックリマーク表示
                e.ShowAlert();
                //強制 Chase にする
                e.ChangeState(new ChaseState(e));
            }
        }
    }

    public bool IsFrontEnemyAttacking(Transform enemy, Transform player, float frontAngle = 1f)
    {
        foreach (var e in enemies)
        {
            if (e == null || e == enemy) continue;
            if (e.CurrentStateType == StateType.Attack) continue;
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
    public int IsEnemyAttacking()
    {
        int count = 0;

        foreach (var e in enemies)
        {
            if (e == null) continue;
            if (e.CurrentStateType == StateType.Attack)
                count++;
        }
        return count;
    }

    public List<EnemyAI> GetNearbyEnemies(EnemyAI self, float radius)
    {
        List<EnemyAI> nearby = new List<EnemyAI>();
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            var e = enemies[i];

            // Destroy済み or null を削除
            if (e == null)
            {
                enemies.RemoveAt(i);
                continue;
            }

            if (e == self) continue;

            if (Vector3.Distance(e.transform.position, self.transform.position) < radius)
            {
                nearby.Add(e);
            }
        }
        return nearby;
    }

    public void RegisterEnemy(EnemyAI e)
    {
        //e.Initialize(this, player, attackController);
        if (!enemies.Contains(e))
            enemies.Add(e);
    }
    public void UnregisterEnemy(EnemyAI e)
    {
        if (enemies.Contains(e))
        {
            enemies.Remove(e);
        }
    }
    //public int NumEnemiesAttackingNearby()
    //{ return attackController.CurrentAttacking; }

    public void SetPlayer(Transform Player)
    {
        player = Player;
        //登録済み EnemyAI に player を通知
        foreach (var e in enemies) e.SetPlayer(player);
    }

    //取得関数
    public Transform Player => player;
    public AttackController AttackController => attackController;
}