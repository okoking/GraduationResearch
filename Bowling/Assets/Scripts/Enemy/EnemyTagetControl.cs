using UnityEngine;

public class EnemyTagetControl : MonoBehaviour
{

    public float speed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal"); // A,D or ��,��
        float v = Input.GetAxis("Vertical");   // W,S or ��,��

        Vector3 move = new Vector3(h, 0, v); // Y�͌Œ�iXZ�ړ������j
        transform.position += move * speed * Time.deltaTime;
    }
}

//�r�[�������f�����b�g
//