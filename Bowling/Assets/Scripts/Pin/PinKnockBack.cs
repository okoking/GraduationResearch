using UnityEngine;

public class PinKnockBack : MonoBehaviour
{
    public float knockbackPower = 10f;  //�Ԃ���΂�����
    public float upPower = 4.5f;          //������ɏ�����������

    private Rigidbody pinRd;

    private EnemyBase enemybase;

    private Vector3 defaultPos;
    private Quaternion defaultRot;

    public float minY = -5f;          //y���W������ȉ��Ȃ烊�Z�b�g
    public float stopThreshold = 0.1f; //���x�����̈ȉ��Ȃ��~�Ƃ݂Ȃ�
    public float checkDelay = 1f;      //�n�ʂɗ����Ă��画����n�߂�x���i�b�j

    private float groundedTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //���̃I�u�W�F�N�g�̃��W�b�h�{�f�B���擾
        pinRd = this.GetComponent<Rigidbody>();

        enemybase = GetComponent<EnemyBase>();

        defaultPos = transform.position;
        defaultRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < minY)
        {
            ResetPin();
        }

        // �X�s�[�h�Ɖ�]���~�܂�����
        if (pinRd.linearVelocity.magnitude < stopThreshold && pinRd.angularVelocity.magnitude < stopThreshold)
        {
            groundedTime += Time.deltaTime;
            if (groundedTime >= checkDelay)
            {
                ResetPin();
            }
        }
        else
        {
            groundedTime = 0f; // �����Ă��烊�Z�b�g
        }
    }

    //�����蔻��
    private void OnCollisionEnter(Collision collision)
    {
        //�{�[���Ƃ̓����蔻��
        if (collision.gameObject.CompareTag("Ball"))
        {
            //�e�̐i�s�������g���Đ�����΂�
            Vector3 forceDir = (transform.position - collision.transform.position).normalized;
            forceDir += Vector3.up * upPower; //������ɂ������͂�������

            pinRd.AddForce(forceDir.normalized * knockbackPower, ForceMode.Impulse);

            FindFirstObjectByType<EnemyBase>().GetEnemyHp().TakeDamage((int)pinRd.linearVelocity.magnitude);

            // �~�b�V�����ɒʒm
            FindFirstObjectByType<MissionManager>().HitEnemy();
        }

        //�~�T�C���Ɠ���������~�T�C��������,�_���[�W���󂯂�
        if (collision.gameObject.CompareTag("Missile"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void ResetPin()
    {
        pinRd.linearVelocity = Vector3.zero;
        pinRd.angularVelocity = Vector3.zero;
        transform.position = defaultPos;
        transform.rotation = defaultRot;
        groundedTime = 0f;
    }
}
