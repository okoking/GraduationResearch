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

        // ===== �X�^���� =====
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
                isStunned = false;
            return;
        }

        // ===== �N�[���^�C���� =====
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
                isOnCooldown = false;
        }

        // ===== �^�b�N���� =====
        if (isTackling)
        {
            tackleTimer -= Time.deltaTime;
            transform.position += transform.forward * tackleSpeed * Time.deltaTime;

            if (tackleTimer <= 0)
                EndTackle();

            return;
        }

        // ===== �ʏ�̒ǐ� =====
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > stopDistance)
        {
            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            // �N�[���^�C�����I����Ă�����^�b�N��
            if (!isOnCooldown)
                StartTackle();
        }
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
                // �v���C���[�Ƀ_���[�W����������
            }
            else
            {
                Debug.Log("�ǂȂǂɏՓ� �� �X�^��");
                StartStun();
            }

            EndTackle();
        }
    }

    void StartStun()
    {
        isStunned = true;
        stunTimer = stunTime;
        Debug.Log("�X�^�����");
    }
}

//�v���C���[�ɋ߂Â�
//�߂Â�����^�b�N��
//�v���C���[�ɓ�����΃_���[�W
//�v���C���[�ɂ悯���āA�ǂ��Q���ɂԂ���΃X�^�����ă_���[�W���ʂ�悤�ɂȂ�
//�����i�̓_���[�W���ʂ�Ȃ�