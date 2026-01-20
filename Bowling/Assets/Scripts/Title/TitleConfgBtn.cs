using UnityEngine;
using UnityEngine.UI;

public class TitleConfgBtn : MonoBehaviour
{

    [SerializeField] GameObject configPanel;

    private void Start()
    {
        HideConfigpanel();
    }

    public void ShowConfigpanel()
    {
        configPanel.SetActive(true);
    }
    public void HideConfigpanel()
    {
        configPanel.SetActive(false);
    }
}
