using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioConfig : MonoBehaviour
{

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioSource seAudio;
    [SerializeField] AudioSource bgmAudio;

    [SerializeField] Slider SESlider;
    [SerializeField] Slider BGMSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //スライダーを触ったら音量が変化する
        BGMSlider.onValueChanged.AddListener((value) =>
        {
            value = Mathf.Clamp01(value);

            //変化するのは-80から0までの間
            //log 1=0
            float decibel = 20f * Mathf.Log10(value);
            //デシベルの計算
            decibel = Mathf.Clamp(decibel, -80f, 0f);
            audioMixer.SetFloat("BGM", decibel);

        });

        //スライダーを触ったら音量が変化する
        SESlider.onValueChanged.AddListener((value) =>
        {
            value = Mathf.Clamp01(value);

            //変化するのは-80から0までの間
            //log 1=0
            float decibel = 20f * Mathf.Log10(value);
            //デシベルの計算
            decibel = Mathf.Clamp(decibel, -80f, 0f);
            audioMixer.SetFloat("SE", decibel);

        });
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            seAudio.Play();
        }

    }
}
