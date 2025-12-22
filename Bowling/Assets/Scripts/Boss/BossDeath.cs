using System.Collections;
using UnityEngine;

public class BossDeath : MonoBehaviour
{
    private BossHp bosshp;

    float height;
    float startY;

    void Start()
    {
        height = GetComponent<Renderer>().bounds.size.y;
        startY = transform.position.y;

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

        if (bosshp.GetIsDeath() && transform.position.y < startY + height)
        {
            Debug.Log("Ž€‚ñ‚¾‚Ì‚Å“®‚«‚Ü‚·");
            transform.Translate(Vector3.up * 1f * Time.deltaTime);
        }
    }
}
