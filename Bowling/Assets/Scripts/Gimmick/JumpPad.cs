using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float upPower = 10.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log("すり抜け判定: " + other.gameObject.name);
            Rigidbody ballRb = other.GetComponent<Rigidbody>();  //Rigidbodyは物理演算で力を加えたり速度を変えたりするのに必要。当たったオブジェクトの情報を持ってくる

            // ジャンプパッドの向きや力をここで指定
            if (ballRb != null)                                 //オブジェクトが取得出来たら
            {
                ballRb.AddForce(Vector3.up * upPower, ForceMode.VelocityChange);
            }
        }
    }
}
