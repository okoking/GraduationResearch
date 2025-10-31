using UnityEngine;

public class LockOnSystem : MonoBehaviour
{
    public Transform lockOnTarget;
    [SerializeField] private float lockOnRange = 20f;

    void Update()
    {
        if (Input.GetKeyDown("joystick button 1"))
        {
            lockOnTarget = FindClosestEnemy()?.transform;
        }

        // �Ώۂ������������
        if (lockOnTarget == null)
            return;

        // ��苗���O�ꂽ�����
        if (Vector3.Distance(transform.position, lockOnTarget.position) > lockOnRange)
        {
            lockOnTarget = null;
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDist && dist < lockOnRange)
            {
                closest = enemy;
                minDist = dist;
            }
        }

        return closest;
    }
}
