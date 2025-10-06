using UnityEngine;

public class FollowBall : MonoBehaviour
{
    private Transform target;     // �{�[���i�Ǐ]�Ώہj
    public Vector3 offset;       // ���Έʒu

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject ballObj = GameObject.FindGameObjectWithTag("Ball");
        if (ballObj != null)
        {
            target = ballObj.transform;
        }
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
