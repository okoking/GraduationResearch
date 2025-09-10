using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Wall : MonoBehaviour
{
    public float speed = 2f;       // 移動速度（毎秒）
    public float minX = -5f;       // 左端の座標transform.position.x
    public float maxX = 5f;        // 右端の座標
    bool IsUse;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxX += transform.position.x;
        minX += transform.position.x;
        IsUse = true;
    }

    // Update is called once per frame
    void Update()
    {
        float currentX = transform.position.x;
       
        if(IsUse==true)
        {
            currentX--;
            if (transform.position.x <= minX)
            {
                IsUse = false;
            }
        }
        if (IsUse == false)
        {
            currentX++;
            if (transform.position.x >= maxX)
            {
                IsUse = true;
            }
        }
       
        Vector3 UpDataPos;
        UpDataPos.x = currentX;
        UpDataPos.y = 0;
        UpDataPos.z = 0;
        transform.position = UpDataPos;
    }
    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("衝突した: " + gameObject.name);
            

        }
    }
}
