using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    private LockOnSystem lockOn;

    public float dashSpeed = 15f;   // 突進スピード
    public float punchRange = 1.5f; // パンチが届く距離

    bool isDashing = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lockOn = GetComponent<LockOnSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lockOn.transform == null) return;

        if (Input.GetButtonDown("Fire1") && !isDashing)
        {
            StartDash();
        }

        if (isDashing)
        {
            DashToTarget();
        }
    }

    void StartDash()
    {
        isDashing = true;

        // 敵の方向を向く（超重要）
        Vector3 dir = lockOn.transform.position - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void DashToTarget()
    {
        Vector3 targetPos = lockOn.transform.position;
        targetPos.y = transform.position.y; // 高さ固定

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            dashSpeed * Time.deltaTime
        );

        float distance = Vector3.Distance(transform.position, targetPos);

        if (distance <= punchRange)
        {
            DoPunch();
            isDashing = false;
        }
    }

    void DoPunch()
    {
        Debug.Log("パンチ！！");

        // ここで
        // ・攻撃アニメーション再生
        // ・当たり判定ON
        // ・ダメージ処理
    }
}
