using UnityEngine;
using System.Collections;

public class KariBeam : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float beamLength = 50f;
    [SerializeField] private float beamWidth = 0.2f;
    [SerializeField] private float MinibeamDuration = 1.5f; // 何秒間出し続けるか
    [SerializeField] private Camera mainCam;

    private bool isFiring = false;
    private BeamCamera beamCamera;
    private LockOnSystem lockOn;
    private BeamGauge beamGauge;
    void Start()
    {
        lineRenderer.enabled = false;
        lineRenderer.startWidth = beamWidth;
        lineRenderer.endWidth = beamWidth;
        beamCamera = GetComponent<BeamCamera>();
        lockOn = GetComponent<LockOnSystem>();
        beamGauge = GetComponent<BeamGauge>();
    }

    void Update()
    {
        // ボタン押したらビーム発射
        if (Input.GetKeyDown(KeyCode.JoystickButton5) && !isFiring)
        {
            if (!beamCamera.isSootBeam)
            {
                StartCoroutine(FireLockOnBeam());
            }
        }
    }

    IEnumerator FireLockOnBeam()
    {
        if (lockOn != null && lockOn.lockOnTarget != null)
        {
            isFiring = true;
            lineRenderer.enabled = true;

            int cnt = 0;
            while (cnt < 10)
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
                        if(!h.collider.GetComponent<EnemyAI>().TakeDamage(1, h.point))
                        {
                            beamGauge.Charge();
                        }
                        cnt++;
                    }
                }

                lineRenderer.SetPosition(0, start);
                lineRenderer.SetPosition(1, end);

                yield return null;
            }

            lineRenderer.enabled = false;
            isFiring = false;
        }
    }

    public bool GetisFiring()
    {
        return isFiring;
    }
}

