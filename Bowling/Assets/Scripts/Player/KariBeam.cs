using UnityEngine;
using System.Collections;
using static UnityEngine.UI.Image;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

public class KariBeam : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float beamLength = 50f;
    [SerializeField] private float beamWidth = 0.2f;
    [SerializeField] private float MegabeamDuration = 3f; // 何秒間出し続けるか
    [SerializeField] private float MinibeamDuration = 1.5f; // 何秒間出し続けるか
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private Camera mainCam;

    private bool isFiring = false;
    private BeamCamera beamCamera;
    private LockOnSystem lockOn;

    void Start()
    {
        lineRenderer.enabled = false;
        lineRenderer.startWidth = beamWidth;
        lineRenderer.endWidth = beamWidth;
        beamCamera = GetComponent<BeamCamera>();
        lockOn = GetComponent<LockOnSystem>();
    }

    void Update()
    {
        // ボタン押したらビーム発射
        if (Input.GetKeyDown("joystick button 5") && !isFiring)
        {
            if (beamCamera.isSootBeam)
            {
                StartCoroutine(FireBeam());
            }
            else
            {
                StartCoroutine(FireLockOnBeam());
            }
        }
    }

    IEnumerator FireBeam()
    {
        isFiring = true;
        lineRenderer.enabled = true;

        float timer = 0f;
        while (timer < MegabeamDuration)
        {
            Vector3 start = transform.position;
            start.y += 1;
            Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 中央(0.5,0.5)
            Vector3 end = ray.origin + ray.direction * beamLength;

            // Raycastで命中判定
            if (Physics.SphereCast(start, beamWidth,transform.forward, out RaycastHit hit, beamLength))
            {
                // end = hit.point;
                // 当たった敵に処理
                if (hit.collider.CompareTag("Enemy"))
                {
                    Debug.Log("敵にヒット！: " + hit.collider.name);
                    // EnemyスクリプトのTakeDamageを呼ぶなど
                    // hit.collider.GetComponent<Enemy>()?.TakeDamage(10);
                }
            }

            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);

            timer += Time.deltaTime;
            yield return null;
        }

        lineRenderer.enabled = false;
        isFiring = false;
    }
    
    IEnumerator FireLockOnBeam()
    {
        if (lockOn != null && lockOn.lockOnTarget != null)
        {
            isFiring = true;
            lineRenderer.enabled = true;

            float timer = 0f;
            while (timer < MinibeamDuration)
            {
                if (lockOn.lockOnTarget == null)
                {
                    isFiring = false;                 // ビーム発射フラグをOFF
                    lineRenderer.enabled = false;     // LineRendererを非表示
                    StopAllCoroutines();              // 発射中のIEnumeratorを止める
                    yield break;
                }

                Vector3 start = transform.position;
                start.y += 1;
                Vector3 end = lockOn.lockOnTarget.position;

                Vector3 direction = (end - start).normalized;

                float Distance = Vector3.Distance(end, start);

                RaycastHit[] hits = Physics.SphereCastAll(start, beamWidth, direction, Distance);
                foreach (var h in hits)
                {
                    if (h.collider.CompareTag("Enemy"))
                        Debug.Log("敵に貫通ヒット: " + h.collider.name);
                }

                //// Raycastで命中判定
                //if (Physics.SphereCast(start, beamWidth, direction, out RaycastHit hit, Distance))
                //{
                //    // end = hit.point;
                //    // 当たった敵に処理
                //    //if (hit.collider.CompareTag("Enemy"))
                //    //{
                //    //    Debug.Log("敵にヒット！: " + hit.collider.name);
                //    //    // EnemyスクリプトのTakeDamageを呼ぶなど
                //    //    // hit.collider.GetComponent<Enemy>()?.TakeDamage(10);
                //    //}
                //}

                lineRenderer.SetPosition(0, start);
                lineRenderer.SetPosition(1, end);

                timer += Time.deltaTime;
                yield return null;
            }

            lineRenderer.enabled = false;
            isFiring = false;
        }
    }

    //void CollisionBeam()
    //{
    //    // Ray（始点、方向）
    //    Ray ray = new Ray(transform.position, transform.forward);
    //    RaycastHit hit;

    //    // LineRendererの始点設定
    //    lineRenderer.SetPosition(0, transform.position);

    //    // もし何かに当たったら
    //    if (Physics.Raycast(ray, out hit, beamLength, hitMask))
    //    {
    //        // LineRendererの終点を当たった位置に
    //        lineRenderer.SetPosition(1, hit.point);

    //        // 敵に当たった場合の処理
    //        if (hit.collider.CompareTag("Enemy"))
    //        {
    //            Debug.Log("敵にヒット！: " + hit.collider.name);
    //            // 例：敵のHPを減らす処理など
    //            // hit.collider.GetComponent<Enemy>().TakeDamage(10);
    //        }
    //    }
    //    else
    //    {
    //        // 何にも当たらなかった場合は最大距離まで
    //        lineRenderer.SetPosition(1, transform.position + transform.forward * beamLength);
    //    }
    //}
}

