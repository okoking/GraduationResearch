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

        // ===== スタン中 =====
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
                isStunned = false;
            return;
        }

        // ===== クールタイム中 =====
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
                isOnCooldown = false;
        }

        // ===== タックル中 =====
        if (isTackling)
        {
            tackleTimer -= Time.deltaTime;
            transform.position += transform.forward * tackleSpeed * Time.deltaTime;

            if (tackleTimer <= 0)
                EndTackle();

            return;
        }

        // ===== 通常の追跡 =====
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > stopDistance)
        {
            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            // クールタイムが終わっていたらタックル
            if (!isOnCooldown)
                StartTackle();
        }
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
                // プレイヤーにダメージ処理を入れる
            }
            else
            {
                Debug.Log("壁などに衝突 → スタン");
                StartStun();
            }

            EndTackle();
        }
    }

    void StartStun()
    {
        isStunned = true;
        stunTimer = stunTime;
        Debug.Log("スタン状態");
    }
}

//プレイヤーに近づく
//近づいたらタックル
//プレイヤーに当たればダメージ
//プレイヤーによけられて、壁や障害物にぶつかればスタンしてダメージが通るようになる
//※普段はダメージが通らない