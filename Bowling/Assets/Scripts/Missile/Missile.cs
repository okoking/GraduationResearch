using UnityEngine;

public class Missile : MonoBehaviour
{
    public string targetTag = "Enemy";       // ターゲットのタグ
    public float flightTime = 1f;            // 飛ぶ時間（秒）
    public GameObject projectilePrefab;      // 発射する球のプレハブ
    public Transform spawnPoint;             // 発射位置
    public float maxHorizontalAngle = 15f;   // 左右ランダム角度（度）

    private static bool canShoot = true;     // 発射可能か

    void Update()
    {
        if (canShoot && Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // 球を生成
        GameObject bullet = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);

        // Rigidbodyを取得
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // 最も近いターゲットを探す
        Transform nearest = FindNearestTarget();
        if (nearest == null) return;

        // 初速を計算
        Vector3 startPos = spawnPoint.position;
        Vector3 targetPos = nearest.position;

        Vector3 diff = targetPos - startPos;
        Vector3 horizontal = new Vector3(diff.x, 0, diff.z);

        float vx = horizontal.magnitude / flightTime;
        float vy = (diff.y / flightTime) + 0.5f * Mathf.Abs(Physics.gravity.y) * flightTime;

        Vector3 velocity = horizontal.normalized * vx + Vector3.up * vy;
        rb.linearVelocity = velocity;

        // 衝突で消すスクリプトを追加
        MissileCollision bc = bullet.AddComponent<MissileCollision>();
        bc.shooter = this; // ★ 発射元をしっかり渡す！

        canShoot = false;
    }

    Transform FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        if (targets.Length == 0) return null;

        Transform nearest = targets[0].transform;
        float minDist = Vector3.Distance(spawnPoint.position, nearest.position);

        foreach (GameObject t in targets)
        {
            float dist = Vector3.Distance(spawnPoint.position, t.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = t.transform;
            }
        }
        return nearest;
    }

    // 衝突後に再度発射可能にする
    public void ResetShoot()
    {
        canShoot = true;
    }
}
