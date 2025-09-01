using UnityEngine;

public class CubeRotation : MonoBehaviour
{
    public Vector3 m_RotationSpeed = new Vector3(0, 0, 0); // 1•bŠÔ‚Ì‰ñ“]Šp“x

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(m_RotationSpeed * Time.deltaTime);
    }
}
