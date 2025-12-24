using System.Collections;
using UnityEngine;

//カメラモード
public enum CameraMode
{
    Ivent,          //コース確認
    Play,           //プレイ
    Enemy,          //エネミー
    Boss,           //ボス

    //Replay        //将来用
}

//カメラマネージャー
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    private Camera IventCamera;
    private Camera playerCamera;
    private Camera EnemyCamera;
    private Camera BossCamera;

    private CameraMode currentMode;

    private Coroutine cameraRoutine;

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
        RegisterCameras();
        //SwitchCamera(CameraMode.Play);
    }

    //動的にシーン内のカメラを探して登録
    public void RegisterCameras()
    {
        IventCamera = GameObject.Find("IventCamera")?.GetComponent<Camera>();
        playerCamera = GameObject.Find("PlayerCamera")?.GetComponent<Camera>();
        //EnemyCamera = GameObject.Find("EnemyCamera")?.GetComponent<Camera>();
        //BossCamera = GameObject.Find("Main Camera")?.GetComponent<Camera>();
        if (IventCamera == null) Debug.LogError("IventCamera が見つかりません！");
        if (playerCamera == null) Debug.LogError("PlayerCamera が見つかりません！");
        //if (EnemyCamera == null) Debug.LogError("EnemyCamera が見つかりません！");
        //if (BossCamera == null) Debug.LogError("BossCamera が見つかりません！");

        ////すべてのカメラを無効化
        //if (IventCamera != null) IventCamera.gameObject.SetActive(false);
        //if (playerCamera != null) playerCamera.gameObject.SetActive(false);
        //if (EnemyCamera != null) EnemyCamera.gameObject.SetActive(false);
        //if (BossCamera != null) BossCamera.gameObject.SetActive(false);
    }

    public void SwitchCamera(CameraMode mode)
    {
        currentMode = mode;

        //モードに応じて有効化
        switch (mode)
        {
            case CameraMode.Ivent:
                if (IventCamera != null) IventCamera.gameObject.SetActive(true);
                Debug.Log("イベントカメラに変更");
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
            case CameraMode.Ivent:
                GUI.Label(new Rect(10, 20, 300, 30), $"Vキー：PlayCameraへ");
                GUI.Label(new Rect(10, 35, 300, 30), $"Qキー：上昇");
                GUI.Label(new Rect(10, 50, 300, 30), $"Eキー：下降");
                break;
            case CameraMode.Play:
                GUI.Label(new Rect(10, 20, 300, 30), $"Cキー：FreeCameraへ");
                GUI.Label(new Rect(10, 35, 300, 30), $"Spaceキー：MissileIventCameraへ");
                break;
        }
    }

    //カメラ演出
    public void PlayStartCameraSequence(float duration)
    {
        if (cameraRoutine != null)
            StopCoroutine(cameraRoutine);

        cameraRoutine = StartCoroutine(StartCameraSequence(duration));
    }

    IEnumerator StartCameraSequence(float duration)
    {
        SwitchCamera(CameraMode.Ivent);   //演出カメラ
        yield return new WaitForSecondsRealtime(duration);
        SwitchCamera(CameraMode.Play);       //通常カメラ
    }

    public void PlayZoomInToPlayer(float duration)
    {
        if (cameraRoutine != null)
            StopCoroutine(cameraRoutine);

        cameraRoutine = StartCoroutine(ZoomInToPlayerCoroutine(duration));
    }

    IEnumerator ZoomInToPlayerCoroutine(float duration)
    {
        // 演出用カメラに切替
        SwitchCamera(CameraMode.Ivent);

        Transform fromCam = IventCamera.transform;
        Transform toCam = playerCamera.transform;

        Vector3 startPos = fromCam.position;
        Quaternion startRot = fromCam.rotation;

        Vector3 targetPos = toCam.position;
        Quaternion targetRot = toCam.rotation;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;

            // 位置・回転を補間
            fromCam.position = Vector3.Lerp(startPos, targetPos, EaseOut(t));
            fromCam.rotation = Quaternion.Slerp(startRot, targetRot, EaseOut(t));

            yield return null;
        }

        // 最後にプレイカメラへ
        SwitchCamera(CameraMode.Play);
    }
    //カメラモード取得関数
    public CameraMode GetCurrentMode() => currentMode;

    float EaseOut(float t)
{
    return 1f - Mathf.Pow(1f - t, 3f);
}
}
