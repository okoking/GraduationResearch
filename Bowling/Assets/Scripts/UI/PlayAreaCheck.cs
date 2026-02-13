using TMPro;
using UnityEngine;

public class PlayAreaCheck : MonoBehaviour
{

    //出入り口の感圧版を踏んだら文字を変更する★
    //UIにつけるスクリプト

    public static PlayAreaCheck Instance;
    [SerializeField] private TextMeshProUGUI areaNameText;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        //else
        //{
        //    Destroy(gameObject);
        //}
    }

    //文字を表示
    public void Show(string name)
    {
        areaNameText.text = name;
    }

    //文字を非表示
    //public void Hide()
    //{
    //    areaNameText.text = "";

    //}
}
