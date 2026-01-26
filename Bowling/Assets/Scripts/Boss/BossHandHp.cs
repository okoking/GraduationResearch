using UnityEngine;

public class BossHandHp : MonoBehaviour
{

    public int hp = 25;
    public int maxHp = 25;

    //ñ≥ìGÇ©Ç«Ç§Ç©
    bool isPerfect = false;

    //ñ≥ìGéûä‘
    float isInvincibleTime;

    //ç≈ëÂñ≥ìGéûä‘
    public float maxInvincibleTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    BossHandManager handManager;

    void Start()
    {
        BossHandManager manager =
        FindAnyObjectByType<BossHandManager>();

        if (manager != null)
        {
            SetHandManager(manager);
        }
        else
        {
            Debug.LogError("BossHandManager Ç™å©Ç¬Ç©ÇËÇ‹ÇπÇÒ", this);
        }
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

        ////âº
        //if (Input.GetKey(KeyCode.H))
        //{
        //    TakeDamage(10);
        //}
    }

    public void TakeDamage(int take)
    {
        if (!isPerfect)
        {
            hp -= take;
            isPerfect = true;
        }

        if (hp <= 0)
        {
            Die();
        }
    }

    public void SetHandManager(BossHandManager manager)
    {
        handManager = manager;
    }


    void Die()
    {
        if (handManager != null)
        {
            handManager.OnHandDestroyed();
        }
        else
        {
            Debug.LogError("HandManager Ç™ê›íËÇ≥ÇÍÇƒÇ¢Ç‹ÇπÇÒ", this);
        }

        Destroy(gameObject);
    }
}
