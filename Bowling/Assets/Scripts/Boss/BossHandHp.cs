using UnityEngine;

public class BossHandHp : MonoBehaviour
{

    private BossHp bosshp;

    public int hp = 50;

    private Boss boss;

    //ñ≥ìGÇ©Ç«Ç§Ç©
    bool isPerfect = false;

    //ñ≥ìGéûä‘
    float isInvincibleTime;

    //ç≈ëÂñ≥ìGéûä‘
    public float maxInvincibleTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject bossObj = GameObject.FindGameObjectWithTag("Boss");
        if (bossObj != null)
            bosshp = bossObj.GetComponent<BossHp>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPerfect)
        {
            isInvincibleTime += Time.deltaTime;
        }

        if (isInvincibleTime > maxInvincibleTime)
        {
            isInvincibleTime = 0f;
            isPerfect = false;
        }

        //âº
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
        //É{ÉXÇÃäÆëSñ≥ìGèÛë‘ÇâèúÇ∑ÇÈ
        bosshp.SetIsPerfectInvincible(false);
    }
}
