using UnityEngine;

//カメラモード
public enum CameraMode
{
    Select,     //ボール選択UI
    FreeLook,   //コース確認
    Play,       //投球時
    Replay      //将来用
}

//カメラマネージャー
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    private Camera selectCamera;
    private Camera freeLookCamera;
    private Camera playerCamera;

    private CameraMode currentMode;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
       
    }

    void Update()
    {
        //必要に応じてカメラ切り替えの入力処理など
       
    }

    //動的にシーン内のカメラを探して登録
    public void RegisterCameras()
    {
        selectCamera = GameObject.Find("SelectCamera")?.GetComponent<Camera>();
        freeLookCamera = GameObject.Find("FreeCamera")?.GetComponent<Camera>();
        playerCamera = GameObject.Find("PlayerCamera")?.GetComponent<Camera>();

        if (selectCamera == null) Debug.LogWarning("SelectCamera が見つかりません！");
        if (freeLookCamera == null) Debug.LogWarning("FreeCamera が見つかりません！");
        if (playerCamera == null) Debug.LogWarning("PlayerCamera が見つかりません！");
    }

    public void SwitchCamera(CameraMode mode)
    {
        currentMode = mode;

        //すべてのカメラを無効化
        if (selectCamera != null) selectCamera.gameObject.SetActive(false);
        if (freeLookCamera != null) freeLookCamera.gameObject.SetActive(false);
        if (playerCamera != null) playerCamera.gameObject.SetActive(false);

        //モードに応じて有効化
        switch (mode)
        {
            case CameraMode.Select:
                if (selectCamera != null) selectCamera.gameObject.SetActive(true);
                Debug.Log("セレクトカメラに変更");
                break;
            case CameraMode.FreeLook:
                if (freeLookCamera != null) freeLookCamera.gameObject.SetActive(true);
                Debug.Log("フリーカメラに変更");
                break;
            case CameraMode.Play:
                if (playerCamera != null) playerCamera.gameObject.SetActive(true);
                Debug.Log("プレイカメラに変更");
                break;
        }
    }

    private void OnGUI()
    {
        if (Instance == null) return;

        CameraMode mode = Instance.GetCurrentMode()/*.ToString()*/;
        GUI.color = Color.red;
        
        GUI.Label(new Rect(10, 5, 300, 30), $"現在のカメラモード: {mode}");

        //モードに応じて有効化
        switch (mode)
        {
            case CameraMode.Select:
                GUI.Label(new Rect(10, 20, 300, 30), $"Cキー：FreeCameraへ");
                GUI.Label(new Rect(10, 35, 300, 30), $"Vキー：PlayCameraへ");
                break;
            case CameraMode.FreeLook:
                GUI.Label(new Rect(10, 20, 300, 30), $"Vキー：PlayCameraへ");
                GUI.Label(new Rect(10, 35, 300, 30), $"Qキー：上昇");
                GUI.Label(new Rect(10, 50, 300, 30), $"Eキー：下降");
                break;
            case CameraMode.Play:
                GUI.Label(new Rect(10, 20, 300, 30), $"Cキー：FreeCameraへ");
                break;
        }
    }

    //カメラモード取得関数
    public CameraMode GetCurrentMode() => currentMode;
}
