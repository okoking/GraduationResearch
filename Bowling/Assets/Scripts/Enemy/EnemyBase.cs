using UnityEngine;

public class EnemyBase : MonoBehaviour
{

    private HitPointManager enemyHp;

    private Rigidbody enemyRd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyHp = GetComponent<HitPointManager>();

        //���̃I�u�W�F�N�g�̃��W�b�h�{�f�B���擾
        enemyRd = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Enemy HP: " + enemyHp.GetCurrentHp());
        }
        Debug.Log("Enemy Spd: " + enemyRd);
    }

    //�����蔻��
    private void OnCollisionEnter(Collision collision)
    {
        //�{�[���Ƃ̓����蔻��
        if (collision.gameObject.CompareTag("Ball"))
        {
            enemyHp.TakeDamage((int)enemyRd.linearVelocity.magnitude);
        }
    }

    public HitPointManager GetEnemyHp() { return enemyHp; }
}