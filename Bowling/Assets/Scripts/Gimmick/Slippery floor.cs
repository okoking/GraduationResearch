using UnityEngine;

public class Slipperyfloor : MonoBehaviour
{
    public float SlowSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerStay(Collider other)
    {
        //if (other.CompareTag("Ball"))
        //{
        //    Debug.Log("すり抜けスローダウン" + other.gameObject.name);
        //    // 現在の速度を取得
        //    Rigidbody rb = other.attachedRigidbody;

        //    //オブジェクトの直線方向の移動速度
        //    Vector3 originalVelocity = rb.linearVelocity;
        //    // 減速倍率をかけて新しい速度を計算
        //    Vector3 slowedVelocity = originalVelocity * SlowSpeed;
        //    //減速を適用
        //    rb.linearVelocity = slowedVelocity;
        //}
    }
}
