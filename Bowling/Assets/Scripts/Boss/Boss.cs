using UnityEngine;

public class Boss : MonoBehaviour
{
    //Š®‘S–³“G‚©‚Ç‚¤‚©
    bool isPerfectInvincible = true;

    //–³“G‚©‚Ç‚¤‚©
    bool isPerfect = false;

    //–³“GŠÔ
    float isInvincibleTime;

    int hp = 5;

    Transform player;

    private float floorAttackDispTimer;
    private bool isFloorAtackDisp = false;
    private float floorAttackTimer;
    private bool isFloorAtack = false;
    private float FloorAtackFinTimer;
    private bool isFloorAtackFin = true;

    bool isAttttttack = false;          //_‚Ìˆêè

    public int FloorAtkNum;

    GameObject[] floorAttackSub;          //°UŒ‚‘O‚ÌŠëŒ¯•\¦

    GameObject[] floorAttack;             //°UŒ‚

    Vector3[] PPos;

    public GameObject floorAttackSubPrefab;//°UŒ‚
    public GameObject floorAttackPrefab;   //°UŒ‚

    bool isDeath = false;

    //Å‘å–³“GŠÔ
    public float maxInvincibleTime;

    public void FalseIsPerfectInvincible()
    {
        isPerfectInvincible = false;
    }

    public bool ReturnDeath()
    {
        return isDeath;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PPos = new Vector3[FloorAtkNum];
        floorAttackSub = new GameObject[FloorAtkNum];
        floorAttack = new GameObject[FloorAtkNum];

        if (player == null)
            player = GameObject.FindWithTag("Player")?.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPerfect)
        {
            isInvincibleTime += Time.deltaTime;
        }

        if (isInvincibleTime > maxInvincibleTime)
        {
            isInvincibleTime = 0f;
            isPerfect = false;
        }

        if (hp <= 0)
        {
            isDeath = true;
            Destroy(gameObject);
        }

        //–³“G‚Å‚È‚¯‚ê‚ÎˆÈ‰º‚Ìˆ—‚ğs‚¤
        if (!isPerfectInvincible)
        {
            //UŒ‚—\‘ª•\¦ˆ—
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

            // UŒ‚—\‘ª•\¦’†‚È‚ç
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
                //‚±‚±‚ÅUŒ‚–{‘Ì‚ğ¶¬
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
                //‚±‚±‚ÅƒfƒXƒgƒƒC‚Ü‚Å‚Ìƒ^ƒCƒ}[‚ğ‰ñ‚·
                FloorAtackFinTimer += Time.deltaTime;
            }

            if (FloorAtackFinTimer > 5f)
            {
                //UŒ‚–{‘Ì‚ğE‚·
                for (int i = 0; i < FloorAtkNum; i++)
                {
                    Destroy(floorAttack[i]);
                }
                FloorAtackFinTimer = 0f;
                //UŒ‚I—¹‚µ‚½‚±‚Æ‚ğ“`‚¦‚é
                isFloorAtackFin = true;
                isAttttttack = false;
            }
        }

            //‰¼
        if (Input.GetKeyDown(KeyCode.V))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int i)
    {
        //–³“G‚Å‚ ‚ê‚ÎˆÈ‰º‚Ìˆ—‚ğs‚í‚È‚¢
        if (isPerfectInvincible) return;

        //–³“G’†‚Å‚È‚¯‚ê‚ÎUŒ‚‚ª—˜‚­
        if (!isPerfect)
        {
            hp -= i;
            Debug.Log(hp);
            isPerfect = true;
        }
    }

    private void RoundFloorAttack()
    {
        //ƒvƒŒƒCƒ„[‚ÌÀ•W‚Éo‚·
        for (int i = 0; i < FloorAtkNum; i++)
        {
            PPos[i] = new Vector3(player.position.x + Random.Range(-50, 50), 0.01f, player.position.z + Random.Range(-50, 50));
            floorAttackSub[i] = Instantiate(floorAttackSubPrefab, PPos[i], new Quaternion(0f, 0f, 0f, 0f));
        }
        isFloorAtackDisp = true;
    }
}