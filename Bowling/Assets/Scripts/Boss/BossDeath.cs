using UnityEngine;

public class BossDeath : MonoBehaviour
{
    private BossHp bosshp;

    float height;

    float startY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bosshp = GameObject.Find("Boss").GetComponent<BossHp>();

        height = GetComponent<Renderer>().bounds.size.y;

        startY = transform.position.y; 
    }

    // Update is called once per frame
    void Update()
    {
        if (bosshp.GetIsDeath() && transform.position.y < startY + height)
        {
            Debug.Log("Ž€‚ñ‚¾‚Ì‚Å“®‚«‚Ü‚·");
            transform.Translate(Vector3.up * height * Time.deltaTime);
        }
    }
}