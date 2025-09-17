using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float upPower = 10.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log("���蔲������: " + other.gameObject.name);
            Rigidbody ballRb = other.GetComponent<Rigidbody>();  //Rigidbody�͕������Z�ŗ͂��������葬�x��ς����肷��̂ɕK�v�B���������I�u�W�F�N�g�̏��������Ă���

            // �W�����v�p�b�h�̌�����͂������Ŏw��
            if (ballRb != null)                                 //�I�u�W�F�N�g���擾�o������
            {
                ballRb.AddForce(Vector3.up * upPower, ForceMode.VelocityChange);
            }
        }
    }
}
