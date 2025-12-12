using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private Transform cam;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float acceleration = 10f;    // 加速の速さ
    [SerializeField] private float deceleration = 10f;    // 減速の速さ

    private Vector3 velocity;
    private KariBeam beamInfo;
    private CharacterController controller;
    private Vector3 currentMove = Vector3.zero; // 慣性付きの移動速度
    private bool isonSteepSlope;
    private bool wasonSteepSlope;

    private bool isGroundEx;

    private Vector3 wallNormal;

    void Start()
    {
        beamInfo = GetComponent<KariBeam>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // --- 移動 ---
        Vector3 camForward = cam.forward;
        camForward.y = 0f;         // 上下成分は消す
        camForward.Normalize();

        Vector3 camRight = cam.right;
        camRight.y = 0f;
        camRight.Normalize();

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveInput = camForward * v + camRight * h;
        moveInput.Normalize();

        CheckGroundedEx(out bool onSteepSlope, out wallNormal);

        isonSteepSlope = onSteepSlope;

        if (!isonSteepSlope && wasonSteepSlope)
        {
            velocity = Vector3.zero;
        }

        wasonSteepSlope = isonSteepSlope;

        // --- 慣性付きの移動ベクトルを計算 ---
        if (moveInput.magnitude > 0.1f)
        {
            // 加速
            currentMove = Vector3.Lerp(
                currentMove,
                moveInput.normalized,
                acceleration * Time.deltaTime
            );
        }
        else
        {
            // 減速（入力が0のときゆっくり止まる）
            currentMove = Vector3.Lerp(
                currentMove,
                Vector3.zero,
                deceleration * Time.deltaTime
            );
        }

        // --- 回転（向き） ---
        if (!beamInfo.disableRotate)
        {
            if (currentMove.magnitude > 0.1f)
            {
                Quaternion targetRot = Quaternion.LookRotation(currentMove);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRot,
                    rotationSpeed * Time.deltaTime
                );
            }
        }

        // --- ジャンプ ---
        if (Input.GetButtonDown("Jump") && isGroundEx)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // --- 接地判定 ---
        if (isGroundEx && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        // --- 壁滑り中かどうか ---
        if (isonSteepSlope && velocity.y <= 0f)
        {
            // 斜面上の滑り方向（重力を地面に投影）
            Vector3 slideDir = Vector3.ProjectOnPlane(Physics.gravity, wallNormal).normalized;

            // 重力の大きさを調整（正の値）
            float slideSpeed = 15f;

            // ユーザー入力の水平移動
            Vector3 horizontal = currentMove * moveSpeed;

            // velocity は "横移動 + 滑り" の合成
            velocity = horizontal + slideDir * slideSpeed;

            // Move 実行
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            // 通常重力計算
            velocity.y += gravity * Time.deltaTime;

            // 通常移動（横 + 慣性）
            Vector3 finalMove = moveSpeed * currentMove + velocity;
            controller.Move(finalMove * Time.deltaTime);
        }

        if (transform.position.y < -5f)
        {
            Debug.Log("落下");
            transform.position = Vector3.zero;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 天井（normal.y が負）
        if (hit.normal.y < -0.5f)
        {
            // 上昇中なら強制停止
            if (velocity.y > 0)
            {
                velocity.y = 0f;
                Debug.Log("天井に衝突 → y停止");
            }
        }
    }

    void CheckGroundedEx(
    out bool onSteepSlope,
    out Vector3 groundNormal
)
    {
        isGroundEx = controller.isGrounded;

        onSteepSlope = false;
        groundNormal = Vector3.up;

        // --- SphereCast パラメータ ---
        float radius = controller.radius * 0.95f;  // 少し小さくして誤検出を防ぐ
        float castDistance = 0.3f;                // 斜面を拾える程度の長さ
        
        // 足元の位置（カプセルの底より少し上）
        Vector3 origin = transform.position + controller.center;
        origin.y -= controller.height / 2 - radius;

        if (Physics.SphereCast(
            origin,
            radius,
            Vector3.down,
            out RaycastHit hit,
            castDistance
        ))
        {
            groundNormal = hit.normal;

            // 角度チェック（CharacterController の slopeLimit と同じ基準）
            float angle = Vector3.Angle(hit.normal, Vector3.up);

            if (angle > controller.slopeLimit)
            {
                onSteepSlope = true;
                isGroundEx = false;
            }
        }
    }
}
