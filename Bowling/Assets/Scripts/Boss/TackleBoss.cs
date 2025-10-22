using UnityEngine;

public class TackleBoss : MonoBehaviour
{
    public float speed = 3f;               // �ʏ�̈ړ����x
    public float tackleSpeed = 10f;        // �^�b�N�����̑��x
    public float stopDistance = 10f;      // �߂Â�����
    public float tackleDuration = 1.0f;    // �^�b�N���̎�������
    public float stunTime = 2.0f;          // �X�^������
    public float tackleCooldown = 3.0f;    // �^�b�N���̍Ďg�p�܂ł̎���

    private Transform target;
    private bool isTackling = false;
    private bool isStunned = false;
    private bool isOnCooldown = false;
    private float tackleTimer = 0f;
    private float stunTimer = 0f;
    private float cooldownTimer = 0f;

    //�̗�
    public int hitPoint = 1;

    void Start()
    {
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

        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
                isStunned = false;
            return;
        }

        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
                isOnCooldown = false;
        }

        //===== �^�b�N���� =====
        if (isTackling)
        {
            tackleTimer -= Time.deltaTime;

            // forward�����ɐi�ނ��ǁAY���͌Œ�
            Vector3 moveDir = transform.forward;
            moveDir.y = 0; // Y�����̓���������
            transform.position += moveDir.normalized * tackleSpeed * Time.deltaTime;

            if (tackleTimer <= 0)
                EndTackle();

            return;
        }

        // ===== �ʏ�ǐ� =====
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > stopDistance)
        {
            // �v���C���[��Y�𖳎����Đ����Ɍ���
            Vector3 lookPos = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.LookAt(lookPos);

            // �ړ����Y���Œ�
            Vector3 nextPos = Vector3.MoveTowards(transform.position, lookPos, speed * Time.deltaTime);
            nextPos.y = transform.position.y;
            transform.position = nextPos;
        }
        else
        {
            if (!isOnCooldown)
                StartTackle();
        }

        //HP0�ȉ��ɂȂ�����
        Death();
    }

    void StartTackle()
    {
        isTackling = true;
        tackleTimer = tackleDuration;
        isOnCooldown = true;
        cooldownTimer = tackleCooldown;
        transform.LookAt(target);
        Debug.Log("�^�b�N���J�n�I");
    }

    void EndTackle()
    {
        isTackling = false;
        Debug.Log("�^�b�N���I��");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTackling)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("�v���C���[�ɖ����I");
                //�v���C���[�Ƀ_���[�W
            }
            if (collision.gameObject.CompareTag("Wall"))
            {
                Debug.Log("�ǂȂǂɏՓ� �� �X�^��");
                StartStun();
            }

            EndTackle();
        }

        //�v���C���[�̍U���Ƃ̓����蔻��//��//
        if (collision.gameObject.CompareTag("Player"))
        {
            TakeDamage();
        }
    }

    void StartStun()
    {
        isStunned = true;
        stunTimer = stunTime;
        Debug.Log("�X�^�����");
    }

    //�_���[�W���󂯂�
    void TakeDamage()
    {
        //�X�^�����ł���΃_���[�W���ʂ�
        if (isStunned)
        {
            hitPoint--;
        }
    }

    void Death()
    {
        if(hitPoint <= 0)
        {
            Destroy(gameObject);
        }
    }
}

//�v���C���[�ɋ߂Â�
//�߂Â�����^�b�N��
//�v���C���[�ɓ�����΃_���[�W
//�v���C���[�ɂ悯���āA�ǂ��Q���ɂԂ���΃X�^�����ă_���[�W���ʂ�悤�ɂȂ�
//�����i�̓_���[�W���ʂ�Ȃ�