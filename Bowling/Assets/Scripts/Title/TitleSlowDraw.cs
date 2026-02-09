using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleSlowDraw : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    float       time;             //時間      
    float       text_alph;        //テキストの透明度

    void Start()
    {
        //text = gameObject.GetComponent<TMP_Text>();
        group.alpha = 0.0f;
        time = 0.0f;
        
    }

    enum TitleState
    {
        WAITE,      //待つ
        SLOW_DRAW,  //浮かび上がる
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
                text_alph += 0.001f;

                if (text_alph >= 1.0f)
                {
                    text_alph = 1.0f;
                }

                //透明度加算
                group.alpha = text_alph;
                break;
           
        }

    }
}
