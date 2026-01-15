using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Unity.IO.LowLevel.Unsafe;

public class FlasingBtn : MonoBehaviour
{
    float time;             //時間      
    float speed = 1.0f;     //スピード

    TMP_Text text;          //テキスト
    float text_alph;        //テキストの透明度

    void Start()
    {
        text = gameObject.GetComponent<TMP_Text>();
        text_alph = 0.0f;
        time = 0.0f;
    }

    enum TitleState
    {
        WAITE,      //待つ
        SLOW_DRAW,  //浮かび上がる
        FLASH_DRAW  //点滅
    }
    TitleState titleState = TitleState.SLOW_DRAW;

    void Update()
    {
        switch (titleState)
        { 
        case TitleState.WAITE:
            time += 0.01f;
            if (time > 50.0f)
            {
                time = 0.0f;
                titleState = TitleState.SLOW_DRAW;
                Debug.Log("UI開始");
            }
            break;

        case TitleState.SLOW_DRAW:
            //加算
             text_alph += 0.0005f;

            if (text_alph >= 1.0f)
            {
                text_alph = 1.0f;
                titleState = TitleState.FLASH_DRAW;
            }

            //透明度加算
            text.color = new Color(1.0f, 1.0f, 1.0f, text_alph);
            break;
        
        case TitleState.FLASH_DRAW:
            //点滅用
            text.color = GetAlphaColor(text.color);
            break;
        }
        
    }


    //テキストのアルファ値をMathF.Sin関数を使って周期的に「０→１」「１→０」をさせている
    //その値を返り値としてUpdateのtext.colorに返している
    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * 3.5f * speed;
        color.a = Mathf.Sin(time);

        return color;
    }
}

