using UnityEngine;

public class PinKnockBack : MonoBehaviour
{
    public float knockbackPower = 10f;  //ぶっ飛ばす強さ
    public float upPower = 4.5f;          //上方向に少し浮かす量

    private Rigidbody pinRd;

    private EnemyBase enemybase;

    private Vector3 defaultPos;
    private Quaternion defaultRot;

    public float minY = -5f;          //y座標がこれ以下ならリセット
    public float stopThreshold = 0.1f; //速度がこの以下なら停止とみなす
    public float checkDelay = 1f;      //地面に落ちてから判定を始める遅延（秒）

    private float groundedTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //このオブジェクトのリジッドボディを取得
        pinRd = this.GetComponent<Rigidbody>();

        enemybase = GetComponent<EnemyBase>();

        defaultPos = transform.position;
        defaultRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < minY)
        {
            ResetPin();
        }

        // スピードと回転が止まったら
        if (pinRd.linearVelocity.magnitude < stopThreshold && pinRd.angularVelocity.magnitude < stopThreshold)
        {
            groundedTime += Time.deltaTime;
            if (groundedTime >= checkDelay)
            {
                ResetPin();
            }
        }
        else
        {
            groundedTime = 0f; // 動いてたらリセット
        }
    }

    //当たり判定
    private void OnCollisionEnter(Collision collision)
    {
        //ボールとの当たり判定
        if (collision.gameObject.CompareTag("Ball"))
        {
            //弾の進行方向を使って吹っ飛ばす
            Vector3 forceDir = (transform.position - collision.transform.position).normalized;
            forceDir += Vector3.up * upPower; //上方向にも少し力を加える

            pinRd.AddForce(forceDir.normalized * knockbackPower, ForceMode.Impulse);

            FindFirstObjectByType<EnemyBase>().GetEnemyHp().TakeDamage((int)pinRd.linearVelocity.magnitude);

            // ミッションに通知
            FindFirstObjectByType<MissionManager>().HitEnemy();
        }

        //ミサイルと当たったらミサイルを消す,ダメージを受ける
        if (collision.gameObject.CompareTag("Missile"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void ResetPin()
    {
        pinRd.linearVelocity = Vector3.zero;
        pinRd.angularVelocity = Vector3.zero;
        transform.position = defaultPos;
        transform.rotation = defaultRot;
        groundedTime = 0f;
    }
}
