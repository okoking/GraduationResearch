using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MissileCollision : MonoBehaviour
{
    public  Missile shooter;
    public float killHeight = -5f;

    void Update()
    {
        Debug.Log("Bullet height: " + transform.position.y);
        if (transform.position.y < killHeight)
        {
            if (shooter != null)
            {
                shooter.ResetShoot(); // ★ 自分を撃った Shooter のみをリセット
            }

            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Kill();
        }
    }

    void Kill()
    {
        if (shooter != null)
        {
            shooter.ResetShoot(); // ★ 自分を撃った Shooter のみをリセット
        }

        Destroy(gameObject);
    }
}
