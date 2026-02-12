using System;
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
public enum CameraLookMode
{
    RailRotation,   // レールポイントの回転を使う
    LookTarget,     // 特定ターゲットを見る
    FreeRotation,   // 外部から指定した回転
    BlendToPlayer   // 最後にPlayerへ寄せる
}

//カメラマネージャー
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    private CameraMode currentMode;

    private Coroutine cameraRoutine;

    private Dictionary<CameraMode, CinemachineCamera> cameras = new();

    public bool IsReady { get; private set; }

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

        //fader = ScreenFader.Find("Fader");
    }

    void Start()
    {
        //cameras[CameraMode.Ivent].gameObject.SetActive(true);
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{

        //    cameras[CameraMode.Ivent].gameObject.SetActive(false);
        //    cameras[CameraMode.Player].gameObject.SetActive(true);


        //}
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
        

        //初回登録時に切り替え
        if (mode == CameraMode.Ivent)
        {
            cameras[mode].gameObject.SetActive(true);
            currentMode = mode;
        }

        // 必要なカメラが全部揃ったら Ready
        if (/*cameras.ContainsKey(CameraMode.Ivent) &&*/
            cameras.ContainsKey(CameraMode.Player))
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

        //全OFF
        foreach (var cam in cameras.Values)
            cam.gameObject.SetActive(false);

        currentMode = mode;
        cameras[mode].gameObject.SetActive(true);
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
        if (/*!cameras.TryGetValue(CameraMode.Ivent, out var iventCam) ||*/
       !cameras.TryGetValue(CameraMode.Player, out var playerCam))
        {
            Debug.LogError("Ivent / Player カメラが Register されていません");
            yield break;
        }

        //IventCamera を有効化


        currentMode = CameraMode.Player;
        //Transform from = iventCam.transform;
        //Transform to = playerCam.transform;

        //Vector3 startPos = from.position;
        //Quaternion startRot = from.rotation;

        //Vector3 endPos = to.position;
        //Quaternion endRot = to.rotation;

        //float t = 0f;

        //while (t < 1f)
        //{
        //    t += Time.deltaTime / duration;
        //    float eased = EaseOut(t);

        //    from.position = Vector3.Lerp(startPos, endPos, eased);
        //    from.rotation = Quaternion.Slerp(startRot, endRot, eased);

        //    yield return null;
        //}

        ////念のため最終一致
        //from.position = endPos;
        //from.rotation = endRot;

        ////// ★ PlayerCam も同じ Transform にする
        ////playerCam.transform.position = from.position;
        ////playerCam.transform.rotation = from.rotation;

        //PlayerCamera に切替
        SwitchCamera(CameraMode.Player);
    }
    public void PlayRail(CameraRail rail, Transform lookTarget, float duration,
    CameraLookMode lookMode, Quaternion? freeRotation = null)
    {
        if (cameraRoutine != null)
            StopCoroutine(cameraRoutine);

        cameraRoutine = StartCoroutine(RailSequence(rail, lookTarget, duration,
            lookMode, freeRotation));
    }
    public void PlayRailWithFade(CameraRail rail, Transform lookTarget, float duration,
    float fadeTime)
    {
        if (cameraRoutine != null)
            StopCoroutine(cameraRoutine);

        cameraRoutine = StartCoroutine(RailSequenceWithFade(rail, lookTarget,
    duration, fadeTime));
    }

    IEnumerator RailSequence(CameraRail rail, Transform lookTarget, float duration,
    CameraLookMode lookMode, Quaternion? freeRotation = null)
    {
        if (!cameras.TryGetValue(CameraMode.Ivent, out var iventCam))
        {
            Debug.LogError("IventCamera 未登録");
            yield break;
        }

        //IventCamera を使う
        SwitchCamera(CameraMode.Ivent);

        float totalSegments = rail.Count - 1;
        float segmentTime = duration / totalSegments;

        for (int i = 0; i < rail.Count - 1; i++)
        {
            Transform from = rail.GetPoint(i);
            Transform to = rail.GetPoint(i + 1);

            if (i == 0)
            {
                ScreenFader.Instance.PlayFadeIn(1.0f);
                ScreenFader.Instance.PlayFadeOut(1.0f);
            }

            float t = 0f;
            while (t < 1f)
            {
                

                t += Time.deltaTime / segmentTime;
                float eased = EaseOut(t);

                //位置
                iventCam.transform.position =
                    Vector3.Lerp(from.position, to.position, eased);

                //向き
                Quaternion targetRot = iventCam.transform.rotation;

                switch (lookMode)
                {
                    case CameraLookMode.RailRotation:
                        targetRot =
                            Quaternion.Slerp(from.rotation, to.rotation, eased);
                        break;

                    case CameraLookMode.LookTarget:
                        Vector3 dir =
                            lookTarget.position - iventCam.transform.position;
                        targetRot = Quaternion.LookRotation(dir, Vector3.up);
                        break;

                    case CameraLookMode.FreeRotation:
                        if (freeRotation.HasValue)
                            targetRot = freeRotation.Value;
                        break;
                }

                iventCam.transform.rotation =
                    Quaternion.Slerp(
                        iventCam.transform.rotation,
                        targetRot,
                        10f * Time.deltaTime
                    );



                yield return null;
            }

            

        }
    }
    IEnumerator RailSequenceWithFade(
    CameraRail rail,
    Transform lookTarget,
    float duration,
    float fadeTime
)
    {
        if (!cameras.TryGetValue(CameraMode.Ivent, out var cam))
            yield break;

        SwitchCamera(CameraMode.Ivent);

        float moveTime = duration / (rail.Count - 1);

        for (int i = 0; i < rail.Count - 1; i++)
        {
            // フェードアウトを「並列」で開始
            Coroutine fadeOut = null;
            if (i < rail.Count - 2)
            {
                fadeOut = ScreenFader.Instance.PlayFadeOut(fadeTime);
            }

            // 補間移動
            yield return MoveBetweenPoints(
                cam,
                rail.GetPoint(i),
                rail.GetPoint(i + 1),
                lookTarget,
                moveTime
            );

            // フェードアウトが終わるのを待つ
            if (fadeOut != null)
                yield return fadeOut;

            // フェードイン
            if (i < rail.Count - 2)
                yield return ScreenFader.Instance.PlayFadeIn(fadeTime);
        }
    }
    IEnumerator MoveBetweenPoints(
    CinemachineCamera cam,
    Transform from,
    Transform to,
    Transform lookTarget,
    float time
)
    {
        float t = 0f;

        Vector3 startPos = from.position;
        Quaternion startRot = from.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime / time;
            float eased = EaseOut(t);

            cam.transform.position =
                Vector3.Lerp(startPos, to.position, eased);

            if (lookTarget != null)
            {
                Vector3 dir = lookTarget.position - cam.transform.position;
                cam.transform.rotation =
                    Quaternion.LookRotation(dir, Vector3.up);
            }
            else
            {
                cam.transform.rotation =
                    Quaternion.Slerp(startRot, to.rotation, eased);
            }

            yield return null;
        }

        cam.transform.SetPositionAndRotation(
            to.position, to.rotation
        );
    }
    float EaseOut(float t) { return 1f - Mathf.Pow(1f - t, 3f); }
}
