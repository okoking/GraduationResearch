using UnityEngine;

public class BallExplosion : MonoBehaviour
{
    public GameObject explosionEffect; // 爆発エフェクト（プレハブ）
    public float explosionForce = 700f; // 吹き飛ばす力
    public float explosionRadius = 5f;  // 効果範囲
    public int explosionDamage;  // 爆発のダメージ

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
        // 敵に当たったら
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 一度爆発したら爆発しない
            if (isExplosion) return;
            //isExplosion = true;

            // 爆発エフェクト生成
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            // 周囲のオブジェクトに力を加える
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

            foreach (Collider nearby in colliders)
            {
                Rigidbody rb = nearby.GetComponent<Rigidbody>();
                HitPointManager hpmang = nearby.GetComponent<HitPointManager>();
                if (rb != null && hpmang != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                    hpmang.TakeDamage(explosionDamage);
                }
            }

            // ボールを消す
            //Destroy(gameObject);
        }
    }
}
