using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody rb;

    public float JUMP_POWER = 1000f;
    public float BALL_SPEED_SCALE = 1000f;

    private Vector3 m_BallSpeed = new(0f, 0f, 0f);
    private Vector3 prevInput = new(0f, 0f, 0f);
    private Vector3 prevMaxInput = new(0f, 0f, 0f);

    private float eventStartTime;
    private float updateStickPosTime;

    // �W�����v��������
    private bool isJump = false;
    //private bool eventStarted = false;

    // ���̒l�b�Ԃ̂����̃X�e�B�b�N�ő���W��ۑ�����
    const float PREVIVENT_STICKPOS_UPDATE_TIME = .5f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        updateStickPosTime = Time.time;
    }

    void Update()
    {
        // �W�����v
        Jump();
        // X�Ń��Z�b�g
        if (Input.GetKeyDown("joystick button 2"))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.position = new(0f, 0.5f, -5f);
        }
    }
    void FixedUpdate()
    {    
        // �X�e�B�b�N����
        StickOperation();
        // �ʏ�̏d�͂ɒǉ��ŉ������̗͂�������
        if (isJump)
        {
            rb.AddForce(Vector3.up * JUMP_POWER, ForceMode.Impulse); // ������ɗ͂�������        }
            isJump = false;
        }
        rb.AddForce(Vector3.down * 20f, ForceMode.Acceleration);
    }


    // �X�e�B�b�N����֐�
    void StickOperation()
    {
        float h = Input.GetAxis("Horizontal"); // A/D, ��/��, �X�e�B�b�NX
        float v = Input.GetAxis("Vertical");   // W/S, ��/��, �X�e�B�b�NY

        Vector3 input = new(h, 0f, v);

        float elapsed = Time.time - updateStickPosTime;

        // �ł����l�����邩�A���Ԃ��������玞�Ԃƒl���X�V����
        if (input.magnitude > prevMaxInput.magnitude || elapsed > PREVIVENT_STICKPOS_UPDATE_TIME)
        {
            prevMaxInput = input;
            updateStickPosTime = Time.time;
        }

        // �������u�Ԃ̏����i
        if (prevInput.magnitude > 0f &&
            input.magnitude == 0f &&
            prevMaxInput.magnitude > 0.2f)
        {
            //�X�e�B�b�N�����ɓ|����Ă��鎞�������˂ł���
            if (prevInput.z < 0f)
            {
                rb.AddForce(-prevMaxInput * BALL_SPEED_SCALE);
                prevMaxInput = new(0f, 0f, 0f);
            }
        }


        prevInput = input;
    }

    void Jump()
    {
        // Y�ŃW�����v
        if (Input.GetKeyDown("joystick button 3"))
        {
            isJump = true;
        }
    }
}
