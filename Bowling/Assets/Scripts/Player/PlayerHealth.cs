using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP設定")]
    public int maxHealth = 1000;   // 最大HP
    public int currentHealth;     // 現在HP

    void Start()
    {
        currentHealth = maxHealth;  // ゲーム開始時に全回復
    }

    // ダメージを受ける
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 0～maxに制限

        Debug.Log("現在HP：" + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 回復する
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("回復！ 現在HP：" + currentHealth);
    }

    // 死亡処理
    void Die()
    {
        Debug.Log("プレイヤー死亡！");
        // 例）アニメ再生、リスポーン、ゲームオーバーUI表示など

        transform.position = Vector3.zero;
        currentHealth = maxHealth;
    }
}
