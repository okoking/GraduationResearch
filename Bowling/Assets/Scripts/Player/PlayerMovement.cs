using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 8f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpForce = 7f;          // ジャンプ力
    [SerializeField] private Transform cameraTransform;

    private Rigidbody rb;
    private Vector3 currentVelocity;
    private bool isGrounded;
    private BeamCamera beamCamera;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        beamCamera = GetComponent<BeamCamera>();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        // ジャンプ入力（Yボタン）
        if (Input.GetKeyDown(KeyCode.JoystickButton3) && isGrounded)
        {
            Jump();
        }
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(h, 0, v).normalized;
        // ★ 動く前にRigidbodyをWakeUpさせる
        if (inputDir.magnitude > 0)
        {
            rb.WakeUp();
        }

        if (inputDir.magnitude > 0)
        {
            // カメラ基準で方向変換
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = (camForward * v + camRight * h).normalized;

            Vector3 targetVelocity = moveDir * moveSpeed;
            currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

            // キャラ回転(ビーム打つときは変えないようにする)
            if (!beamCamera.isSootBeam)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
            }
        }
        else
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
        }

        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // 接地判定（地面タグを使うなら "Ground" に変更）
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }
}
