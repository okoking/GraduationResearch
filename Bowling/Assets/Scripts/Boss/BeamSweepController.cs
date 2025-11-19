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

        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
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
                Debug.Log(h.collider.name);
                playerHealth.TakeDamage(1);
            }
        }

        // LineRenderer更新
        line.SetPosition(0, start);
        line.SetPosition(1, hitPoint);
    }
}