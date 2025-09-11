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

    // ジャンプ押したか
    private bool isJump = false;
    // 発射するか
    private bool isShot = false;

    // ジャンプできるか
    private bool isableJump = false;
    // 発射できるか
    private bool isableShot = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isableJump = true;
        isableShot = true;
    }

    void Update()
    {
        // ジャンプ
        Jump();
        // 発射
        Shot();
        // 移動
        Move();
    }
    void FixedUpdate()
    {    
        // 通常の重力に追加で下向きの力をかける
        if (isJump)
        {
            rb.AddForce(Vector3.up * JUMP_POWER, ForceMode.Impulse); // 上方向に力を加える
            isJump = false;
        }
        // 発射
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
            // 力を加えて球を動かす
            rb.AddForce(HorizontalballSpeed);
        }

        rb.AddForce(Vector3.down * 20f, ForceMode.Acceleration);
    }


    // スティック操作関数
    void Shot()
    {

        float h = Input.GetAxis("Horizontal"); // A/D, ←/→, スティックX
        float v = Input.GetAxis("Vertical");   // W/S, ↑/↓, スティックY
        InputPower = new(h, 0f, v);

        // Xでリセット
        if (Input.GetKeyDown("joystick button 2"))
        {
            if (isableShot) // 発射準備
            {
                h = Mathf.Abs(h);
                if (v < 0f && h < .5f)
                {
                    isShot = true;
                    isableShot = false;
                }
            }
            else           // リセット
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

        //// でかい値が入るか、時間がたったら時間と値を更新する
        //if (input.magnitude > prevMaxInput.magnitude || elapsed > PREVIVENT_STICKPOS_UPDATE_TIME)
        //{
        //    prevMaxInput = input;
        //    updateStickPosTime = Time.time;
        //    Debug.Log(prevMaxInput);
        //}

        //// 離した瞬間の処理
        //if (prevInput.magnitude > 0f &&
        //    input.magnitude == 0f &&
        //    prevMaxInput.magnitude > 0.2f * BALL_SPEED_SCALE)
        //{
        //    //スティックが下に倒されている時だけ発射できる
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

        // 入力を取得（キーボードまたはスティック）
        float h = Input.GetAxis("Horizontal") * HoriSpeed; // A/D, ←/→, スティックX

        HorizontalballSpeed = new Vector3(h, 0f, 0f);
    }

    void Jump()
    {
        if (!isableJump)
        {
            return;
        }
        // Yでジャンプ
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
