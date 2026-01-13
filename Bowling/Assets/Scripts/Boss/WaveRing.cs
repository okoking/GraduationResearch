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
        scale.z += speed * Time.deltaTime;
        scale.y = 0.1f; // © c‚ÍŒÅ’è
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
            //other.GetComponent<PlayerStatus>().Damage(10);
        }
    }

}