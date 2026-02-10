using UnityEngine;
using System.Collections;

public class CanvasFader : MonoBehaviour
{
    [Header("開始時表示")][SerializeField] bool InitDrow;
    [SerializeField][Range(0.0f, 1.0f)] float MaxAlpha = 1f;
    [SerializeField][Range(0.0f, 1.0f)] float MinAlpha = 0f;

    CanvasGroup canvasGroup;    //透明度
    Coroutine fadeCoroutine;    //現在の進行度

    //現在演出中フラグ
    public bool m_IsFadeNow { get; private set; }

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (InitDrow) return;

        //開始時は非表示
        FadeOut(MinAlpha);
    }

    // フェードイン：かかる時間
    public void FadeIn(float duration)
    {
        gameObject.SetActive(true);
        StartFade(MinAlpha, MaxAlpha, duration);
    }

    // フェードアウト：かかる時間
    public void FadeOut(float duration)
    {
        gameObject.SetActive(true);
        StartFade(MaxAlpha, MinAlpha, duration);
    }

    //from      ：開始時の透明度。表示 = 1f, 非表示 = 0f
    //to        ：終了時の透明度。表示 = 1f, 非表示 = 0f
    //duration  ：かかる時間(秒)
    void StartFade(float from, float to, float duration)
    {
        //すでにフェード中なら前のフェードを止める
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeCoroutine(from, to, duration));
    }

    //IEnumerator = 処理を途中で止めて、また続きを実行できるもの
    IEnumerator FadeCoroutine(float from, float to, float duration)
    {
        m_IsFadeNow = true;
        //開始時の透明度を反映
        canvasGroup.alpha = from;
        //UIを反応させないようにする
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        //時間の初期化
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            //透明度を加算
            //Mathf.Lerp = A から B までを、割合に応じて滑らかに変化させる関数
            canvasGroup.alpha = Mathf.Lerp(from, to, time / duration);
            //1フレーム待つ
            yield return null;
        }

        //終了時の透明度を反映
        canvasGroup.alpha = to;

        //終了時が非表示にされていたらキャンバスを非表示にする
        if (to == 0f)
            gameObject.SetActive(false);

        //コールチンを動いてない設定にする
        fadeCoroutine = null;
        m_IsFadeNow = false;
    }

    public float GetAlpha()
    {
        return canvasGroup.alpha;
    }

    public float GetMaxAlpha()
    {
        return MaxAlpha;
    }

    public float GetMinAlpha()
    {
        return MinAlpha;
    }
}
