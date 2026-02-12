using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async Task FadeOut()
    {
        await Fade(0f, 1f);
    }

    public async Task FadeIn()
    {
        await Fade(1f, 0f);
    }

    private async Task Fade(float start, float end)
    {
        float time = 0f;
        Color color = fadeImage.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;
            color.a = Mathf.Lerp(start, end, t);
            fadeImage.color = color;
            await Task.Yield();
        }

        color.a = end;
        fadeImage.color = color;
    }
}
