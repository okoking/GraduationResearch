using UnityEngine;

public class WaveRing : MonoBehaviour
{
    float speed;

    float maxRadius;

    public void Init(float startRadius, float speed, float maxRadius)
    {
        this.speed = speed;
        this.maxRadius = maxRadius;
        transform.localScale = Vector3.one * startRadius;
    }

    void Update()
    {
        Vector3 scale = transform.localScale;
        scale.x += speed * Time.deltaTime;
        scale.z = 1f;
        scale.y += speed * Time.deltaTime;
        transform.localScale = scale;

        if (transform.localScale.x >= maxRadius)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //プレイヤーにダメージ
            other.GetComponent<PlayerHealth>().TakeDamage(100);
        }
    }
}