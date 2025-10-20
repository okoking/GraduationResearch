using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerTracking : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float stopDistance = 1.5f;

    Transform target;

    bool withinRange = false;

    void Start()
    {

        // �V�[����́uPlayer�v�^�O�����I�u�W�F�N�g��T��
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

        float distance = Vector3.Distance(transform.position, target.position);
        //��苗���ɋ߂Â��܂Œǐ�
        if (distance > stopDistance)
        {
            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );
            withinRange = false;
        }
        //�����łȂ����
        else
        {
            withinRange = true;
        }
    }

    public bool GetRange()
    {
        return withinRange;
    }
}
