using UnityEngine;

public class Slipperyfloor : MonoBehaviour
{
    public float SlowSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerStay(Collider other)
    {
        //if (other.CompareTag("Ball"))
        //{
        //    Debug.Log("���蔲���X���[�_�E��" + other.gameObject.name);
        //    // ���݂̑��x���擾
        //    Rigidbody rb = other.attachedRigidbody;

        //    //�I�u�W�F�N�g�̒��������̈ړ����x
        //    Vector3 originalVelocity = rb.linearVelocity;
        //    // �����{���������ĐV�������x���v�Z
        //    Vector3 slowedVelocity = originalVelocity * SlowSpeed;
        //    //������K�p
        //    rb.linearVelocity = slowedVelocity;
        //}
    }
}
