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

    bool isAttttttack = false;          //ê_ÇÃàÍéË

    public int FloorAtkNum;

    GameObject[] floorAttackSub;          //è∞çUåÇëOÇÃäÎåØï\é¶

    GameObject[] floorAttack;             //è∞çUåÇ

    Vector3[] PPos;

    private BossHp bossHp;

    public GameObject floorAttackSubPrefab;//è∞çUåÇ
    public GameObject floorAttackPrefab;   //è∞çUåÇ

    [SerializeField] private Transform handLeft;
    [SerializeField] private Transform handRight;

    bool isWaveAttacking = false;

    float waveAttackTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PPos = new Vector3[FloorAtkNum];
        floorAttackSub = new GameObject[FloorAtkNum];
        floorAttack = new GameObject[FloorAtkNum];

        bossHp = GetComponent<BossHp>();

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
        //ñ≥ìGÇ≈Ç»ÇØÇÍÇŒà»â∫ÇÃèàóùÇçsÇ§
        if (!bossHp.GetIsPerfectInvincible())
        {
            //çUåÇó\ë™ï\é¶èàóù
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

            // çUåÇó\ë™ï\é¶íÜÇ»ÇÁ
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
                //Ç±Ç±Ç≈çUåÇñ{ëÃÇê∂ê¨
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
                //Ç±Ç±Ç≈ÉfÉXÉgÉçÉCÇ‹Ç≈ÇÃÉ^ÉCÉ}Å[ÇâÒÇ∑
                FloorAtackFinTimer += Time.deltaTime;
            }

            if (FloorAtackFinTimer > 5f)
            {
                //çUåÇñ{ëÃÇéEÇ∑
                for (int i = 0; i < FloorAtkNum; i++)
                {
                    Destroy(floorAttack[i]);
                }
                FloorAtackFinTimer = 0f;
                //çUåÇèIóπÇµÇΩÇ±Ç∆Çì`Ç¶ÇÈ
                isFloorAtackFin = true;
                isAttttttack = false;
            }

            waveAttackTimer += Time.deltaTime;

            if (!isWaveAttacking && waveAttackTimer > 10f)
            {
                StartCoroutine(WaveAttack(transform.position));
                StartCoroutine(WaveAttack(handLeft.position));
                StartCoroutine(WaveAttack(handRight.position));
                waveAttackTimer = 0;
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
    }

    private void RoundFloorAttack()
    {
        //ÉvÉåÉCÉÑÅ[ÇÃç¿ïWÇ…èoÇ∑
        for (int i = 0; i < FloorAtkNum; i++)
        {
            PPos[i] = new Vector3(player.position.x + Random.Range(-50, 50), 0.01f, player.position.z + Random.Range(-50, 50));
            floorAttackSub[i] = Instantiate(floorAttackSubPrefab, PPos[i], new Quaternion(0f, 0f, 0f, 0f));
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