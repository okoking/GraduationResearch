using UnityEngine;

public class MissileCamera : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera subCamera;

    Camera currentCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �ŏ��̓��C���J������L����
        mainCamera.enabled = true;
        subCamera.enabled = false;
        currentCamera = mainCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // �؂�ւ�
            if (currentCamera == mainCamera)
            {
                mainCamera.enabled = false;
                subCamera.enabled = true;
                currentCamera = subCamera;
            }
            else
            {
                mainCamera.enabled = true;
                subCamera.enabled = false;
                currentCamera = mainCamera;
            }
        }
    }
}
