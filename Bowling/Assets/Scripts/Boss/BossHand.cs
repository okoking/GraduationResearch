using UnityEngine;
using System.Collections;

public class BossHand : MonoBehaviour
{
    Transform player;
    public Transform bossHandSpawn;

    public GameObject beamSweepPrefab;     // 実際のビーム
    public LineRenderer aimLinePrefab;     // 地面に出す予兆線

    public float orbitRadius = 5f;
    public float orbitSpeed = 30f;
    public float floatAmplitude = 1f;
    public float floatSpeed = 2f;

    public float beamInterval = 5f;
    public float beamWarningTime = 1.5f;   // 予兆線を表示しておく時間
    public float sweepAngle = 90f;         // 薙ぎ払い角度
    public float sweepSpeed = 60f;

    private float angle;
    private float beamTimer;
    private bool isFiringBeam = false;

    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        if (bossHandSpawn == null || player == null) return;

        if (!isFiringBeam)
        {
            angle += orbitSpeed * Time.deltaTime;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(
                Mathf.Cos(rad) * orbitRadius,
                Mathf.Sin(Time.time * floatSpeed) * floatAmplitude,
                Mathf.Sin(rad) * orbitRadius
            );
            transform.position = bossHandSpawn.position + offset;
            transform.LookAt(player);
        }

        beamTimer += Time.deltaTime;
        if (beamTimer >= beamInterval && !isFiringBeam)
        {
            beamTimer = 0f;
            StartCoroutine(ShootSweepBeam());
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
            Vector3 dir = rot * flatForward;

            // Rayを少し下向きにして、地面まで届かせる
            Vector3 rayOrigin = origin + Vector3.up * 5f;
            Vector3 rayDir = (dir + Vector3.down * 0.2f).normalized;

            if (Physics.Raycast(rayOrigin, rayDir, out RaycastHit hit, beamRange))
            {
                line.SetPosition(i, hit.point + Vector3.up * 0.02f);
            }
            else
            {
                // 地面まで届かなかったら、beamRange先を下にもう一度探す
                if (Physics.Raycast(rayOrigin + rayDir * beamRange, Vector3.down, out RaycastHit downHit, 100f))
                {
                    line.SetPosition(i, downHit.point + Vector3.up * 0.02f);
                }
                else
                {
                    line.SetPosition(i, origin + dir * beamRange);
                }
            }
        }

        // 見た目設定
        line.startWidth = 0.15f;
        line.endWidth = 0.15f;
        line.material.color = new Color(1, 0, 0, 0.8f);

        // 警告時間待ち
        yield return new WaitForSeconds(displayTime);

        // 実際のビーム発射
        GameObject beam = Instantiate(beamSweepPrefab, transform.position, transform.rotation);
        float beamDuration = beam.GetComponent<BeamSweepController>().duration;

        yield return new WaitForSeconds(beamDuration);

        Destroy(line.gameObject);
        isFiringBeam = false;
    }

    void DrawWarningLine(LineRenderer line, Vector3 dir)
    {
        // LineRendererの基本設定
        line.positionCount = 20;
        line.useWorldSpace = true;
        line.enabled = true;

        // 地面にRayを飛ばして「薙ぎ払い軌道」を描く
        float startAngle = -sweepAngle / 2;
        float endAngle = sweepAngle / 2;

        for (int i = 0; i < line.positionCount; i++)
        {
            float t = (float)i / (line.positionCount - 1);
            float currentAngle = Mathf.Lerp(startAngle, endAngle, t);
            Quaternion rot = Quaternion.AngleAxis(currentAngle, Vector3.up);
            Vector3 sweepDir = rot * dir;

            // 地面ヒット位置をRaycastで求める
            if (Physics.Raycast(transform.position, sweepDir, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
            {
                line.SetPosition(i, hit.point + Vector3.up * 0.05f);
            }
            else
            {
                // ヒットしなかったら少し前方
                line.SetPosition(i, transform.position + sweepDir * 10f);
            }
        }
    }

    IEnumerator SweepBeam(GameObject beam, Vector3 dir)
    {
        float currentAngle = -sweepAngle / 2;

        while (currentAngle <= sweepAngle / 2)
        {
            Quaternion rotation = Quaternion.AngleAxis(currentAngle, Vector3.up);
            Vector3 sweepDir = rotation * dir;

            transform.rotation = Quaternion.LookRotation(sweepDir);
            beam.transform.position = transform.position;
            beam.transform.rotation = transform.rotation;

            currentAngle += sweepSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
