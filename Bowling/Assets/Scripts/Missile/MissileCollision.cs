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
            Destroy(gameObject);  //球を消す
            shooter.ResetShoot(); //再度発射可能に
            Debug.Log("おちた");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);  //球を消す
            shooter.ResetShoot(); //再度発射可能に
        }
    }
}
