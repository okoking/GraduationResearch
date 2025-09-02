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
        // ���͂��擾�i�L�[�{�[�h�܂��̓X�e�B�b�N�j
        float h = Input.GetAxis("Horizontal"); // A/D, ��/��, �X�e�B�b�NX
        float v = Input.GetAxis("Vertical");   // W/S, ��/��, �X�e�B�b�NY

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

        // �͂������ċ��𓮂���
        //rb.AddForce(move * moveSpeed);







        //Vector3 move = new Vector3(h, 0, v);

        //// �͂������ċ��𓮂���
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
