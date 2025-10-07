using UnityEngine;

public class MissileCamera : MonoBehaviour
{
    [SerializeField]Camera MissileIventCamera;
    CameraMode currentCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        currentCamera = CameraManager.Instance.GetCurrentMode();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //切り替え
            if (currentCamera != CameraMode.MissileIvent)
            {
                CameraManager.Instance.SwitchCamera(CameraMode.MissileIvent);
                Debug.Log("ミサイルイベントカメラへ変更");
                currentCamera = CameraMode.MissileIvent;
            }
            else
            {
                CameraManager.Instance.SwitchCamera(CameraMode.Play);
                Debug.Log("プレイカメラへ変更");
                currentCamera = CameraMode.Play;
            }
        }
    }
}
