using UnityEngine;
using System.Collections;
using UnityEngine.VFX;
using System.Globalization;
using System.Runtime.InteropServices.WindowsRuntime;

public class KariBeam : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float beamLength = 50f;
    [SerializeField] private float beamWidth = 0.2f;
    [SerializeField] private float MinibeamDuration = 1.5f; // 何秒間出し続けるか
    [SerializeField] private Camera mainCam;
    
    [Header("VFX")]
    [SerializeField] VisualEffect vfxPrefab;
    private VisualEffect currentVFX;

    public bool isFiring = false;

    private BeamCamera beamCamera;
    private LockOnSystem lockOn;
    private BeamGauge beamGauge;
    private PlayerAnimation plAnim;

    private bool TriggershotBeam;
    public bool isShotAnimationing;

    [ContextMenu("フラグ確認")]
    public void Checkflag()
    {
        Debug.Log("isFiring:"+ isFiring.ToString()+"\n"+
            "TriggershotBeam:" + TriggershotBeam.ToString() + "\n" +
            "isShotAnimationing:" + isShotAnimationing.ToString());
    }
    
    void Start()
    {
        lineRenderer.enabled = false;
        lineRenderer.startWidth = beamWidth;
        lineRenderer.endWidth = beamWidth;
        beamCamera = GetComponent<BeamCamera>();
        lockOn = GetComponent<LockOnSystem>();
        beamGauge = GetComponent<BeamGauge>();
        plAnim = GetComponent<PlayerAnimation>();
        TriggershotBeam = false;
        isShotAnimationing = false;
    }

    void Update()
    {
        // ボタン押したらビーム発射
        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            if (!beamCamera.isSootBeam)
            {
                ShootStart();
            }

            //if (!beamCamera.isSootBeam)
            //{
            //    StartCoroutine(FireLockOnBeam());
            //}
        }

        StartBeam();

        UpdateBeam();

        UpdateRotate();
    }

    Vector3 GetDirToTarget()
    {
        if (lockOn.lockOnTarget == null) return transform.forward;
        return (lockOn.lockOnTarget.position - transform.position).normalized;
    }

    void StartBeam()
    {
        if (!isShotAnimationing || lockOn.lockOnTarget == null || !TriggershotBeam || !isFiring) return;

        currentVFX = Instantiate(
                vfxPrefab,
                transform.position + Vector3.up * 1.0f,
                Quaternion.LookRotation(GetDirToTarget())
        );

        currentVFX.transform.SetParent(transform, true);
        currentVFX.SendEvent("OnPlay");

        TriggershotBeam = false;
    }

    void UpdateRotate()
    {
        //if (!isShotAnimationing) return;
        ////if (lockOn.lockOnTarget == null) return;

        //Vector3 startPos = transform.position + Vector3.up * 1.0f;
        //Vector3 targetPos = lockOn.lockOnTarget.position;

        //Vector3 dir = targetPos - startPos;

        //// 向き
        //transform.rotation = Quaternion.Slerp(
        //    transform.rotation,
        //    Quaternion.LookRotation(dir),
        //    5f * Time.deltaTime
        //);

        if (!isShotAnimationing) return;
        if (lockOn == null) return;
        if (lockOn.lockOnTarget == null) return;

        Vector3 startPos = transform.position + Vector3.up * 1.0f;

        //Destroy 済み対策
        Transform targetTf = lockOn.lockOnTarget;
        if (!targetTf) return;

        Vector3 dir = targetTf.position - startPos;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            5f * Time.deltaTime
        );
    }


    void UpdateBeam()
    {
        if (!isShotAnimationing || lockOn.lockOnTarget == null || !isFiring) return;

        Vector3 startPos = transform.position + Vector3.up * 1.0f;
        Vector3 targetPos = lockOn.lockOnTarget.position;

        Vector3 dir = targetPos - startPos;
        float length = dir.magnitude;

        // 位置
        currentVFX.transform.position = startPos;

        // 向き
        currentVFX.transform.rotation = Quaternion.LookRotation(dir);

        // VFXに距離を渡す
        currentVFX.transform.localScale = new Vector3(1, 1, length * .1f);

        //var vfx = currentVFX.GetComponent<VisualEffect>();
        //vfx.SetFloat("BeamLength", length);


        // =====================
        // 当たり判定（Raycast）
        // =====================
        Ray ray = new Ray(startPos, dir.normalized);

        if (Physics.Raycast(ray, out RaycastHit hit, length))
        {
            // デバッグ表示
            Debug.DrawRay(startPos, dir.normalized * hit.distance, Color.red);

            // ダメージ処理
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<EnemyAI>()?.TakeDamage(999, hit.point);
                beamGauge.Charge();
            }

            // ヒット地点までビームを止めたい場合
            length = hit.distance;
        }
    }

    void ShootStart()
    {
        if (isFiring || lockOn.lockOnTarget == null) return;

        plAnim.ChangedAnim(5);

        isShotAnimationing = true;
    }

    public void ShotweekBeam()
    {
        TriggershotBeam = true;
        isFiring = true;

        Debug.Log("打ち始め");
    }

    public void EndweekBeam()
    {
        isShotAnimationing = false;
        isFiring = false;
        Destroy(currentVFX);
        currentVFX = null; 
        Debug.Log("打ち終わり");
    }

    public bool GetisFiring()
    {
        return isFiring;
    }


    //void BeamRaycast()
    //{
    //    //Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 中央(0.5,0.5)
    //    Vector3 start = transform.position;
    //    start.y += 1;
    //    Vector3 end = lockOn.lockOnTarget.position;

    //    Vector3 direction = (end - start).normalized;

    //    float Distance = Vector3.Distance(end, start);

    //    RaycastHit[] hits = Physics.SphereCastAll(start, beamWidth, direction, Distance);

    //    foreach (var h in hits)
    //    {
    //        if (h.collider.CompareTag("Enemy"))
    //        {
    //            if (!h.collider.GetComponent<EnemyAI>().TakeDamage(1, h.point))
    //            {
    //                beamGauge.Charge();
    //            }
    //        }
    //    }
    //}

    IEnumerator FireLockOnBeam()
    {
        if (lockOn != null && lockOn.lockOnTarget != null)
        {
            plAnim.ChangedAnim(5);

            isFiring = true;
            lineRenderer.enabled = true;

            bool isFin = true;
            while (isFin)
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
                    {
                        if (!h.collider.GetComponent<EnemyAI>().TakeDamage(1, h.point))
                        {
                            beamGauge.Charge();
                            isFin = false;
                        }
                    }
                }

                lineRenderer.SetPosition(0, start);
                lineRenderer.SetPosition(1, end);

                yield return null;
            }

            lineRenderer.enabled = false;
            isFiring = false;
            TriggershotBeam = false;
        }
    }

}

