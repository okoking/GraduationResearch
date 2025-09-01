using UnityEngine;

public class CubeRotation : MonoBehaviour
{
    public Vector3 m_RotationSpeed = new Vector3(0, 0, 0); // 1•bŠÔ‚Ì‰ñ“]Šp“x

    const float ROTA_SCALE = 1000.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        if ((hori != 0) || (vert != 0))
        {
            m_RotationSpeed.x = vert * ROTA_SCALE;
            m_RotationSpeed.y = -hori * ROTA_SCALE;
            transform.Rotate(m_RotationSpeed * Time.deltaTime);
        }
    }
}
