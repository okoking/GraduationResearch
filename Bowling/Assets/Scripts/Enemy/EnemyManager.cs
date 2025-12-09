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
    }
    void Update()
    {
        attackController.Update();
        if (player == null)
        {
            Debug.Log("プレイヤーを取得できませんでした");
            Debug.Log("再度取得します。");
            var p = GameObject.FindWithTag("Player");
            if (p != null) player = p.transform;
            return;
        }
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
        foreach (var e in enemies)
        {
            if (Vector3.Distance(e.transform.position, self.transform.position) < radius)
                nearby.Add(e);
        }
        return nearby;
    }

    public void RegisterEnemy(EnemyAI e)
    {
        e.Initialize(this, player, attackController);
        if (!enemies.Contains(e))
            enemies.Add(e);
    }
    //public int NumEnemiesAttackingNearby()
    //{ return attackController.CurrentAttacking; }

    //取得関数
    public Transform Player => player;
    public AttackController AttackController => attackController;
}