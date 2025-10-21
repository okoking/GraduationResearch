using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //�U���\��
    bool isAttack = true;

    //�U����̃N�[���^�C��
    public float coolTime = 1200f;

    //�N�[���^�C���J�E���g�p
    float timeCnt;

    PlayerTracking playerTracking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTracking = FindAnyObjectByType<PlayerTracking>();
    }

    // Update is called once per frame
    void Update()
    {
        //�U���\�Ńv���C���[�͈͓��ɂ����
        if (isAttack && playerTracking.GetRange())
        {
            //�U������
            Debug.Log("�U���ł�");
            isAttack = false;
        }
        //�U���s�\�ł����
        if (!isAttack)
        {
            //���Ԃ��J�E���g
            timeCnt++;
            //�w�莞�Ԃ𒴂���ƍU���\�ɂȂ�
            if (timeCnt >= coolTime)
            {
                Debug.Log("�����܂�!");
                isAttack = true;
                timeCnt = 0f;
            }
        }
    }
}
