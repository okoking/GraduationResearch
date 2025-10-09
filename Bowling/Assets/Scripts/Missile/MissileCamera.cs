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
            if (CameraManager.Instance.GetCurrentMode() != CameraMode.MissileEvent)
            {
                CameraManager.Instance.SwitchCamera(CameraMode.MissileEvent);
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
