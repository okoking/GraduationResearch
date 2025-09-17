using UnityEngine;
using static UnityEditorInternal.ReorderableList;

public class EnemyBase : MonoBehaviour
{
    public float knockbackPower = 10f;  // ぶっ飛ばす強さ
    public float upPower = 4.5f;          // 上方向に少し浮かす量

    private HitPointManager enemyHp;

    private Rigidbody enemyRd;

    private Vector3 defaultPos;

    public float minY = -5f;          // y座標がこれ以下ならリセット
    public float stopThreshold = 0.1f; // 速度がこの以下なら停止とみなす
    public float checkDelay = 1f;      // 地面に落ちてから判定を始める遅延（秒）

    private float groundedTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyHp = GetComponent<HitPointManager>();

        //このオブジェクトのリジッドボディを取得
        enemyRd = this.GetComponent<Rigidbody>();

        defaultPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < minY)
        {
            ResetEnemy();
        }

        // スピードと回転が止まったら
        if (enemyRd.linearVelocity.magnitude < stopThreshold && enemyRd.angularVelocity.magnitude < stopThreshold)
        {
            groundedTime += Time.deltaTime;
            if (groundedTime >= checkDelay)
            {
                ResetEnemy();
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
            enemyHp.TakeDamage((int)enemyRd.linearVelocity.magnitude);

            // 弾の進行方向を使って吹っ飛ばす
            Vector3 forceDir = (transform.position - collision.transform.position).normalized;
            forceDir += Vector3.up * upPower; // 上方向にも少し力を加える

            enemyRd.AddForce(forceDir.normalized * knockbackPower, ForceMode.Impulse);
        }

        //ミサイルと当たったらミサイルを消す,ダメージを受ける
        if (collision.gameObject.CompareTag("Missile"))
        {
            Destroy(collision.gameObject);

            enemyHp.TakeDamage(1);
        }
    }

    public HitPointManager GetEnemyHp() { return enemyHp; }

    private void ResetEnemy()
    {
        enemyRd.linearVelocity = Vector3.zero;
        enemyRd.angularVelocity = Vector3.zero;
        transform.position = defaultPos;
        transform.rotation = Quaternion.identity;
        groundedTime = 0f;
    }
}