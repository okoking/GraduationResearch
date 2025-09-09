using UnityEngine;

public class EnemyBase : MonoBehaviour
{

    private HitPointManager enemyHp;

    private Rigidbody enemyRd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyHp = GetComponent<HitPointManager>();

        //このオブジェクトのリジッドボディを取得
        enemyRd = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Enemy HP: " + enemyHp.GetCurrentHp());
        }
        Debug.Log("Enemy Spd: " + enemyRd);
    }

    //当たり判定
    private void OnCollisionEnter(Collision collision)
    {
        //ボールとの当たり判定
        if (collision.gameObject.CompareTag("Ball"))
        {
            enemyHp.TakeDamage((int)enemyRd.linearVelocity.magnitude);
        }
    }

    public HitPointManager GetEnemyHp() { return enemyHp; }
}