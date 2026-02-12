using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BeamSweepController : MonoBehaviour
{
    public float duration = 2f;
    public float sweepAngle = 90f;
    public float sweepSpeed = 90f;
    public float beamLength = 50f;
    public float beamWidth = 0.2f;
    public LayerMask groundLayer;
    private LineRenderer line;
    private float timer;
    private float currentAngle;
    private bool sweepingRight = true;

    GameObject player;

    private Vector3 groundOrigin;
    private float baseYaw;

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

        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }

        // 手の位置を地面に固定
        groundOrigin = new Vector3(transform.position.x, 0.01f, transform.position.z);

        // 水平成分だけ取得
        Vector3 flatForward =
            Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;

        // Yaw角を保存
        baseYaw = Mathf.Atan2(flatForward.x, flatForward.z) * Mathf.Rad2Deg;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float step = sweepSpeed * Time.deltaTime * (sweepingRight ? 1 : -1);
        currentAngle += step;

        if (Mathf.Abs(currentAngle) >= sweepAngle / 2f)
            sweepingRight = !sweepingRight;

        // ★ transform.forward は使わない！
        float yaw = baseYaw + currentAngle;

        // 常にワールド基準
        Vector3 dir = Quaternion.Euler(0f, yaw, 0f) * Vector3.forward;

        // 常に地面から出す
        Vector3 start = groundOrigin;

        Vector3 hitPoint = start + dir * beamLength;

        if (Physics.Raycast(start, dir, out RaycastHit hit, beamLength, groundLayer))
        {
            hitPoint = hit.point;
        }

        // プレイヤー判定
        RaycastHit[] hits = Physics.SphereCastAll(start, beamWidth, dir, beamLength);
        foreach (var h in hits)
        {
            if (h.collider.CompareTag("Player"))
            {
                playerHealth.TakeDamage(1);
                EffectManager.instance.Play("BeamColl", h.transform.position);
            }
        }

        // LineRenderer
        // 見た目の始点は「手」
        line.SetPosition(0, transform.position);

        // 見た目の終点は「地面上の薙ぎ払い結果」
        line.SetPosition(1, hitPoint);
    }
}