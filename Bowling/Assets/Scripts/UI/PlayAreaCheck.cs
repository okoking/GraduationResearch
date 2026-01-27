using TMPro;
using UnityEngine;

public class PlayAreaCheck : MonoBehaviour
{

    //o“ü‚èŒû‚ÌŠ´ˆ³”Å‚ğ“¥‚ñ‚¾‚ç•¶š‚ğ•ÏX‚·‚é

    public static PlayAreaCheck Instance;
    [SerializeField] private TextMeshProUGUI areaNameText;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
        }
    }
    public void Show(string name)
    {
        areaNameText.text = name;
    }
    public void Hide()
    {
        areaNameText.text = "";

    }
}
