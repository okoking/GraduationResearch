using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player; // �v���C���[��Transform���w��
    [SerializeField] Vector3 offset = new Vector3(0, 5, -10); // �v���C���[����̋���
    [SerializeField] float followSpeed = 5f; // �Ǐ]�X�s�[�h

    void LateUpdate()
    {
        if (player == null) return;

        // �ڕW�ʒu���v�Z
        Vector3 targetPos = player.position + offset;

        // �X���[�Y�ɒǏ]
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);

        // �v���C���[�̕���������
        transform.LookAt(player);
    }
}