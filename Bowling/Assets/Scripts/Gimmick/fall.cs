using UnityEngine;

public class fall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初期化

    }

    // Update is called once per frame
    void Update()
    {
        //ステップ

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("衝突した: " + gameObject.name);
            // 衝突した相手のGameObject
            GameObject otherObject = collision.gameObject;
            otherObject.SetActive(false);
        }
    }
}
