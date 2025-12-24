using System.Collections;
using UnityEngine;

public class BossDeath : MonoBehaviour
{
    private BossHp bosshp;

    public void SetBoss(BossHp hp)
    {
        bosshp = hp;
    }

    void Start()
    {

    }

    void Update()
    {
        Debug.Log(bosshp);

        if (bosshp == null) return;

        Debug.Log("死んでない！！！！！！！！！！！！");

        if (bosshp.GetIsDeath())
        {
            Debug.Log("死んだので動きます");
            Destroy(gameObject);
        }
    }
}