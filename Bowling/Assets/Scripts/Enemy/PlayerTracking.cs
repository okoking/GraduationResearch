using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerTracking : MonoBehaviour
{
    //�ړ����x
    public float speed = 3f;

    //�v���C���[�ɂǂ��܂ŋ߂Â���,�U���\�ɂ��邩
    public float stopDistance = 1.5f;

    //�ǐՑΏ�(�v���C���[)
    Transform target;

    //�v���C���[���w��͈͓��ɂ��邩
    bool withinRange = false;

    void Start()
    {
        //�v���C���[�^�O�����I�u�W�F�N�g��T��
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player�^�O�����I�u�W�F�N�g��������܂���ł���");
        }
    }

    void Update()
    {
        if (target == null) return;

        //����
        float distance = Vector3.Distance(transform.position, target.position);
        //��苗���ɋ߂Â��܂Œǐ�
        if (distance > stopDistance)
        {
            //�Ώ�(�v���C���[)�̕�������������
            transform.LookAt(target);
            //�i�s�����ɐi��
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            //�����ĂȂ�
            withinRange = false;
        }
        //�����łȂ����
        else
        {
            //�����Ă���
            withinRange = true;
        }
    }

    public bool GetRange()
    {
        return withinRange;
    }
}
