using UnityEngine;
using System.Collections;
using TMPro;
using Unity.Cinemachine;

public class GameStartDirector : MonoBehaviour
{
    //[Header("Camera")]
    //[SerializeField] CinemachineVirtualCamera introCam;
    //[SerializeField] CinemachineVirtualCamera playCam;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] TextMeshProUGUI goText;

    [Header("Timing")]
    [SerializeField] float cameraDuration = 2.0f;
    [SerializeField] float countInterval = 1.0f;

    [SerializeField] CameraRail startRail;
    [SerializeField] float railDuration = 3f;
    [SerializeField] Transform lookTarget;

    public static bool IsGameStarted { get; private set; }

    void Start()
    {
        countdownText.gameObject.SetActive(false);
        goText.gameObject.SetActive(false);
        StartCoroutine(GameStartSequence());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            CameraManager.Instance.PlayMoveFromIventToPlayer(cameraDuration);
        }
    }

    IEnumerator GameStartSequence()
    {
        //CameraManager の初期化待ち
        yield return new WaitUntil(() =>
       CameraManager.Instance != null && CameraManager.Instance.IsReady);

        //ロック
        IsGameStarted = false;

        yield return new WaitForSecondsRealtime(2.0f);

        //CameraManager.Instance.PlayRailWithFade(startRail, lookTarget, railDuration,
        //    1f);
        //CameraManager.Instance.PlayRail(startRail, lookTarget, railDuration,
        //    CameraLookMode.LookTarget);
        //yield return new WaitForSecondsRealtime(railDuration);

        //カメラ演出
        CameraManager.Instance.PlayMoveFromIventToPlayer(cameraDuration);
        yield return new WaitForSecondsRealtime(cameraDuration);

        yield return new WaitForSecondsRealtime(2.0f);

        //カウントダウン
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(countInterval);
        }

        countdownText.gameObject.SetActive(false);

        //GO!
        goText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        goText.gameObject.SetActive(false);

        //解放
        IsGameStarted = true;
    }
}
