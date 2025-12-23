using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BeamSweepController : MonoBehaviour
{
    public float duration = 2f;        // ビームが出ている時間
    public float sweepAngle = 90f;     // 振り幅
    public float sweepSpeed = 90f;     // 回転速度
    public float beamLength = 50f;     // 最大長さ
    public float beamWidth = 0.2f;     // 太さ
    public LayerMask groundLayer;      // 地面レイヤーを設定（例：Default）

    private LineRenderer line;
    private float timer;
    private float currentAngle;
    private bool sweepingRight = true;

    private PlayerHealth playerHealth;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.startWidth = beamWidth;
        line.endWidth = beamWidth;
        line.useWorldSpace = true;

        currentAngle = -sweepAngle / 2f;

        Destroy(gameObject, duration);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 回転角度更新
        float step = sweepSpeed * Time.deltaTime * (sweepingRight ? 1 : -1);
        currentAngle += step;
        if (Mathf.Abs(currentAngle) >= sweepAngle / 2f)
            sweepingRight = !sweepingRight;

        // 回転方向計算
        Quaternion rot = Quaternion.Euler(0, currentAngle, 0);
        Vector3 dir = rot * transform.forward;

        // 地面ヒットチェック
        Vector3 start = transform.position;
        Vector3 hitPoint = start + dir * beamLength; // デフォルトは最大距離

        if (Physics.Raycast(start, dir, out RaycastHit hit, beamLength, groundLayer))
        {
            hitPoint = hit.point;
        }

        RaycastHit[] hits = Physics.SphereCastAll(start, beamWidth, dir, beamLength);
        foreach (var h in hits)
        {
            if (h.collider.CompareTag("Player"))
            {
                playerHealth.TakeDamage(1);
                EffectManager.instance.Play("BeamColl",h.transform.position);
            }
        }

        // LineRenderer更新
        line.SetPosition(0, start);
        line.SetPosition(1, hitPoint);
    }
}

//using UnityEngine;
//using System.Collections;

//public class KariBeam : MonoBehaviour
//{
//    [SerializeField] private LineRenderer lineRenderer;
//    [SerializeField] private float beamLength = 50f;
//    [SerializeField] private float beamWidth = 0.2f;
//    [SerializeField] private float MegabeamDuration = 3f; // 何秒間出し続けるか
//    [SerializeField] private float MinibeamDuration = 1.5f; // 何秒間出し続けるか
//    [SerializeField] private LayerMask hitMask;
//    [SerializeField] private Camera mainCam;
//    public bool disableRotate;

//    private bool isFiring = false;
//    private BeamCamera beamCamera;
//    private LockOnSystem lockOn;

//    private blockSpawn BlockHitPoint;

//    public GameObject WallObj;

//    void Start()
//    {
//        lineRenderer.enabled = false;
//        lineRenderer.startWidth = beamWidth;
//        lineRenderer.endWidth = beamWidth;
//        beamCamera = GetComponent<BeamCamera>();
//        lockOn = GetComponent<LockOnSystem>();

//        BlockHitPoint = WallObj.GetComponent<blockSpawn>();
//    }

//    void Update()
//    {
//        // ボタン押したらビーム発射
//        if (Input.GetKeyDown("joystick button 5") && !isFiring)
//        {
//            if (beamCamera.isSootBeam)
//            {
//                StartCoroutine(FireBeam());
//            }
//            else
//            {
//                StartCoroutine(FireLockOnBeam());
//            }
//        }
//    }

//    IEnumerator FireBeam()
//    {
//        isFiring = true;
//        disableRotate = true;
//        lineRenderer.enabled = true;

//        float timer = 0f;
//        while (timer < MegabeamDuration)
//        {
//            Vector3 start = transform.position;
//            start.y += 1;
//            Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 中央(0.5,0.5)
//            Vector3 end = ray.origin + ray.direction * beamLength;

//            // ★ 撃っている方向にプレイヤーを向ける（水平だけ）
//            Vector3 lookDir = ray.direction;
//            lookDir.y = 0;
//            Quaternion targetRot = Quaternion.LookRotation(lookDir);
//            transform.rotation = Quaternion.Slerp(
//                transform.rotation,
//                targetRot,
//                10.0f * Time.deltaTime
//            );
//            // Raycastで命中判定
//            if (Physics.SphereCast(start, beamWidth, transform.forward, out RaycastHit hit, beamLength))
//            {
//                // end = hit.point;
//                // 当たった敵に処理
//                if (hit.collider.CompareTag("Enemy"))
//                {
//                    Debug.Log("敵にヒット！: " + hit.collider.name);
//                    BlockHitPoint.TakeDamage(50);
//                    // EnemyスクリプトのTakeDamageを呼ぶなど
//                    // hit.collider.GetComponent<Enemy>()?.TakeDamage(10);
//                }
//            }

//            lineRenderer.SetPosition(0, start);
//            lineRenderer.SetPosition(1, end);

//            timer += Time.deltaTime;
//            yield return null;
//        }

//        lineRenderer.enabled = false;
//        disableRotate = false;
//        isFiring = false;
//    }

//    IEnumerator FireLockOnBeam()
//    {
//        if (lockOn != null && lockOn.lockOnTarget != null)
//        {
//            isFiring = true;
//            lineRenderer.enabled = true;

//            float timer = 0f;
//            while (timer < MinibeamDuration)
//            {
//                if (lockOn.lockOnTarget == null)
//                {
//                    isFiring = false;                 // ビーム発射フラグをOFF
//                    lineRenderer.enabled = false;     // LineRendererを非表示
//                    StopAllCoroutines();              // 発射中のIEnumeratorを止める
//                    yield break;
//                }

//                Vector3 start = transform.position;
//                start.y += 1;
//                Vector3 end = lockOn.lockOnTarget.position;

//                Vector3 direction = (end - start).normalized;

//                float Distance = Vector3.Distance(end, start);

//                RaycastHit[] hits = Physics.SphereCastAll(start, beamWidth, direction, Distance);
//                foreach (var h in hits)
//                {
//                    if (h.collider.CompareTag("Enemy"))
//                        Debug.Log("敵に貫通ヒット: " + h.collider.name);
//                    BlockHitPoint.TakeDamage(50);
//                }

//                //// raycastで命中判定
//                //if (physics.spherecast(start, beamwidth, direction, out raycasthit hit, distance))
//                //{
//                //    // end = hit.point;
//                //    // 当たった敵に処理
//                //    //if (hit.collider.comparetag("enemy"))
//                //    //{
//                //    //    debug.log("敵にヒット！: " + hit.collider.name);
//                //    //    // enemyスクリプトのtakedamageを呼ぶなど
//                //    //    // hit.collider.getcomponent<enemy>()?.takedamage(10);
//                //    //}
//                //}

//                lineRenderer.SetPosition(0, start);
//                lineRenderer.SetPosition(1, end);

//                timer += Time.deltaTime;
//                yield return null;
//            }

//            lineRenderer.enabled = false;
//            isFiring = false;
//        }
//    }

//    //void CollisionBeam()
//    //{
//    //    // Ray（始点、方向）
//    //    Ray ray = new Ray(transform.position, transform.forward);
//    //    RaycastHit hit;

//    //    // LineRendererの始点設定
//    //    lineRenderer.SetPosition(0, transform.position);

//    //    // もし何かに当たったら
//    //    if (Physics.Raycast(ray, out hit, beamLength, hitMask))
//    //    {
//    //        // LineRendererの終点を当たった位置に
//    //        lineRenderer.SetPosition(1, hit.point);

//    //        // 敵に当たった場合の処理
//    //        if (hit.collider.CompareTag("Enemy"))
//    //        {
//    //            Debug.Log("敵にヒット！: " + hit.collider.name);
//    //            // 例：敵のHPを減らす処理など
//    //            // hit.collider.GetComponent<Enemy>().TakeDamage(10);
//    //        }
//    //    }
//    //    else
//    //    {
//    //        // 何にも当たらなかった場合は最大距離まで
//    //        lineRenderer.SetPosition(1, transform.position + transform.forward * beamLength);
//    //    }
//    //}
//}

