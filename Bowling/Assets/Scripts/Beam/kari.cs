using UnityEngine;
using UnityEngine.VFX;

public class kari : MonoBehaviour
{

    public VisualEffect currentVFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            currentVFX.SendEvent("OnPlay");
            Debug.Log("アニメーション開始");

        }
    }
}
