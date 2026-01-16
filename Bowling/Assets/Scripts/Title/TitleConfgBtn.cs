using UnityEngine;
using UnityEngine.UI;

public class TitleConfgBtn : MonoBehaviour
{

    [SerializeField] GameObject configPanel;

    public void ShowConfigpanel()
    {
        configPanel.SetActive(true);
    }
    public void HideConfigpanel()
    {
        configPanel.SetActive(true);
    }
}
