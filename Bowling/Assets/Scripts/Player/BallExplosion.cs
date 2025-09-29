using UnityEngine;

public class BallExplosion : MonoBehaviour
{
    public GameObject explosionEffect; // �����G�t�F�N�g�i�v���n�u�j
    public float explosionForce = 700f; // ������΂���
    public float explosionRadius = 5f;  // ���ʔ͈�

    private bool isExplosion;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isExplosion = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �G�ɓ���������
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // ��x���������甚�����Ȃ�
            if (isExplosion) return;
            //isExplosion = true;

            // �����G�t�F�N�g����
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            // ���͂̃I�u�W�F�N�g�ɗ͂�������
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

            foreach (Collider nearby in colliders)
            {
                Rigidbody rb = nearby.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }

            // �{�[��������
            //Destroy(gameObject);
        }
    }
}
