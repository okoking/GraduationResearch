using UnityEngine;

public class FollowBall : MonoBehaviour
{
    public Transform target;     // �{�[���i�Ǐ]�Ώہj
    public Vector3 offset;       // ���Έʒu�i��: (0, 5, -10)�j

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LateUpdate()
    {
        if (target == null) return;

        // �{�[���̈ʒu + �I�t�Z�b�g �ɃJ�������ړ�
        transform.position = target.position + offset;

        // ��Ƀ{�[���̕���������
        transform.LookAt(target);
    }
}
