using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody rb;

    public float JUMP_POWER = 1000f;
    public float BALL_SPEED_SCALE = 1000f;

    public Vector3 InputPower = new(0f, 0f, 0f);

    private Vector3 HorizontalballSpeed = new(0f, 0f, 0f);

    const float DEFAULT_HORI_SPEED = 20f;
    public float HoriSpeed = DEFAULT_HORI_SPEED;

    // �W�����v��������
    private bool isJump = false;
    // ���˂��邩
    private bool isShot = false;

    // �W�����v�ł��邩
    private bool isableJump = false;
    // ���˂ł��邩
    private bool isableShot = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isableJump = true;
        isableShot = true;
    }

    void Update()
    {
        // �W�����v
        Jump();
        // ����
        Shot();
        // �ړ�
        Move();
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
            //rb.AddForce(-prevMaxInput);
            //prevMaxInput = new(0f, 0f, 0f);
            rb.AddForce(-InputPower* BALL_SPEED_SCALE);
            InputPower = new(0f, 0f, 0f);
            isShot = false;
        }

        if (!isableShot)
        {
            // �͂������ċ��𓮂���
            rb.AddForce(HorizontalballSpeed);
        }

        rb.AddForce(Vector3.down * 20f, ForceMode.Acceleration);
    }


    // �X�e�B�b�N����֐�
    void Shot()
    {

        float h = Input.GetAxis("Horizontal"); // A/D, ��/��, �X�e�B�b�NX
        float v = Input.GetAxis("Vertical");   // W/S, ��/��, �X�e�B�b�NY
        InputPower = new(h, 0f, v);

        // X�Ń��Z�b�g
        if (Input.GetKeyDown("joystick button 2"))
        {
            if (isableShot) // ���ˏ���
            {
                h = Mathf.Abs(h);
                if (v < 0f && h < .5f)
                {
                    isShot = true;
                    isableShot = false;
                }
            }
            else           // ���Z�b�g
            {
                isableShot = true;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.position = new(0f, 0.5f, -5f);
            }
        }

        //float hor = Mathf.Abs(Input.GetAxis("Horizontal"));

        //if (hor < .5f)
        //{
        //    Debug.Log(hor);
        //}

        //Vector3 input = new(h, 0f, v);

        //float elapsed = Time.time - updateStickPosTime;

        //// �ł����l�����邩�A���Ԃ��������玞�Ԃƒl���X�V����
        //if (input.magnitude > prevMaxInput.magnitude || elapsed > PREVIVENT_STICKPOS_UPDATE_TIME)
        //{
        //    prevMaxInput = input;
        //    updateStickPosTime = Time.time;
        //    Debug.Log(prevMaxInput);
        //}

        //// �������u�Ԃ̏���
        //if (prevInput.magnitude > 0f &&
        //    input.magnitude == 0f &&
        //    prevMaxInput.magnitude > 0.2f * BALL_SPEED_SCALE)
        //{
        //    //�X�e�B�b�N�����ɓ|����Ă��鎞�������˂ł���
        //    if (prevInput.z < 0f)
        //    {
        //        isShot = true;
        //        isableShot = false;
        //    }
        //}

        //prevInput = input;
    }

    void Move()
    {
        if (isableShot) 
        {
            return;
        }

        // ���͂��擾�i�L�[�{�[�h�܂��̓X�e�B�b�N�j
        float h = Input.GetAxis("Horizontal") * HoriSpeed; // A/D, ��/��, �X�e�B�b�NX

        HorizontalballSpeed = new Vector3(h, 0f, 0f);
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
