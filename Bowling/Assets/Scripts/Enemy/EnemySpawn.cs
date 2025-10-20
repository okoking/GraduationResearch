using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    //�I���W���v���n�u
    public GameObject enemyPrefab;

    //���̐��ނ�
    public int spawnNum;

    //�G�������W
    public Vector3 spawnPos;

    //���b�����ɐ������邩
    public float spawnSpeed;

    //�b���J�E���g�ϐ�
    float count = 0.0f;

    //�X�|�[�������������t���O
    bool finSpawn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�������ĂȂ����
        if (!finSpawn)
        {
            for (int i = 0; i < spawnNum; i++)
            {
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            }
            //����������I��
            finSpawn = true;
        }

        //�������Ă���Έ����Ԍ�ɍĐ���
        if (finSpawn)
        {
            count++;
            //�J�E���g���w��b���ɒB������
            if (count > spawnSpeed) {
                finSpawn = false;
                count = 0f;
            }
        }
    }
}