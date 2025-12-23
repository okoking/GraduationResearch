using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    [SerializeField] private Slider HpSlinder;
    [SerializeField] private TextMeshProUGUI textHP;

    private BossHp boss;

    public void SetBoss(BossHp bossHp)
    {
        boss = bossHp;
        UpdateHp();
    }

    void Update()
    {
        if (boss == null) return;

        UpdateHp();

        if (boss.GetIsDeath())
        {
            Destroy(gameObject);
        }
    }

    void UpdateHp()
    {
        int currentHp = boss.GetCurrentHp();
        HpSlinder.value = boss.GetRatio();
        textHP.text = currentHp.ToString("D4");
    }
}
