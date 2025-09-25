using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Transform[] waypoints; // �ʂ�I�u�W�F�N�g�i���ԁj
    [SerializeField] float speed = 2f;      // �ړ����x
    [SerializeField] float arriveDistance = 0.1f; // ��������̋���

    int currentIndex = 0; // ���������Ă���E�F�C�|�C���g

    void Update()
    {
        if (waypoints.Length == 0) return;

        // ���̖ڕW
        Transform target = waypoints[currentIndex];

        // �^�[�Q�b�g�̕���
        Vector3 dir = (target.position - transform.position).normalized;

        // �ړ�
        transform.position += dir * speed * Time.deltaTime;

        // �߂Â����玟��
        if (Vector3.Distance(transform.position, target.position) < arriveDistance)
        {
            currentIndex++;
            if (currentIndex >= waypoints.Length)
            {
                currentIndex = 0; // �ŏ��ɖ߂�
            }
        }
    }
}
