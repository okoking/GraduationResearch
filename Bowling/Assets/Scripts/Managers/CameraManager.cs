using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public bool IsReady { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        //if (cameras.ContainsKey(CameraMode.Player))
        //{
        //    cameras[CameraMode.Player].gameObject.SetActive(true);
        //    IsReady = true;
        //}
    }

    void Start()
    {

    }

    void Update()
    {
       
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
        cameras[mode].gameObject.SetActive(false);

        // 必要なカメラが全部揃ったら Ready
        if (cameras.ContainsKey(CameraMode.Player))
        {
            IsReady = true;
        }
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
        // 破棄されたカメラを除去
        List<CameraMode> removeKeys = new List<CameraMode>();

        foreach (var pair in cameras)
        {
            if (pair.Value == null)
            {
                removeKeys.Add(pair.Key);
                continue;
            }

            pair.Value.gameObject.SetActive(false);
        }

        foreach (var key in removeKeys)
            cameras.Remove(key);

        if (!cameras.ContainsKey(mode) || cameras[mode] == null)
        {
            Debug.LogError("指定カメラは既に破棄されています");
            return;
        }

        currentMode = mode;
        cameras[mode].gameObject.SetActive(true);
        Debug.Log($"カメラ切替: {mode}");
        ////全OFF
        //foreach (var cam in cameras.Values)
        //    cam.gameObject.SetActive(false);

        //currentMode = mode;
        //cameras[mode].gameObject.SetActive(true);
        //Debug.Log($"カメラ切替: {mode}");
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
        if (!cameras.TryGetValue(CameraMode.Player, out var playerCam))
        {
            Debug.LogError("Ivent / Player カメラが Register されていません");
            yield break;
        }

        currentMode = CameraMode.Player;
      
        //PlayerCamera に切替
        SwitchCamera(CameraMode.Player);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
