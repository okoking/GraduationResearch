using UnityEngine;
using static UnityEditorInternal.ReorderableList;

public class EnemyBase : MonoBehaviour
{
    public float knockbackPower = 10f;  // �Ԃ���΂�����
    public float upPower = 4.5f;          // ������ɏ�����������

    private HitPointManager enemyHp;

    private Rigidbody enemyRd;

    private Vector3 defaultPos;

    public float minY = -5f;          // y���W������ȉ��Ȃ烊�Z�b�g
    public float stopThreshold = 0.1f; // ���x�����̈ȉ��Ȃ��~�Ƃ݂Ȃ�
    public float checkDelay = 1f;      // �n�ʂɗ����Ă��画����n�߂�x���i�b�j

    private float groundedTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyHp = GetComponent<HitPointManager>();

        //���̃I�u�W�F�N�g�̃��W�b�h�{�f�B���擾
        enemyRd = this.GetComponent<Rigidbody>();

        defaultPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < minY)
        {
            ResetEnemy();
        }

        // �X�s�[�h�Ɖ�]���~�܂�����
        if (enemyRd.linearVelocity.magnitude < stopThreshold && enemyRd.angularVelocity.magnitude < stopThreshold)
        {
            groundedTime += Time.deltaTime;
            if (groundedTime >= checkDelay)
            {
                ResetEnemy();
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
            enemyHp.TakeDamage((int)enemyRd.linearVelocity.magnitude);

            // �e�̐i�s�������g���Đ�����΂�
            Vector3 forceDir = (transform.position - collision.transform.position).normalized;
            forceDir += Vector3.up * upPower; // ������ɂ������͂�������

            enemyRd.AddForce(forceDir.normalized * knockbackPower, ForceMode.Impulse);
        }

        //�~�T�C���Ɠ���������~�T�C��������,�_���[�W���󂯂�
        if (collision.gameObject.CompareTag("Missile"))
        {
            Destroy(collision.gameObject);

            enemyHp.TakeDamage(1);
        }
    }

    public HitPointManager GetEnemyHp() { return enemyHp; }

    private void ResetEnemy()
    {
        enemyRd.linearVelocity = Vector3.zero;
        enemyRd.angularVelocity = Vector3.zero;
        transform.position = defaultPos;
        transform.rotation = Quaternion.identity;
        groundedTime = 0f;
    }
}