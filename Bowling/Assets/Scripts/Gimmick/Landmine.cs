using UnityEngine;

public class Landmine : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("è’ìÀÇµÇΩ: " + gameObject.name);
            // è’ìÀÇµÇΩëäéËÇÃGameObject
            GameObject otherObject = collision.gameObject;
            otherObject.SetActive(false);
        }
    }
}
