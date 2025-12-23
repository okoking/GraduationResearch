using System.Collections;
using UnityEngine;

public class BossDeath : MonoBehaviour
{
    private BossHp bosshp;

    float height;
    float startY;

    bool isDeath;

    void Start()
    {
        height = GetComponent<Renderer>().bounds.size.y;
        startY = transform.position.y;
        isDeath = false;

        StartCoroutine(WaitForBoss());
    }

    IEnumerator WaitForBoss()
    {
        while (bosshp == null)
        {
            GameObject bossObj = GameObject.FindGameObjectWithTag("Boss");
            if (bossObj != null)
            {
                bosshp = bossObj.GetComponent<BossHp>();
                break;
            }
            yield return null;
        }
    }

    void Update()
    {
        if (bosshp == null) return;

        if (bosshp.GetIsDeath())
        {
            isDeath = true;
        }

        if (isDeath && transform.position.y < startY + height)
        {
            Debug.Log("Ž€‚ñ‚¾‚Ì‚Å“®‚«‚Ü‚·");
            //Destroy(gameObject);
            transform.Translate(Vector3.up * 1f * Time.deltaTime);
        }
    }
}
