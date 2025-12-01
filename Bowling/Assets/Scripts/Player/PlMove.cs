using UnityEngine;

public class PlMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 2f; 
    public float gravity = -9.81f;
    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // --- 移動 ---
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;

        Debug.Log(controller.isGrounded);

        // --- 接地判定 ---
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // 地面に吸い付けておく（0にすると浮きやすい）
        }

        // --- ジャンプ ---
        if (Input.GetKeyDown(KeyCode.JoystickButton3) && controller.isGrounded)
        {
            // v = √(2gh) で計算
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // --- 重力 ---
        velocity.y += gravity * Time.deltaTime;

        // --- 全体適用 ---
        controller.Move(move * moveSpeed * Time.deltaTime);

        controller.Move(velocity * Time.deltaTime);
    }
}
