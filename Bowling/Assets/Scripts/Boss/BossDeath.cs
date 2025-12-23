using System.Collections;
using UnityEngine;

public class BossDeath : MonoBehaviour
{
    private BossHp bosshp;
    public GameObject bossObj;

    float height;
    float startY;

    bool isDeath;

    public void SetBoss(BossHp hp)
    {
        bosshp = hp;
    }

    void Start()
    {
        height = GetComponent<Renderer>().bounds.size.y;
        startY = transform.position.y;
        isDeath = false;
    }

    void Update()
    {
        if (bosshp == null) return;

        if (bosshp.GetIsDeath())
        {
            Debug.Log("Ž€‚ñ‚¾‚Ì‚Å“®‚«‚Ü‚·");
            Destroy(gameObject);
        }
    }
}