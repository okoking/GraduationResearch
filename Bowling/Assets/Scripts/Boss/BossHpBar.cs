using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    [SerializeField] private Slider HpSlinder;
    [SerializeField] private TextMeshProUGUI textHP;
    private BossHp boss;

    private int currentHp;

    void Start()
    {
        StartCoroutine(WaitForBoss());
    }

    IEnumerator WaitForBoss()
    {
        while (boss == null)
        {
            GameObject bossObj = GameObject.FindGameObjectWithTag("Boss");
            if (bossObj != null)
            {
                boss = bossObj.GetComponent<BossHp>();
                break;
            }
            yield return null;
        }

        currentHp = boss.GetCurrentHp();
        HpSlinder.value = boss.GetRatio();
        textHP.text = currentHp.ToString("D4");
    }

    void Update()
    {
        if (boss == null) return;

        TakeDamage();

        if (boss.GetIsDeath())
        {
            Destroy(gameObject);
        }
    }

    void TakeDamage()
    {
        currentHp = boss.GetCurrentHp();
        HpSlinder.value = boss.GetRatio();
        textHP.text = currentHp.ToString("D4");
    }
}
