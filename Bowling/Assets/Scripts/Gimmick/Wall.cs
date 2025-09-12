using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Wall : MonoBehaviour
{
    public float speed = 1f;       // �ړ����x�i���b�j
    public float minX = -5f;       // ���[�̍��W
    public float maxX = 5f;        // �E�[�̍��W
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
            Debug.Log("�Փ˂���: " + gameObject.name);
            

        }
    }
    public Color lineColor = Color.red;
    public Color rangeLineColor = Color.yellow;

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = lineColor;

    //    float left = transform.position.x + minX;
    //    float right = transform.position.x + maxX;

    //    // ���[
    //    Gizmos.DrawLine(
    //        new Vector3(left, transform.position.y - 1, transform.position.z),
    //        new Vector3(left, transform.position.y + 1, transform.position.z)
    //    );

    //    // �E�[
    //    Gizmos.DrawLine(
    //        new Vector3(right, transform.position.y - 1, transform.position.z),
    //        new Vector3(right, transform.position.y + 1, transform.position.z)
    //    );
    //    // ���S��
    //    Gizmos.color = rangeLineColor;
    //    Gizmos.DrawLine(
    //        new Vector3(left, transform.position.y, transform.position.z),
    //        new Vector3(right, transform.position.y, transform.position.z)
    //    );
    //}
}
