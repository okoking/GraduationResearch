using UnityEngine;
using System.Collections;
public class Boss : MonoBehaviour
{

    Transform player;

    private GameObject cube;

    private float floorAttackDispTimer;
    private bool isFloorAtackDisp = false;
    private float floorAttackTimer;
    private bool isFloorAtack = false;
    private float FloorAtackFinTimer;
    private bool isFloorAtackFin = true;

    bool isAttttttack = false;

    public int FloorAtkNum;

    GameObject[] floorAttack;             //床攻撃

    Vector3[] PPos;

    private BossHp bossHp;

    public GameObject floorAttackSubPrefab;//床攻撃
    public GameObject floorAttackPrefab;   //床攻撃

    [SerializeField] private Transform handLeft;
    [SerializeField] private Transform handRight;

    bool isWaveAttacking = false;

    float waveAttackTimer;

    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PPos = new Vector3[FloorAtkNum];
        floorAttack = new GameObject[FloorAtkNum];

        bossHp = GetComponent<BossHp>();

        animator = GetComponentInParent<Animator>();

        cube = GameObject.FindWithTag("BossCube");

        if (cube != null)
        {
            cube.transform.SetParent(transform, true);
        }

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

            //攻撃予測表示中なら
            if (isFloorAtackDisp)
            {
                animator.SetBool("isAttack", true);
                floorAttackDispTimer += Time.deltaTime;
                if (floorAttackDispTimer > 1.5f)
                {
                    floorAttackDispTimer = 0f;
                    isFloorAtack = true;
                    isFloorAtackDisp = false;
                }
            }

            if (isFloorAtack)
            {
                animator.SetBool("isAttack", false);
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

            waveAttackTimer += Time.deltaTime;

            if (!isWaveAttacking && waveAttackTimer > 10f)
            {
                //各地点でウェーブの攻撃を開始させる。
                StartCoroutine(WaveAttack(transform.position));
                StartCoroutine(WaveAttack(handLeft.position));
                StartCoroutine(WaveAttack(handRight.position));
                waveAttackTimer = 0;
            }

            if (bossHp.GetIsDeath())
            {
                for (int i = 0; i < FloorAtkNum; i++)
                {
                    Destroy(floorAttack[i]);
                }
                bossHp.ThisDestroy();
            }
        }
    }

    private void RoundFloorAttack()
    {
        //プレイヤーの座標に出す
        for (int i = 0; i < FloorAtkNum; i++)
        {
            PPos[i] = new Vector3(player.position.x + Random.Range(-50, 50), 0.01f, player.position.z + Random.Range(-50, 50));
            EffectManager.instance.Play("Ciecle", PPos[i]);
        }
        isFloorAtackDisp = true;
    }

    [SerializeField]
    GameObject wavePrefab;

    void SpawnWave(Vector3 pos)
    {
        GameObject wave = Instantiate(
            wavePrefab,
            new Vector3(pos.x, 0.5f, pos.z),
            Quaternion.Euler(90, 0, 0)
        );

        wave.GetComponent<WaveRing>()
            .Init(
                startRadius: 100f,
                speed: 800f,
                maxRadius: 10000f
            );
    }


    IEnumerator WaveAttack(Vector3 pos)
    {
        isWaveAttacking = true;

        float attackDuration = 5f;
        float interval = 5f;
        float timer = 0f;

        while (timer < attackDuration)
        {
            SpawnWave(pos);
            yield return new WaitForSeconds(interval);
            timer += interval;
        }

        isWaveAttacking = false;
    }
}