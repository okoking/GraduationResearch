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
        if (ps && !ps.IsAlive()) // ‚Ü‚¾“®‚¢‚Ä‚é‚©Šm”F
        {
            Destroy(gameObject);
        }
    }
}
