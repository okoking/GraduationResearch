using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Rigidbody rb;

    private Vector3 minSpeed = new(0, 0, 0.5f);
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // 入力を取得（キーボードまたはスティック）
        float h = Input.GetAxis("Horizontal"); // A/D, ←/→, スティックX
        float v = Input.GetAxis("Vertical");   // W/S, ↑/↓, スティックY

        Vector3 move = minSpeed;
        move.z += v;
        move.x += h * 3f;

        if (v < 0f)
        {
            if(rb.linearVelocity.z > 5f)
            {
                //rb.linearVelocity = new(rb.linearVelocity.x, rb.linearVelocity.y, v);
            }
        }

        move.z += v;

        rb.linearVelocity = new (rb.linearVelocity.x, rb.linearVelocity.y, 5f);

        // 力を加えて球を動かす
        //rb.AddForce(move * moveSpeed);







        //Vector3 move = new Vector3(h, 0, v);

        //// 力を加えて球を動かす
        //rb.AddForce(move * moveSpeed);
    }

    void Update()
    {
        //float hori = Input.GetAxis("Horizontal");
        //float vert = Input.GetAxis("Vertical");
        //if ((hori != 0) || (vert != 0))
        //{
        //    m_RotationSpeed.x = vert * ROTA_SCALE;
        //    m_RotationSpeed.y = -hori * ROTA_SCALE;
        //    transform.Rotate(m_RotationSpeed * Time.deltaTime);
        //}
    }
}
