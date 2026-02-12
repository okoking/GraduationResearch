using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BeamSweepController : MonoBehaviour
{
    public float duration = 2f;
    public float sweepAngle = 90f;
    public float beamLength = 50f;
    public float beamWidth = 0.2f;
    public LayerMask groundLayer;

    private LineRenderer line;
    private float timer;

    // ワールド座標でのターゲット地点
    private Vector3 targetLeft;
    private Vector3 targetRight;

    private PlayerHealth playerHealth;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.startWidth = beamWidth;
        line.endWidth = beamWidth;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerHealth = player.GetComponent<PlayerHealth>();

        Vector3 origin = transform.position;

        // --- ここから書き換え ---
        Vector3 groundReference;
        if (player != null)
        {
            // プレイヤーの今の足元の位置を基準にする
            groundReference = player.transform.position;
        }
        else
        {
            // プレイヤーがいない場合は正面の地面を基準にする
            groundReference = origin + transform.forward * 10f;
        }
        groundReference.y = 0; // 地面の高さに固定（ステージに合わせて調整）
                               // --- ここまで ---

        // 基準点（プレイヤー位置）への方向を出す
        Vector3 dirToTarget = (groundReference - origin).normalized;

        // その方向を左右に振って、扇形の端っこ（ターゲット地点）を決める
        Quaternion leftRot = Quaternion.AngleAxis(-sweepAngle / 2f, Vector3.up);
        Quaternion rightRot = Quaternion.AngleAxis(sweepAngle / 2f, Vector3.up);

        // 発射地点が動いても「ここを狙う」という座標を確定させる
        targetLeft = origin + (leftRot * dirToTarget) * beamLength;
        targetRight = origin + (rightRot * dirToTarget) * beamLength;

        // ターゲットの高さも地面（y=0）に揃えておく
        targetLeft.y = 0;
        targetRight.y = 0;

        Destroy(gameObject, duration);
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 0〜1の間で往復、または片道移動させる（Sinなどで往復、またはpingpong）
        // ここでは duration に合わせて1往復する例
        float progress = Mathf.PingPong(timer * 2f / duration, 1f);

        // ターゲット地点を線形補間で決定（地面上の「点」を狙い続ける）
        Vector3 currentTarget = Vector3.Lerp(targetLeft, targetRight, progress);

        // 発射地点からターゲットへの方向を計算
        Vector3 origin = transform.position;
        Vector3 dir = (currentTarget - origin).normalized;

        // 地面への衝突判定（必要に応じて）
        Vector3 hitPoint = origin + dir * beamLength;
        if (Physics.Raycast(origin, dir, out RaycastHit hit, beamLength, groundLayer))
        {
            hitPoint = hit.point;
        }

        // プレイヤー判定
        RaycastHit[] hits = Physics.SphereCastAll(origin, beamWidth, dir, beamLength);
        foreach (var h in hits)
        {
            if (h.collider.CompareTag("Player") && playerHealth != null)
            {
                playerHealth.TakeDamage(1);
                // EffectManager.instance.Play("BeamColl", h.point);
            }
        }

        // 見た目の更新
        line.SetPosition(0, origin);
        line.SetPosition(1, hitPoint);
    }
}