using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MissileCollision : MonoBehaviour
{

    public Missile shooter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);  //��������
            shooter.ResetShoot(); //�ēx���ˉ\��
            Debug.Log("������");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);  //��������
            shooter.ResetShoot(); //�ēx���ˉ\��
        }
    }
}
