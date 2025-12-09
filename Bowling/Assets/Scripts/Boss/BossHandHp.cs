using UnityEngine;

public class BossHandHp : MonoBehaviour
{

    public int hp = 50;

    private Boss boss;

    //–³“G‚©‚Ç‚¤‚©
    bool isPerfect = false;

    //–³“GŽžŠÔ
    float isInvincibleTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        boss = FindAnyObjectByType<Boss>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPerfect)
        {
            isInvincibleTime += Time.deltaTime;
        }

        if (isInvincibleTime > 0.5f)
        {
            isInvincibleTime = 0f;
            isPerfect = false;
        }

        //‰¼
        if (Input.GetKey(KeyCode.H))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int take)
    {
        if (!isPerfect)
        {
            hp -= take;
            Debug.Log(hp);
            isPerfect = true;
        }

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        //ƒ{ƒX‚ÌŠ®‘S–³“Gó‘Ô‚ð‰ðœ‚·‚é
        boss.FalseIsPerfectInvincible();
    }
}
