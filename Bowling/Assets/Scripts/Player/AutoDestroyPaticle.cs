using UnityEngine;

public class AutoDestroyPaticle : MonoBehaviour
{
    private ParticleSystem ps;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ps && !ps.IsAlive()) // �܂������Ă邩�m�F
        {
            Destroy(gameObject);
        }
    }
}
