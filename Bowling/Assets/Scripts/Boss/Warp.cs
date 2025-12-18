using UnityEngine;

public class Warp : MonoBehaviour
{

    public GameObject destination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(destination != null)
        {
            Debug.Log("入ってます");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //なんかCharacterController複雑怪奇ニキ
        CharacterController cc = other.GetComponent<CharacterController>();

        if (cc != null)
        {
            cc.enabled = false; // いったん無効化
            other.transform.position = destination.transform.position;
            cc.enabled = true;  // 再度有効化
        }
    }
}