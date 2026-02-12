using System;
using UnityEngine;

public class ClearUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeTime = 0.001f;
    float t = 0f;
    bool startFlag; //透過を始めるか
    int cont = 200;  //フェード開始のカウント




    void Start()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        startFlag = false;

        cont=200;
    }

    void Update()
    {
        if (!startFlag)
        {
            //カウント減少
            cont--;
        }

        //カウントが終わったらUIを浮かばせる
        if (cont < 0)
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


