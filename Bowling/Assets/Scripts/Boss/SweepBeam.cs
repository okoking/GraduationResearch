using UnityEngine;

public class BeamSweepController : MonoBehaviour
{
    public float duration = 2f;        // ビームが出ている時間
    public float sweepAngle = 90f;     // 振り幅（左右に何度回転するか）
    public float sweepSpeed = 90f;     // 回転速度（度/秒）
    public float beamLength = 10f;     // ビームの長さ
    public float beamWidth = 0.3f;     // ビームの太さ

    private LineRenderer line;
    private float timer;
    private float currentAngle;
    private bool sweepingRight = true;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.startWidth = beamWidth;
        line.endWidth = beamWidth;
        line.positionCount = 2;
        line.useWorldSpace = true;

        //初期向き設定
        currentAngle = -sweepAngle / 2f;

        //一定時間後に消す
        Destroy(gameObject, duration);
    }

    void Update()
    {
        timer += Time.deltaTime;
        //ビームが右か左かによって進む角度を変える
        float step = sweepSpeed * Time.deltaTime * (sweepingRight ? 1 : -1);
        currentAngle += step;

        // 振りきったら反対方向へ
        if (Mathf.Abs(currentAngle) >= sweepAngle / 2f)
            sweepingRight = !sweepingRight;

        // 向き更新
        Quaternion rot = Quaternion.Euler(0, currentAngle, 0);
        Vector3 dir = rot * transform.forward;

        // LineRendererで線を描く
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + dir * beamLength);
    }
}