using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossHandR : MonoBehaviour
{
    Transform player;

    public GameObject beamSweepPrefab;     //実際のビーム
    public GameObject floorAttackSubPrefab;//床攻撃
    public GameObject floorAttackPrefab;   //床攻撃

    public float orbitRadius = 5f;
    public float orbitSpeed = 30f;
    public float floatAmplitude = 1f;
    public float floatSpeed = 2f;

    public float beamInterval = 5f;
    public float beamWarningTime = 1.5f;   //予兆線を表示しておく時間
    public float sweepAngle = 90f;         //薙ぎ払い角度

    private float angle;
    private float beamTimer;
    private bool isFiringBeam = false;

    private float floorAttackDispTimer;
    private bool isFloorAtackDisp = false;
    private float floorAttackTimer;
    private bool isFloorAtack = false;
    private float FloorAtackFinTimer;
    private bool isFloorAtackFin = true;

    bool isAttttttack = false;          //神の一手

    public int FloorAtkNum;

    GameObject[] floorAttack;             //床攻撃

    Vector3[] PPos;

    int[] off;

    float times;

    List<int> pausedIDs = new List<int>();

    [SerializeField] private Transform handRight;

    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player")?.transform;

        PPos = new Vector3[FloorAtkNum];
        floorAttack = new GameObject[FloorAtkNum];
        off = new int[FloorAtkNum];
    }

    void Update()
    {
        if (player == null) return;

        transform.LookAt(player);

        times += Time.deltaTime;
        if (times >= 5f)
        {
            StartCoroutine(ShootSweepBeam());
            times = 0f;
        }

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

            if (floorAttackDispTimer > 1.5f)
            {
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

    private IEnumerator ShootSweepBeam()
    {
        isFiringBeam = true;

        int segmentCount = 30;

        float displayTime = beamWarningTime;
        float halfAngle = sweepAngle / 2f;

        Vector3 origin = transform.position;

        // ビームの水平 forward（手の forward と一致）
        Vector3 flatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;

        for (int i = 0; i < segmentCount; i++)
        {
            float t = (float)i / (segmentCount - 1);
            float yaw = Mathf.Lerp(-halfAngle, halfAngle, t);
            Quaternion rot = Quaternion.Euler(0, yaw, 0);

            // ビームの方向（地面に対して少し下向き）
            Vector3 dir = rot * transform.forward;
            Vector3 start = transform.position;
        }

        // 警告時間待ち
        yield return new WaitForSeconds(displayTime);

        // 実際のビーム発射
        GameObject beam = Instantiate(beamSweepPrefab, transform.position, transform.rotation, transform);
        float beamDuration = beam.GetComponent<BeamSweepController>().duration;

        yield return new WaitForSeconds(beamDuration);
        isFiringBeam = false;
    }

    private void RoundFloorAttack()
    {
        //プレイヤーの座標に出す
        for (int i = 0; i < FloorAtkNum; i++)
        {
            PPos[i] = new Vector3(player.position.x + Random.Range(-20, 20), 0.01f, player.position.z + Random.Range(-20, 20));
            //floorAttackSub[i] = Instantiate(floorAttackSubPrefab, PPos[i], new Quaternion(0f, 0f, 0f, 0f));
            EffectManager.instance.Play("Ciecle", PPos[i]);
        }

        isFloorAtackDisp = true;
    }

    public void StartBeamAttack()
    {
        if (isFiringBeam) return;
        StartCoroutine(ShootSweepBeam());
    }
}