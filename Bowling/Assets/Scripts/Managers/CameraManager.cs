using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

//カメラモード
public enum CameraMode
{
    Ivent,          //イベント
    Player,         //プレイ
    PlayUI,

    //Replay        //将来用
}

//カメラマネージャー
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    private CameraMode currentMode;

    private Coroutine cameraRoutine;

    private Dictionary<CameraMode, CinemachineCamera> cameras = new();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            cameras[CameraMode.Player].gameObject.SetActive(false);
            cameras[CameraMode.Ivent].gameObject.SetActive(true);
        }
    }

    //動的にシーン内のカメラを探して登録
    public void Register(CameraMode mode, CinemachineCamera cam)
    {
        if (cameras.ContainsKey(mode))
        {
            Debug.LogWarning($"CameraMode {mode} は既に登録されています");
            return;
        }

        cameras[mode] = cam;
        Debug.Log($"{cam} を登録しました");
        cameras[mode].Priority = 10;
    }

    //カメラモード取得関数
    public CameraMode GetCurrentMode() => currentMode;

    public void SwitchCamera(CameraMode mode)
    {
        if (!cameras.ContainsKey(mode))
        {
            Debug.LogError($"CameraMode {mode} のカメラが未登録です");
            return;
        }

        //全OFF
        foreach (var cam in cameras.Values)
            cam.Priority = 10;

        currentMode = mode;
        cameras[mode].Priority = 20;
        Debug.Log($"カメラ切替: {mode}");
    }

    //カメラ演出
    public void PlayMoveFromIventToPlayer(float duration)
    {
        if (cameraRoutine != null)
            StopCoroutine(cameraRoutine);

        cameraRoutine = StartCoroutine(MoveFromIventToPlayer(duration));
    }

    //IEnumerator ZoomSequence(float duration)
    //{
    //    SwitchCamera(CameraMode.Ivent);
    //    yield return new WaitForSecondsRealtime(duration);
    //    SwitchCamera(CameraMode.Player);
    //}

   

    IEnumerator MoveFromIventToPlayer(float duration)
    {
        //Eventカメラを表示
        SwitchCamera(CameraMode.Ivent);
        yield return null; // Priority反映待ち（重要）

        var iventCam = cameras[CameraMode.Ivent];
        var playerCam = cameras[CameraMode.Player];

        //Follow取得
        var iventFollow = iventCam.GetComponent<CinemachineFollow>();
        var playerFollow = playerCam.GetComponent<CinemachineFollow>();

        if (iventFollow == null)
        {
            Debug.LogError("IventCamera に CinemachineFollow が付いていません");
            yield break;
        }

        if (playerFollow == null)
        {
            Debug.LogError("PlayerCamera に CinemachineFollow が付いていません");
            yield break;
        }

        Vector3 startOffset = iventFollow.FollowOffset;
        Vector3 targetOffset = playerFollow.FollowOffset;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;
            float eased = EaseOut(t);

            iventFollow.FollowOffset =
                Vector3.Lerp(startOffset, targetOffset, eased);

            yield return null;
        }

        //完了後 Player カメラへ
        SwitchCamera(CameraMode.Player);
        cameraRoutine = null;
    }

    float EaseOut(float t) { return 1f - Mathf.Pow(1f - t, 3f); }
}
