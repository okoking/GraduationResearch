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
    private bool isGrounded;
    private bool wasGrounded;

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

        isGrounded = CheckGrounded(out bool onSteepSlope, out wallNormal);


        if (!wasGrounded && isGrounded)
        {
            velocity = Vector3.zero;
        }

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

        // --- 接地判定 ---
        if (controller.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        // --- ジャンプ ---
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        if (onSteepSlope && !isGrounded)
        {
            // 壁の法線に沿った下方向へ滑らせる
            Vector3 slideDir = Vector3.ProjectOnPlane(Vector3.down, wallNormal).normalized;
            velocity = slideDir * 6f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        
        // --- 移動適用（慣性を使う！） ---
        controller.Move(currentMove * moveSpeed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            Debug.Log("落下");
            transform.position = Vector3.zero;
        }

        wasGrounded = isGrounded;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 地面（normal.y が大きい）
        if (hit.normal.y > 0.5f)
        {
            // 必要なら接地処理
            //velocity.y = 0;  // ※Moveでやるならここは不要
            return;
        }

        // 天井（normal.y が負）
        if (hit.normal.y < -0.5f)
        {
            // 上昇中なら強制停止
            if (velocity.y > 0)
            {
                velocity.y = 0f;
                Debug.Log("天井に衝突 → y停止");
            }
            return;
        }

        //// 壁（normal.y がほぼ 0）
        //if (Mathf.Abs(hit.normal.y) < 0.1f)
        //{
        //    // 壁方向にめり込む速度を打ち消す
        //    Vector3 moveDir = velocity;     // 現在の移動方向
        //    Vector3 normal = hit.normal;    // 壁の法線

        //    // 移動方向を「壁に投影」
        //    Vector3 horizontalSlide = Vector3.ProjectOnPlane(moveDir, normal);

        //    // 壁に向かう成分を消す
        //    velocity.x = horizontalSlide.x;
        //    velocity.z = horizontalSlide.z;

        //    return;
        //}
    }

    bool CheckGrounded(out bool onSteepSlope, out Vector3 hitNormal)
    {
        float rayLength = controller.height / 2 + 0.1f;
        onSteepSlope = false;
        hitNormal = Vector3.up;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, rayLength))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            hitNormal = hit.normal;

            if (slopeAngle <= controller.slopeLimit)
            {
                return true; // 普通の接地
            }
            else
            {
                onSteepSlope = true; // 急斜面に触れている
                return false;       // 接地判定はなし
            }
        }
        return false;
    }
}
