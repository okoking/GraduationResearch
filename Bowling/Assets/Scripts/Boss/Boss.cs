using UnityEngine;
public class Boss : MonoBehaviour
{

    Transform player;

    private float floorAttackDispTimer;
    private bool isFloorAtackDisp = false;
    private float floorAttackTimer;
    private bool isFloorAtack = false;
    private float FloorAtackFinTimer;
    private bool isFloorAtackFin = true;

    bool isAttttttack = false;          //神の一手

    public int FloorAtkNum;

    GameObject[] floorAttackSub;          //床攻撃前の危険表示

    GameObject[] floorAttack;             //床攻撃

    Vector3[] PPos;

    private BossHp bossHp;

    public GameObject floorAttackSubPrefab;//床攻撃
    public GameObject floorAttackPrefab;   //床攻撃

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PPos = new Vector3[FloorAtkNum];
        floorAttackSub = new GameObject[FloorAtkNum];
        floorAttack = new GameObject[FloorAtkNum];

        bossHp = GetComponent<BossHp>();

        if (player == null)
            player = GameObject.FindWithTag("Player")?.transform;
    }


    // Update is called once per frame
    void Update()
    {
        //無敵でなければ以下の処理を行う
        if (!bossHp.GetIsPerfectInvincible())
        {
            //攻撃予測表示処理
            if (!isFloorAtackDisp && isFloorAtackFin)
            {
                floorAttackTimer += Time.deltaTime;
            }

            if (floorAttackTimer >= 5f)
            {
                RoundFloorAttack();
                isFloorAtackFin = false;
                floorAttackTimer = 0f;
            }

            // 攻撃予測表示中なら
            if (isFloorAtackDisp)
            {
                floorAttackDispTimer += Time.deltaTime;
                if (floorAttackDispTimer > 2f)
                {
                    for (int i = 0; i < FloorAtkNum; i++)
                    {
                        Destroy(floorAttackSub[i]);
                    }
                    floorAttackDispTimer = 0f;
                    isFloorAtack = true;
                    isFloorAtackDisp = false;
                }
            }

            if (isFloorAtack)
            {
                //ここで攻撃本体を生成
                for (int i = 0; i < FloorAtkNum; i++)
                {
                    floorAttack[i] = Instantiate(floorAttackPrefab, PPos[i], new Quaternion(0f, 0f, 0f, 0f));
                    EffectManager.instance.Play("meteor", PPos[i]);
                }

                isFloorAtack = false;
                isAttttttack = true;
            }

            if (isAttttttack)
            {
                //ここでデストロイまでのタイマーを回す
                FloorAtackFinTimer += Time.deltaTime;
            }

            if (FloorAtackFinTimer > 5f)
            {
                //攻撃本体を殺す
                for (int i = 0; i < FloorAtkNum; i++)
                {
                    Destroy(floorAttack[i]);
                }
                FloorAtackFinTimer = 0f;
                //攻撃終了したことを伝える
                isFloorAtackFin = true;
                isAttttttack = false;
            }
        }

            //仮
        if (Input.GetKeyDown(KeyCode.V))
        {
            bossHp.TakeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            bossHp.Recovery(10);
        }

        if (bossHp.GetIsDeath())
        {
            for (int i = 0; i < FloorAtkNum; i++)
            {
                Destroy(floorAttackSub[i]);
                Destroy(floorAttack[i]);
            }
            bossHp.ThisDestroy();
        }
    }

    private void RoundFloorAttack()
    {
        //プレイヤーの座標に出す
        for (int i = 0; i < FloorAtkNum; i++)
        {
            PPos[i] = new Vector3(player.position.x + Random.Range(-50, 50), 0.01f, player.position.z + Random.Range(-50, 50));
            floorAttackSub[i] = Instantiate(floorAttackSubPrefab, PPos[i], new Quaternion(0f, 0f, 0f, 0f));
        }
        isFloorAtackDisp = true;
    }
}