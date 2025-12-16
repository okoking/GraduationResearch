using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class Boss2 : MonoBehaviour
{

    enum BossMove
    {
        TRACKING,   //追跡
        JUMP,       //溜めジャンプ
    }

    public float speed;               //移動速度

    public float stopDistance;

    private bool isJump = false;

    private BossMove move = BossMove.TRACKING;

    private Transform target;

    private Rigidbody rb;

    public float chargeTime = 1f;        // 溜め時間
    public float jumpPower = 15f;         // ジャンプ力
    public float shockwaveRadius = 5f;    // 衝撃波範囲
    public int shockwaveDamage = 10;

    bool isCharging;
    bool isJumping;

    public float horizontalSpeed = 5f;

    Vector3 targetPos;   // 着地予定地点

    private PlayerHealth playerHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;
        else
            Debug.LogWarning("Playerタグを持つオブジェクトが見つかりませんでした");

        rb = GetComponent<Rigidbody>();

        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (move)
        {
            case BossMove.TRACKING:
                //もしプレイヤー範囲外にいれば追跡
                float distance = Vector3.Distance(transform.position, target.position);
                if (distance > stopDistance)
                {
                    Vector3 lookPos = new Vector3(target.position.x, transform.position.y, target.position.z);
                    transform.LookAt(lookPos);

                    Vector3 nextPos = Vector3.MoveTowards(transform.position, lookPos, speed * Time.deltaTime);
                    nextPos.y = transform.position.y;
                    transform.position = nextPos;
                }
                else//そうでなければジャンプ攻撃へ
                {
                    move = BossMove.JUMP;
                }

                break;
            case BossMove.JUMP:
                StartChargeJump();
                break;
        }
    }

    public void StartChargeJump()
    {
        if (!isCharging && !isJumping)
            StartCoroutine(ChargeJump());
    }

    IEnumerator ChargeJump()
    {
        isCharging = true;

        // 溜め演出（アニメ・エフェクト）
        // animator.SetTrigger("Charge");

        yield return new WaitForSeconds(chargeTime);

        isCharging = false;
        isJumping = true;

        // ★ プレイヤーの位置を保存（高さは無視）
        targetPos = target.position;
        targetPos.y = transform.position.y;

        // 大ジャンプ
        JumpToTarget();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isJumping) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            Landing();
            //追跡に移行
            move = BossMove.TRACKING;
        }
    }

    void JumpToTarget()
    {
        rb.linearVelocity = Vector3.zero;

        Vector3 start = transform.position;
        Vector3 end = targetPos;

        float gravity = Physics.gravity.y;
        float height = 6f;   // ジャンプの高さ

        Vector3 velocity = CalculateJumpVelocity(start, end, height, gravity);
        rb.linearVelocity = velocity;
    }

    Vector3 CalculateJumpVelocity(Vector3 start, Vector3 end, float height, float gravity)
    {
        float displacementY = end.y - start.y;
        Vector3 displacementXZ = new Vector3(
            end.x - start.x,
            0,
            end.z - start.z
        );

        float timeUp = Mathf.Sqrt(-2 * height / gravity);
        float timeDown = Mathf.Sqrt(2 * (displacementY - height) / gravity);
        float time = timeUp + timeDown;

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = displacementXZ / time;

        return velocityXZ + velocityY;
    }

    void Landing()
    {
        //// 着地エフェクト
        //Instantiate(shockwaveEffect, transform.position, Quaternion.identity);

        // 範囲内のプレイヤー取得
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            shockwaveRadius
        );

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                playerHealth.TakeDamage(1);
            }
        }
    }
}
