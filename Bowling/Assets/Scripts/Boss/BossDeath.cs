using UnityEngine;

public class BossDeath : MonoBehaviour
{
    private Boss boss;

    float height;

    float startY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boss = GameObject.Find("Boss").GetComponent<Boss>();

        height = GetComponent<Renderer>().bounds.size.y;

        startY = transform.position.y; 
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.ReturnDeath() && transform.position.y < startY + height)
        {
            Debug.Log("Ž€‚ñ‚¾‚Ì‚Å“®‚«‚Ü‚·");
            transform.Translate(Vector3.up * height * Time.deltaTime);
        }
    }
}