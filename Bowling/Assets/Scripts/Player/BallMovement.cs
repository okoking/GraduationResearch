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

    // ジャンプ押したか
    private bool isJump = false;
    //private bool eventStarted = false;

    // この値秒間のうちのスティック最大座標を保存する
    const float PREVIVENT_STICKPOS_UPDATE_TIME = .5f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        updateStickPosTime = Time.time;
    }

    void Update()
    {
        // ジャンプ
        Jump();
        // Xでリセット
        if (Input.GetKeyDown("joystick button 2"))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.position = new(0f, 0.5f, -5f);
        }
    }
    void FixedUpdate()
    {    
        // スティック操作
        StickOperation();
        // 通常の重力に追加で下向きの力をかける
        if (isJump)
        {
            rb.AddForce(Vector3.up * JUMP_POWER, ForceMode.Impulse); // 上方向に力を加える        }
            isJump = false;
        }
        rb.AddForce(Vector3.down * 20f, ForceMode.Acceleration);
    }


    // スティック操作関数
    void StickOperation()
    {
        float h = Input.GetAxis("Horizontal"); // A/D, ←/→, スティックX
        float v = Input.GetAxis("Vertical");   // W/S, ↑/↓, スティックY

        Vector3 input = new(h, 0f, v);

        float elapsed = Time.time - updateStickPosTime;

        // でかい値が入るか、時間がたったら時間と値を更新する
        if (input.magnitude > prevMaxInput.magnitude || elapsed > PREVIVENT_STICKPOS_UPDATE_TIME)
        {
            prevMaxInput = input;
            updateStickPosTime = Time.time;
        }

        // 離した瞬間の処理（
        if (prevInput.magnitude > 0f &&
            input.magnitude == 0f &&
            prevMaxInput.magnitude > 0.2f)
        {
            //スティックが下に倒されている時だけ発射できる
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
        // Yでジャンプ
        if (Input.GetKeyDown("joystick button 3"))
        {
            isJump = true;
        }
    }
}
