using UnityEngine;

public class BossHandManager : MonoBehaviour
{
    [SerializeField] int handCount = 2;

    BossHp bossHp;

    void Awake()
    {
        bossHp = GetComponent<BossHp>();
    }

    public void OnHandDestroyed()
    {
        handCount--;

        if (handCount <= 0)
        {
            bossHp.SetIsPerfectInvincible(false);
        }
    }
}