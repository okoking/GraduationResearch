using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SpecialBeam : MonoBehaviour
{
    // ===== References =====
    private BeamCamera beamCamera;

    [Header("VFX")]
    [SerializeField] VisualEffect vfxPrefab;
    private VisualEffect currentVFX;

    [Header("Beam Hit")]
    [SerializeField] float beamRange = 100f;
    [SerializeField] LayerMask hitMask;
    [SerializeField] float damagePerSec = 10f;

    [Header("Beam Gauge")]
    [SerializeField] float maxGauge = 100f;
    [SerializeField] float gaugeDrainPerSec = 20f;

    [Header("PlayerMovement")]
    public bool disableRotate;

    [Header("Camera")]
    [SerializeField] Camera mainCam;

    [Header("LockOnBeam")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float MinibeamDuration = 1.5f; // 何秒間出し続けるか
    private LockOnSystem lockOn;


    private BeamGauge beamGauge;

    // ===== State =====
    private bool isActive = false;

    // =====================

    void Start()
    {
        beamCamera = GetComponent<BeamCamera>();
        beamGauge = GetComponent<BeamGauge>();
        lockOn= GetComponent<LockOnSystem>();
    }

    void Update()
    {
        bool beamInput =
            Input.GetKey(KeyCode.Q) ||
            Input.GetKey(KeyCode.JoystickButton5);

        //if (beamInput && !beamCamera.isSootBeam)
        //{
        //    StartCoroutine(FireLockOnBeam());
        //}
        // --- 開始 ---
        if (beamInput && beamCamera.isSootBeam && !isActive && beamGauge.IsLowestValueShotBeam())
        {
            beamGauge.SetUsingBeam(true);
            StartBeam();
        }

        //// --- 撃っている間 ---
        //if (isActive)
        //{
        //    if (beamGauge.TryConsume())
        //    {
        //        UpdateBeamDirection();
        //        BeamRaycast();
        //    }
        //    else
        //    {
        //        isActive = false;
        //    }
        //}

        //// --- 停止 ---
        //if (!beamInput && isActive)
        //{
        //    StopBeam();
        //    beamGauge.SetUsingBeam(false);
        //}

        disableRotate = false;

        if (isActive)
        {
            if (beamGauge.TryConsume())
            {
                if (beamInput)
                {
                    UpdateBeamDirection();
                    BeamRaycast();
                }
                else
                {
                    disableRotate = true;
                    StopBeam();
                    beamGauge.SetUsingBeam(false);
                }
            }
            else
            {
                disableRotate = true;
                StopBeam();
                beamGauge.SetUsingBeam(false);
            }
        }

    }

    // =====================
    // Beam Control
    // =====================

    void StartBeam()
    {
        Ray ray = GetCenterRay();

        currentVFX = Instantiate(
            vfxPrefab,
            transform.position + Vector3.up * 1.0f,
            Quaternion.LookRotation(ray.direction)
        );

        currentVFX.transform.SetParent(transform, true);
        currentVFX.SendEvent("OnPlay");

        isActive = true;
    }

    void UpdateBeamDirection()
    {
        if (currentVFX == null) return;

        Ray ray = GetCenterRay();

        // 体を向ける
        // ★ 撃っている方向にプレイヤーを向ける（水平だけ）
        Vector3 lookDir = ray.direction;
        lookDir.y = 0;
        Quaternion targetRot = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            10.0f * Time.deltaTime
        );

        currentVFX.transform.position = transform.position + Vector3.up * 1.0f;
        currentVFX.transform.rotation = Quaternion.LookRotation(ray.direction);
    }

    void StopBeam()
    {
        if (currentVFX != null)
        {
            currentVFX.SendEvent("OnStop");
            Destroy(currentVFX.gameObject);
        }

        currentVFX = null;
        isActive = false;
    }

    // =====================
    // Ray Hit
    // =====================

    void BeamRaycast()
    {
        Ray ray = GetCenterRay();
        //Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 中央(0.5,0.5)

        if (Physics.SphereCast(transform.position, .2f, transform.forward, out RaycastHit hit, 50f))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<EnemyAI>()?.TakeDamage(999, hit.point);
            }
            else if (hit.collider.CompareTag("Boss"))
            {
                hit.collider.GetComponent<BossHp>()?.TakeDamage(1);
            }
            else if (hit.collider.CompareTag("Bosshand"))
            {
                hit.collider.GetComponent<BossHandHp>()?.TakeDamage(1);
            }
        }
    }
    // =====================
    // Utility
    // =====================

    Ray GetCenterRay()
    {
        Vector3 screenCenter = new Vector3(
            Screen.width / 2f,
            Screen.height / 2f,
            0f
        );

        return mainCam.ScreenPointToRay(screenCenter);
    }

    //IEnumerator FireLockOnBeam()
    //{
    //    if (lockOn != null && lockOn.lockOnTarget != null)
    //    {
    //        isFiring = true;
    //        lineRenderer.enabled = true;

    //        float timer = 0f;
    //        while (timer < MinibeamDuration)
    //        {
    //            if (lockOn.lockOnTarget == null)
    //            {
    //                isFiring = false;                 // ビーム発射フラグをOFF
    //                lineRenderer.enabled = false;     // LineRendererを非表示
    //                StopAllCoroutines();              // 発射中のIEnumeratorを止める
    //                yield break;
    //            }

    //            Vector3 start = transform.position;
    //            start.y += 1;
    //            Vector3 end = lockOn.lockOnTarget.position;

    //            Vector3 direction = (end - start).normalized;

    //            float Distance = Vector3.Distance(end, start);

    //            RaycastHit[] hits = Physics.SphereCastAll(start, beamWidth, direction, Distance);
    //            foreach (var h in hits)
    //            {
    //                if (h.collider.CompareTag("Enemy"))
    //                {
    //                    h.collider.GetComponent<EnemyAI>()?.TakeDamage(1, h.point);
    //                }
    //            }

    //            lineRenderer.SetPosition(0, start);
    //            lineRenderer.SetPosition(1, end);

    //            timer += Time.deltaTime;
    //            yield return null;
    //        }

    //        lineRenderer.enabled = false;
    //        isFiring = false;
    //    }
    //}
}