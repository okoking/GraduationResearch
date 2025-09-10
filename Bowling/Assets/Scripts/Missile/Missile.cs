using UnityEngine;

public class Missile : MonoBehaviour
{
    public string targetTag = "Enemy";       // �^�[�Q�b�g�̃^�O
    public float flightTime = 1f;            // ��Ԏ��ԁi�b�j
    public GameObject projectilePrefab;      // ���˂��鋅�̃v���n�u
    public Transform spawnPoint;             // ���ˈʒu

    public float randomAngle = 5f; // �x��

    private bool canShoot = true;            // ���ˉ\��

    void Update()
    {
        if (canShoot && Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // ���𐶐�
        GameObject bullet = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);

        // Rigidbody���擾
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // �ł��߂��^�[�Q�b�g��T��
        Transform nearest = FindNearestTarget();
        if (nearest == null) return;

        // �������v�Z
        Vector3 startPos = spawnPoint.position;
        Vector3 targetPos = nearest.position;

        Vector3 diff = targetPos - startPos;
        Vector3 horizontal = new Vector3(diff.x, 0, diff.z);

        float vx = horizontal.magnitude / flightTime;
        float vy = (diff.y / flightTime) + 0.5f * Mathf.Abs(Physics.gravity.y) * flightTime;

        Vector3 velocity = horizontal.normalized * vx + Vector3.up * vy;

        // �����_���Ɍ�������]������
        Quaternion randomRot = Quaternion.Euler(
            Random.Range(-randomAngle, randomAngle), // �㉺�����̃u��
            Random.Range(-randomAngle, randomAngle), // ���E�����̃u��
            0
        );

        // velocity �������_����]
        velocity = randomRot * velocity;

        rb.linearVelocity = velocity;

        // �Փ˂ŏ����X�N���v�g��ǉ�
        MissileCollision bc = bullet.AddComponent<MissileCollision>();
        bc.shooter = this;

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

    // �Փˌ�ɍēx���ˉ\�ɂ���
    public void ResetShoot()
    {
        canShoot = true;
    }
}
