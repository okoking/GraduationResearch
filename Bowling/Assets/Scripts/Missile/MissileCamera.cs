using UnityEngine;

public class MissileCamera : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //�؂�ւ�
            if (CameraManager.Instance.GetCurrentMode() != CameraMode.MissileIvent)
            {
                CameraManager.Instance.SwitchCamera(CameraMode.MissileIvent);
                Debug.Log("�~�T�C���C�x���g�J�����֕ύX");
            }
            else
            {
                CameraManager.Instance.SwitchCamera(CameraMode.Play);
                Debug.Log("�v���C�J�����֕ύX");
            }
        }
    }
}
