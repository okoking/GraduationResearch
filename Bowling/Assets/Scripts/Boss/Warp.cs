using UnityEngine;

public class Warp : MonoBehaviour
{

    public GameObject destination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(destination != null)
        {
            Debug.Log("“ü‚Á‚Ä‚Ü‚·");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterController cc = other.GetComponent<CharacterController>();

        if (cc != null)
        {
            cc.enabled = false; // ‚¢‚Á‚½‚ñ–³Œø‰»
            other.transform.position = destination.transform.position;
            cc.enabled = true;  // Ä“x—LŒø‰»
        }
    }
}