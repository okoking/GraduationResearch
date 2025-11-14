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

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.startWidth = beamWidth;
        line.endWidth = beamWidth;
        line.useWorldSpace = true;

        currentAngle = -sweepAngle / 2f;

        Destroy(gameObject, duration);
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

        //if (Physics.SphereCast(start, beamWidth, transform.forward, out RaycastHit a, beamLength))
        //{
        //    //end = hit.point;
        //    // 当たった敵に処理
        //    Debug.Log("プレイヤーにヒット！: " + a.collider.name);
        //}

        RaycastHit[] hits = Physics.SphereCastAll(start, beamWidth, dir, beamLength);
        foreach (var h in hits)
        {
            if (h.collider.CompareTag("Player"))
            {
                Debug.Log(h.collider.name);
                //あとはプレイヤーにダメージ与える

            }
        }

        // LineRenderer更新
        line.SetPosition(0, start);
        line.SetPosition(1, hitPoint);
    }
}