using System;
using UnityEngine;

public class ClearUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeTime = 0.001f;
    float t = 0f;
    bool startFlag; //透過を始めるか

    void Start()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        startFlag = false;
    }

    void Update()
    {
        //仮のトリガー
        //本来はゲームがクリアしたら
        if (Input.GetKeyDown(KeyCode.F))
        {
            startFlag = true;
        }

        Fead();
    }

    void Fead()
    {
        if (startFlag && t < 1)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeTime);
        }
    }
}


