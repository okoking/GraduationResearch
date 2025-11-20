using UnityEngine;

public class BossHandHp : MonoBehaviour
{

    public int hp = 50;

    private Boss boss;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        boss = FindAnyObjectByType<Boss>();
    }

    // Update is called once per frame
    void Update()
    {
        //仮
        if (Input.GetKey(KeyCode.H))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int take)
    {
        hp -= take;

        Debug.Log("ボスハンド" + hp);

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(this);
        //ボスの完全無敵状態を解除する
        boss.FalseIsPerfectInvincible();
    }
}
