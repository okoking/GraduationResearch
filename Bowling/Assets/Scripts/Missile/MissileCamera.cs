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
            //切り替え
            if (CameraManager.Instance.GetCurrentMode() != CameraMode.MissileIvent)
            {
                CameraManager.Instance.SwitchCamera(CameraMode.MissileIvent);
                Debug.Log("ミサイルイベントカメラへ変更");
            }
            else
            {
                CameraManager.Instance.SwitchCamera(CameraMode.Play);
                Debug.Log("プレイカメラへ変更");
            }
        }
    }
}
