using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Wall : MonoBehaviour
{
    public float speed = 1f;       // 移動速度（毎秒）
    public float minX = -5f;       // 左端の座標
    public float maxX = 5f;        // 右端の座標
    public bool StartPoint = true;
    private bool IsUse=true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 InitPos = transform.position;
        maxX += transform.position.x;
        minX += transform.position.x;
        
        if(StartPoint==true)
        {
            IsUse = true;
        }
        if (StartPoint == false)
        {
            IsUse = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float currentX = transform.position.x;
       
        if(IsUse==true)
        {
            transform.position -= transform.right*speed * Time.deltaTime;
            if (transform.position.x <= minX)
            {
                IsUse = false;
            }
        }
        if (IsUse == false)
        {
            transform.position += transform.right*speed * Time.deltaTime;
            if (transform.position.x >= maxX)
            {
                IsUse = true;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("衝突した: " + gameObject.name);
            

        }
    }
    public Color lineColor = Color.red;
    public Color rangeLineColor = Color.yellow;

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = lineColor;

    //    float left = transform.position.x + minX;
    //    float right = transform.position.x + maxX;

    //    // 左端
    //    Gizmos.DrawLine(
    //        new Vector3(left, transform.position.y - 1, transform.position.z),
    //        new Vector3(left, transform.position.y + 1, transform.position.z)
    //    );

    //    // 右端
    //    Gizmos.DrawLine(
    //        new Vector3(right, transform.position.y - 1, transform.position.z),
    //        new Vector3(right, transform.position.y + 1, transform.position.z)
    //    );
    //    // 中心線
    //    Gizmos.color = rangeLineColor;
    //    Gizmos.DrawLine(
    //        new Vector3(left, transform.position.y, transform.position.z),
    //        new Vector3(right, transform.position.y, transform.position.z)
    //    );
    //}
}
