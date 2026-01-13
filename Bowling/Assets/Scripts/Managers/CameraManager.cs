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

    //private Camera PlayUI;
    //private Camera Ivent;
    //private Camera Player;

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
        //PlayUI = GameObject.Find("PlayerUICamera")?.GetComponent<Camera>();
        
        //Ivent = GameObject.Find("IventCamera")?.GetComponent<Camera>();
        //Player = GameObject.Find("PlayerCamera")?.GetComponent<Camera>();

        //PlayUI.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {

            //cameras[CameraMode.Ivent].enabled = false;
            

            
        }
    }

    //動的にシーン内のカメラを探して登録
    //public void Register(CameraMode mode, CinemachineCamera cam)
    //{
    //    if (cameras.ContainsKey(mode))
    //    {
    //        Debug.LogWarning($"CameraMode {mode} は既に登録されています");
    //        return;
    //    }

    //    cameras[mode] = cam;
    //    Debug.Log($"{cam} を登録しました");
    //    //cameras[mode].Priority = 0;

    //    //初回登録時に切り替え
    //    if (cameras.Count == 3)
    //    {
    //        cameras[CameraMode.PlayUI].enabled = false;
    //    }
    //}

    //カメラモード取得関数
    public CameraMode GetCurrentMode() => currentMode;

    public void SwitchCamera(CameraMode mode)
    {
        if (!cameras.ContainsKey(mode))
        {
            Debug.LogError($"CameraMode {mode} のカメラが未登録です");
            return;
        }

        ////全OFF
        //foreach (var cam in cameras.Values)
        //    cam.Priority = 10;

        currentMode = mode;
        //cameras[mode].Priority = 20;
        Debug.Log($"カメラ切替: {mode}");
    }

    //カメラ演出
    public void PlayMoveFromIventToPlayer(float duration)
    {
        if (cameraRoutine != null)
            StopCoroutine(cameraRoutine);

        cameraRoutine = StartCoroutine(MoveFromIventToPlayer(duration));
    }

    IEnumerator MoveFromIventToPlayer(float duration)
    {
        if (!cameras.TryGetValue(CameraMode.Ivent, out var iventCam) ||
       !cameras.TryGetValue(CameraMode.Player, out var playerCam))
        {
            Debug.LogError("Ivent / Player カメラが Register されていません");
            yield break;
        }

        //IventCamera を有効化
        
        iventCam.Priority = 30;
        playerCam.Priority = 10;
        currentMode = CameraMode.Ivent;
        Transform from = iventCam.transform;
        Transform to = playerCam.transform;

        Vector3 startPos = from.position;
        Quaternion startRot = from.rotation;

        Vector3 endPos = to.position;
        Quaternion endRot = to.rotation;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float eased = EaseOut(t);

            from.position = Vector3.Lerp(startPos, endPos, eased);
            from.rotation = Quaternion.Slerp(startRot, endRot, eased);

            yield return null;
        }

        //念のため最終一致
        from.position = endPos;
        from.rotation = endRot;

        //PlayerCamera に切替
        SwitchCamera(CameraMode.Player);
    }

    float EaseOut(float t) { return 1f - Mathf.Pow(1f - t, 3f); }
}
