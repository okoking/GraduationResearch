using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    public float moveSpeed = 10f;      //�ړ����x
    public float boostMultiplier = 3f; //Shift �ŉ���
    public float lookSpeed = 2f;       //���_�ړ����x

    private float yaw;
    private float pitch;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        //�J�[�\�������b�N���ă}�E�X���_�����L���ɂ���
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //�}�E�X�Ŏ��_��]
        yaw += Input.GetAxis("Mouse X") * lookSpeed;
        pitch -= Input.GetAxis("Mouse Y") * lookSpeed;
        pitch = Mathf.Clamp(pitch, -89f, 89f);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        //�ړ�����
        float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? boostMultiplier : 1f);

        Vector3 move = new Vector3(
            Input.GetAxis("Horizontal"),   //A/D
            0,
            Input.GetAxis("Vertical")      //W/S
        );

        //�J�����̌�����ňړ�
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 direction = forward * move.z + right * move.x;

        //�㉺�ړ��i�X�y�[�X��Ctrl�j
        if (Input.GetKey(KeyCode.Q)) direction += Vector3.up;
        if (Input.GetKey(KeyCode.E)) direction += Vector3.down;

        transform.position += direction * speed * Time.deltaTime;
    }
}

