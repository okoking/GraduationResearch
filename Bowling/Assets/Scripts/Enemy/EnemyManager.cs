using System.Collections.Generic;
using UnityEngine;
 
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Transform player;

    private List<EnemyAI> enemies = new List<EnemyAI>();
    private AttackController attackController;

    void Awake()
    {
        if (Instance == null) Instance = this;

        //Awake でプレイヤー探索（Start より早い）
        //if (player == null)
        //{
        var p = GameObject.FindWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            Debug.Log("[EnemyManager] Player found in Awake.");
        }
        //}

        attackController = new AttackController();
    }

    void Start()
    {
        //if (player == null)
        //{
        //    var p = GameObject.FindWithTag("Player");
        //    if (p != null)
        //    {
        //        Debug.Log("プレイヤーが見つかりました");
        //        player = p.transform;
        //    }
        //    else
        //        Debug.Log("プレイヤーが見つかりません");
        //}
    }
    
    void Update()
    {
        attackController.Update();

<<<<<<< HEAD

    public Transform GetPlayerTransform() => playerTransform;
<<<<<<< HEAD
    {
=======
>>>>>>> parent of 79a22f5 (Revert "a")
        if (player == null)
        {
            Debug.Log("プレイヤーを取得できませんでした");
            Debug.Log("再度取得します。");
            var p = GameObject.FindWithTag("Player");
            if (p != null) player = p.transform;
            return;
        }
<<<<<<< HEAD
=======

        ////各敵を間引いて Update（distanceベース）
        //foreach (var e in enemies)
        //{
        //    if (e == null) continue;

        //    float dist = Vector3.Distance(player.position, e.transform.position);
        //    if (dist < 15f) e.enabled = true; //近距離は常に有効（ManagedUpdate を個別にしている場合はそれを呼ぶ）
        //    else if (dist < 30f) e.enabled = (Time.frameCount % 2 == 0);
        //    else e.enabled = (Time.frameCount % 5 == 0);
        //}
>>>>>>> parent of 79a22f5 (Revert "a")
    }

    public void RegisterEnemy(EnemyAI e)
    {
        e.Initialize(this, player, attackController);

        if (!enemies.Contains(e))
            enemies.Add(e);
    }

    public void Unregister(EnemyAI e) => enemies.Remove(e);
<<<<<<< HEAD

    public Transform GetPlayerTransform() => playerTransform;

    public Transform GetPlayerTransform() => playerTransform;

=======
>>>>>>> parent of 79a22f5 (Revert "a")
=======
>>>>>>> parent of 9b8b393 (a)

    public List<EnemyAI> GetNearbyEnemies(EnemyAI self, float radius)
    {
        List<EnemyAI> nearby = new List<EnemyAI>();
        foreach (var e in enemies)
        {
            if (e == null || e == self) continue;
            if (Vector3.Distance(e.transform.position, self.transform.position) < radius)
                nearby.Add(e);
        }
        return nearby;
    }

    //近くにいる敵に警告をだす
    public void AlertNearby(EnemyAI sender, float alertRadius)
    {
        foreach (var e in enemies)
        {
            if (e == sender || e == null) continue;
            if (Vector3.Distance(e.transform.position, sender.transform.position) <= alertRadius)
            {
                //ビックリマーク表示
                e.ShowAlert();
                //強制 Chase にする
                e.ChangeState(new ChaseState(e));
            }
        }
    }

    public int NumEnemiesAttackingNearby()
    {
        return attackController.CurrentAttacking;
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

    //取得関数
    public Transform Player => player;
    public AttackController AttackController => attackController;
}