using UnityEngine;

public class TackleBoss : MonoBehaviour
{
    public float speed = 3f;               // 通常の移動速度
    public float tackleSpeed = 10f;        // タックル時の速度
    public float stopDistance = 10f;      // 近づく距離
    public float tackleDuration = 1.0f;    // タックルの持続時間
    public float stunTime = 2.0f;          // スタン時間
    public float tackleCooldown = 3.0f;    // タックルの再使用までの時間

    private Transform target;
    private bool isTackling = false;
    private bool isStunned = false;
    private bool isOnCooldown = false;
    private float tackleTimer = 0f;
    private float stunTimer = 0f;
    private float cooldownTimer = 0f;

    //体力
    public int hitPoint = 1;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Playerタグを持つオブジェクトが見つかりませんでした");
        }
    }

    void Update()
    {
        if (target == null) return;

        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
                isStunned = false;
            return;
        }

        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
                isOnCooldown = false;
        }

        //===== タックル中 =====
        if (isTackling)
        {
            tackleTimer -= Time.deltaTime;

            // forward方向に進むけど、Y軸は固定
            Vector3 moveDir = transform.forward;
            moveDir.y = 0; // Y方向の動きを消す
            transform.position += moveDir.normalized * tackleSpeed * Time.deltaTime;

            if (tackleTimer <= 0)
                EndTackle();

            return;
        }

        // ===== 通常追跡 =====
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > stopDistance)
        {
            // プレイヤーのYを無視して水平に向く
            Vector3 lookPos = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.LookAt(lookPos);

            // 移動先のYを固定
            Vector3 nextPos = Vector3.MoveTowards(transform.position, lookPos, speed * Time.deltaTime);
            nextPos.y = transform.position.y;
            transform.position = nextPos;
        }
        else
        {
            if (!isOnCooldown)
                StartTackle();
        }

        //HP0以下になったら
        Death();
    }

    void StartTackle()
    {
        isTackling = true;
        tackleTimer = tackleDuration;
        isOnCooldown = true;
        cooldownTimer = tackleCooldown;
        transform.LookAt(target);
        Debug.Log("タックル開始！");
    }

    void EndTackle()
    {
        isTackling = false;
        Debug.Log("タックル終了");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTackling)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("プレイヤーに命中！");
                //プレイヤーにダメージ
            }
            if (collision.gameObject.CompareTag("Wall"))
            {
                Debug.Log("壁などに衝突 → スタン");
                StartStun();
            }

            EndTackle();
        }

        //プレイヤーの攻撃との当たり判定//★//
        if (collision.gameObject.CompareTag("Player"))
        {
            TakeDamage();
        }
    }

    void StartStun()
    {
        isStunned = true;
        stunTimer = stunTime;
        Debug.Log("スタン状態");
    }

    //ダメージを受ける
    void TakeDamage()
    {
        //スタン中であればダメージが通る
        if (isStunned)
        {
            hitPoint--;
        }
    }

    void Death()
    {
        if(hitPoint <= 0)
        {
            Destroy(gameObject);
        }
    }
}

//プレイヤーに近づく
//近づいたらタックル
//プレイヤーに当たればダメージ
//プレイヤーによけられて、壁や障害物にぶつかればスタンしてダメージが通るようになる
//※普段はダメージが通らない