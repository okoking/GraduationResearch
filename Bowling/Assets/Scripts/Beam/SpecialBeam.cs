using UnityEngine;
using UnityEngine.VFX;

public class SpecialBeam : MonoBehaviour
{
    // ===== References =====
    private BeamCamera beamCamera;

    [Header("VFX")]
    [SerializeField] VisualEffect vfxPrefab;
    private VisualEffect currentVFX;

    [Header("Beam Hit")]
    [SerializeField] float beamRange = 100f;

    [Header("PlayerMovement")]
    public bool disableRotate;

    [Header("Camera")]
    [SerializeField] Camera mainCam;

    private BeamGauge beamGauge;

    private PlayerAnimation plAnim;

    // ===== State =========
    private bool isActive = false;
    // =====================

    void Start()
    {
        beamCamera = GetComponent<BeamCamera>();
        beamGauge = GetComponent<BeamGauge>();
        plAnim = GetComponent<PlayerAnimation>();
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
        // Debug.Log(GetCenterToPlayerDir());

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
        currentVFX = Instantiate(
            vfxPrefab,
            transform.position + Vector3.up * 1.0f,
            Quaternion.LookRotation(GetCenterToPlayerDir())
        );

        currentVFX.transform.SetParent(transform, true);
        currentVFX.SendEvent("OnPlay");

        isActive = true;
    }

    void UpdateBeamDirection()
    {
        if (currentVFX == null) return;

        Vector3 dir = GetCenterToPlayerDir();

        // 体を向ける
        // ★ 撃っている方向にプレイヤーを向ける（水平だけ）
        Vector3 lookDir = dir;
        lookDir.y = 0;
        Quaternion targetRot = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            10.0f * Time.deltaTime
        );

        currentVFX.transform.position = transform.position + Vector3.up * 1.0f;
        currentVFX.transform.rotation = Quaternion.LookRotation(dir);
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
        //Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 中央(0.5,0.5)
        Vector3 dir = GetCenterToPlayerDir();
        if (Physics.SphereCast(
            transform.position,  // プレイヤー位置
            1f,                  // 半径
            dir,                 // ← さっきの座標方向
            out RaycastHit hit,
            30f                  // 距離
        ))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Hit");
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

    //Ray GetCenterRay()
    //{
    //    Vector3 screenCenter = new Vector3(
    //        Screen.width * 4f / 7f,
    //        Screen.height / 2f,
    //        0f
    //    );

    //    return mainCam.ScreenPointToRay(screenCenter);
    //}

    Vector3 GetCenterRayPoint()
    {
        // 画面中央
        Vector3 center = new Vector3(
            Screen.width * 0.5f,
            Screen.height * 0.5f,
            0f
        );

        Ray ray = mainCam.ScreenPointToRay(center);

        RaycastHit[] hits = Physics.RaycastAll(ray, beamRange);

        float nearestDist = float.MaxValue;
        Vector3 result = ray.origin + ray.direction * beamRange;

        foreach (var h in hits)
        {
            if (h.collider.CompareTag("Untagged"))
                continue;

            if (h.distance < nearestDist)
            {
                nearestDist = h.distance;
                result = h.point;
            }
        }

        return result;
    }

    Vector3 GetCenterToPlayerDir()
    {
        // プレイヤー → ターゲットへの方向
        return GetCenterRayPoint() - transform.position;
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