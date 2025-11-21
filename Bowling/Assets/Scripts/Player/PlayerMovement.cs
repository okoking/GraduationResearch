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
        bool isGround = CheckGround();
        if (Input.GetKeyDown(KeyCode.JoystickButton3) && isGround)
        {
            Jump();
        }

        if (transform.position.y < -5f)
        {
            transform.position = new(0, 0, 0);
        }
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(h, 0, v).normalized;

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

            // キャラ回転(ビーム打つときは変えない)
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

    bool CheckGround()
    {
        float rayLength = 0.1f;
        return Physics.Raycast(transform.position + Vector3.forward * 0.1f, Vector3.down, rayLength) ||
               Physics.Raycast(transform.position - Vector3.forward * 0.1f, Vector3.down, rayLength) ||
               Physics.Raycast(transform.position, Vector3.down, rayLength);
    }

    void Jump()
    {
        Vector3 v = rb.linearVelocity;
        v.y = 0;
        rb.linearVelocity = v;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision col)
    {
        //Vector3 n = col.contacts[0].normal;

        //n.y = 0;

        //rb.AddForce(n * 3f, ForceMode.Impulse);
    }
}
