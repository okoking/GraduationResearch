using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP設定")]
    public int maxHealth = 1000;   // 最大HP
    public int currentHealth;     // 現在HP

    [Header("デバッグ用")]
    [SerializeField] private int Damage = 10;

    private bool isInvincible = false;
    public float invincibleTime = 1f; // 無敵時間（秒）
    Renderer[] renderers;

    void Start()
    {
        currentHealth = maxHealth;  // ゲーム開始時に全回復
        renderers = GetComponentsInChildren<Renderer>();
    }

    private IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;

        float timer = 0f;
        Renderer renderer = GetComponentInChildren<Renderer>();

        while (timer < invincibleTime)
        {
            foreach (Renderer r in renderers)
            {
                r.enabled = !r.enabled;
            }
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        renderer.enabled = true;
        isInvincible = false;
    }

    // ダメージを受ける
    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // 無敵中ならダメージ受けない

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 0～maxに制限

        if (currentHealth <= 0)
        {
            Die();
        }
        //SoundManager.Instance.Request("PlayerDamage");
        StartCoroutine(InvincibleCoroutine());
    }

    [ContextMenu("ダメージ与える")]
    void DebugTakeDamage()
    {
        currentHealth -= Damage;
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

    public int GetHealth()
    {
        return currentHealth;
    }

    public float GetRatio()
    {
        return currentHealth / (float)maxHealth;
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
