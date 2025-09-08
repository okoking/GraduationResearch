using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody rb;

    public float JUMP_POWER = 1000f;
    public float BALL_SPEED_SCALE = 1000f;

    private Vector3 prevInput = new(0f, 0f, 0f);
    private Vector3 prevMaxInput = new(0f, 0f, 0f);

    private float updateStickPosTime;

    // �W�����v��������
    private bool isJump = false;
    // ���˂��邩
    private bool isShot = false;

    // �W�����v�ł��邩
    private bool isableJump = false;
    // ���˂ł��邩
    private bool isableShot = false;

    //private bool eventStarted = false;

    // ���̒l�b�Ԃ̂����̃X�e�B�b�N�ő���W��ۑ�����
    const float PREVIVENT_STICKPOS_UPDATE_TIME = .5f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        updateStickPosTime = Time.time;
        isableJump = true;
        isableShot = true;
    }

    void Update()
    {
        // �W�����v
        Jump();
        // ����
        Shot();

        // X�Ń��Z�b�g
        if (Input.GetKeyDown("joystick button 2"))
        {
            isableShot = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.position = new(0f, 0.5f, -5f);
        }
    }
    void FixedUpdate()
    {    
        // �ʏ�̏d�͂ɒǉ��ŉ������̗͂�������
        if (isJump)
        {
            rb.AddForce(Vector3.up * JUMP_POWER, ForceMode.Impulse); // ������ɗ͂�������
            isJump = false;
        }
        // ����
        if (isShot)
        {
            rb.AddForce(-prevMaxInput);
            prevMaxInput = new(0f, 0f, 0f);
            isShot = false;
            isableShot = false;
        }
        rb.AddForce(Vector3.down * 20f, ForceMode.Acceleration);
    }


    // �X�e�B�b�N����֐�
    void Shot()
    {
        if (!isableShot)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal") * BALL_SPEED_SCALE; // A/D, ��/��, �X�e�B�b�NX
        float v = Input.GetAxis("Vertical") * BALL_SPEED_SCALE;   // W/S, ��/��, �X�e�B�b�NY

        Vector3 input = new(h, 0f, v);

        float elapsed = Time.time - updateStickPosTime;

        // �ł����l�����邩�A���Ԃ��������玞�Ԃƒl���X�V����
        if (input.magnitude > prevMaxInput.magnitude || elapsed > PREVIVENT_STICKPOS_UPDATE_TIME)
        {
            prevMaxInput = input;
            updateStickPosTime = Time.time;
        }

        // �������u�Ԃ̏���
        if (prevInput.magnitude > 0f &&
            input.magnitude == 0f &&
            prevMaxInput.magnitude > 0.2f * BALL_SPEED_SCALE)
        {
            //�X�e�B�b�N�����ɓ|����Ă��鎞�������˂ł���
            if (prevInput.z < 0f)
            {
                isShot = true;
                isableShot = false;
            }
        }

        prevInput = input;
    }

    void Jump()
    {
        if (!isableJump)
        {
            return;
        }
        // Y�ŃW�����v
        if (Input.GetKeyDown("joystick button 3"))
        {
            isJump = true;
            isableJump = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        isableJump = true;
    }
}
