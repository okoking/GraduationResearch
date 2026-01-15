using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [SerializeField] Image fadeImage;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (fadeImage == null)
        {
            Debug.LogError("ScreenFader: Image Ç™å©Ç¬Ç©ÇËÇ‹ÇπÇÒÅBCanvasîzâ∫ÇÃImageÇ…ïtÇØÇƒÇ≠ÇæÇ≥Ç¢");
        }
    }

    public Coroutine PlayFadeOut(float time)
    {
        return StartCoroutine(Fade(0f, 1f, time));
    }

    public Coroutine PlayFadeIn(float time)
    {
        return StartCoroutine(Fade(1f, 0f, time));
    }

    IEnumerator Fade(float from, float to, float time)
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < 1f)
        {
            t += Time.deltaTime / time;
            c.a = Mathf.Lerp(from, to, t);
            fadeImage.color = c;
            yield return null;
        }

        c.a = to;
        fadeImage.color = c;
    }
}
