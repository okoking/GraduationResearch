using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float speed = 1f;       // 移動速度（毎秒）
    private bool IsUse = true;
    [ExecuteAlways] // エディタ上でも動くようにする
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float currentX = transform.position.x;

        if (IsUse == true)
        {
            transform.position -= transform.right * speed * Time.deltaTime;
            if (transform.position.x <= minX)
            {
                IsUse = false;
            }
        }
        if (IsUse == false)
        {
            transform.position += transform.right * speed * Time.deltaTime;
            if (transform.position.x >= maxX)
            {
                IsUse = true;
            }
        }
    }
    [Header("移動範囲設定")]
    public float minX = -5f;
    public float maxX = 5f;

    [Header("Gizmoの表示設定")]
    public float gizmoHeight = 2f;
    public Color edgeColor = Color.red;
    public Color lineColor = Color.yellow;

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position;

        float left = pos.x + minX;
        float right = pos.x + maxX;
        //左端の上下の点を設定
        Vector3 bottomLeft = new Vector3(left, pos.y - gizmoHeight / 2f, pos.z);
        Vector3 topLeft = new Vector3(left, pos.y + gizmoHeight / 2f, pos.z);
        //右端の上下の点を設定
        Vector3 bottomRight = new Vector3(right, pos.y - gizmoHeight / 2f, pos.z);
        Vector3 topRight = new Vector3(right, pos.y + gizmoHeight / 2f, pos.z);

        // 左のライン
        Gizmos.color = edgeColor;
        Gizmos.DrawLine(bottomLeft, topLeft);

        // 右のライン
        Gizmos.DrawLine(bottomRight, topRight);

        // 範囲をつなぐライン
        //x,y,zの順でつないでいる
        Gizmos.color = lineColor;
        Gizmos.DrawLine(
            new Vector3(left, pos.y, pos.z),
            new Vector3(right, pos.y, pos.z)
        );
    }
}
