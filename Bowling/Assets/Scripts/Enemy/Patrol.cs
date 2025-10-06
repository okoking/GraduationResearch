using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] Transform target; // �ǂ����������I�u�W�F�N�g
    [SerializeField] Vector3 offset = new Vector3(0, 5, -10); // �J�����̈ʒu�␳
    [SerializeField] float smoothSpeed = 5f; // ��Ԃ̑���

    void LateUpdate()
    {
        if (target == null) return;

        // �ڕW�ʒu�i�^�[�Q�b�g�̈ʒu�{�I�t�Z�b�g�j
        Vector3 desiredPosition = target.position + offset;

        // ��Ԃ��ăX���[�Y�ɒǏ]
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;

        // ��Ƀ^�[�Q�b�g�𒍎�
        transform.LookAt(target);
    }
}
