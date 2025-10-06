using UnityEngine;

public class HitPointManager : MonoBehaviour
{

    public  int  MaxHp;      //�ő�HP
    private int  currentHp;  //���݂�HP
    private bool isActive;   //��������(false�Ŏ��S)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = MaxHp;  //�ŏ��͍ő�HP����n�܂�
        isActive = true;    //�������Ă���
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�_���[�W���󂯂鏈��
    public void TakeDamage(int damage)
    {
        //���������Ă��Ȃ���Ύ��s���Ȃ�
        if (!isActive) return;
        //�^����_���[�W�͕��̒l�̓_��
        if (damage < 0) damage = 0;
        //�_���[�W���󂯂�
        currentHp -= damage;
        //HP��0�ȉ��ɂȂ�����
        if (currentHp <= 0)
        {
            //0�ȉ��ɂ͂Ȃ�Ȃ�
            currentHp = 0;
            //��//
            Die();
        }
    }

    //���S����
    private void Die()
    {
        isActive = false;   //���񂾂��Ƃɂ���
        Destroy(gameObject);//�I�u�W�F�N�g������
    }

    
    public int GetCurrentHp() {  return currentHp; }
}
