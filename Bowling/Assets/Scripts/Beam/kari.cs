using UnityEngine;
using UnityEngine.VFX;

public class kari : MonoBehaviour
{

    public VisualEffect currentVFX;
    bool karifalg = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !karifalg)
        {
            //currentVFX.SendEvent("OnPlay");
            //currentVFX.SendEvent(VisualEffectAsset.PlayEventName);
            currentVFX.Play();
            Debug.Log("エフェクト開始");
            karifalg = true;
        }
    }
}
