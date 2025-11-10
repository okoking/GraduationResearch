using UnityEngine;
using System.Collections;

public class BossHand : MonoBehaviour
{
    Transform player;
    public Transform bossHandSpawn;

    public GameObject beamSweepPrefab;     //実際のビーム
    public GameObject floorAttackSubPrefab;//床攻撃
    public GameObject floorAttackPrefab;   //床攻撃
    public LineRenderer aimLinePrefab;     //地面に出す予兆線

    public float orbitRadius = 5f;
    public float orbitSpeed = 30f;
    public float floatAmplitude = 1f;
    public float floatSpeed = 2f;

    public float beamInterval = 5f;
    public float beamWarningTime = 1.5f;   //予兆線を表示しておく時間
    public float sweepAngle = 90f;         //薙ぎ払い角度
    public float sweepSpeed = 60f;

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

    GameObject floorAttackSub;      //床攻撃前の危険表示

    GameObject floorAttack;         //床攻撃

    private Boss boss;

    public int hp = 50;

    private bool death = false;

    Vector3 PPos;

    void Start()
    {

        boss = FindAnyObjectByType<Boss>();

        if (player == null)
            player = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        if (bossHandSpawn == null || player == null) return;

        //ビーム中でなければ動く
        if (!isFiringBeam)
        {
            //手は軌道を描きながら動く、プレイヤーを向き続ける
            angle += orbitSpeed * Time.deltaTime;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(
                Mathf.Cos(rad) * orbitRadius,
                Mathf.Sin(Time.time * floatSpeed) * floatAmplitude,
                Mathf.Sin(rad) * orbitRadius
            );
            //スポーン座標を基準に回る
            transform.position = bossHandSpawn.position + offset;
            transform.LookAt(player);
        }

        beamTimer += Time.deltaTime;
        if (beamTimer >= beamInterval && !isFiringBeam)
        {
            beamTimer = 0f;
            StartCoroutine(ShootSweepBeam());
        }

        //攻撃予測表示処理
        if (!isFloorAtackDisp && isFloorAtackFin) {
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
            if(floorAttackDispTimer > 2f)
            {
                floorAttackDispTimer = 0f;
                Destroy(floorAttackSub);
                isFloorAtack = true;
                isFloorAtackDisp = false;
            }
        }

        if (isFloorAtack)
        {
            //ここで攻撃本体を生成
            floorAttack = Instantiate(floorAttackPrefab, PPos, new Quaternion(0f, 0f, 0f, 0f));

            isFloorAtack = false;
            isAttttttack = true;
        }

        if (isAttttttack)
        {
            //ここでデストロイまでのタイマーを回す
            FloorAtackFinTimer += Time.deltaTime;
        }

        if(FloorAtackFinTimer > 5f)
        {
            //攻撃本体を殺す
            Destroy(floorAttack);
            FloorAtackFinTimer = 0f;
            //攻撃終了したことを伝える
            isFloorAtackFin = true;
            isAttttttack = false;
        }

        //手を殺すための仮コード
        if (Input.GetKeyUp(KeyCode.H))
        {
            hp--;
            Debug.Log(hp);
        }

        //手のHPが0以下なら死んだフラグを立てる
        if(hp < 0)
        {
            death = true;
        }

        //死んだフラグがたったら
        if (death)
        {
            //ボスの完全無敵状態を解除する
            boss.FalseIsPerfectInvincible();
        }
    }

    private IEnumerator ShootSweepBeam()
    {
        isFiringBeam = true;

        LineRenderer line = Instantiate(aimLinePrefab);
        int segmentCount = 60;
        line.positionCount = segmentCount;

        float beamRange = 50f; // BeamSweepController の beamLength と同じ値に
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

            if (Physics.Raycast(start, dir, out RaycastHit hit, beamRange, LayerMask.GetMask("Ground")))
            {
                // 地面から少し上にオフセット
                line.SetPosition(i, hit.point + Vector3.up * 0.05f);
            }
            else
            {
                if (Physics.Raycast(start + dir * beamRange, Vector3.down, out RaycastHit downHit, 100f, LayerMask.GetMask("Ground")))
                {
                    line.SetPosition(i, downHit.point + Vector3.up * 0.05f);
                }
                else
                {
                    line.SetPosition(i, start + dir * beamRange);
                }
            }
        }

        // 見た目設定
        line.startWidth = 0.15f;
        line.endWidth = 0.15f;
        line.material.color = new Color(1, 1, 0, 1f);

        // 警告時間待ち
        yield return new WaitForSeconds(displayTime);

        // 実際のビーム発射
        GameObject beam = Instantiate(beamSweepPrefab, transform.position, transform.rotation);
        float beamDuration = beam.GetComponent<BeamSweepController>().duration;

        yield return new WaitForSeconds(beamDuration);

        Destroy(line.gameObject);
        isFiringBeam = false;
    }

    private void RoundFloorAttack()
    {
        //プレイヤーの座標に出す
        PPos = new Vector3(player.position.x, 0.0f, player.position.z);
        isFloorAtackDisp = true;
        floorAttackSub = Instantiate(floorAttackSubPrefab, PPos, new Quaternion(0f, 0f, 0f, 0f));
    }
}