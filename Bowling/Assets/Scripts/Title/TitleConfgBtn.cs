using UnityEngine;
using UnityEngine.UI;

public class TitleConfgBtn : MonoBehaviour
{

    [SerializeField] GameObject configPanel;

    private void Start()
    {
        Debug.Log("confg:Start");
        HideConfigpanel();
    }

    public void ShowConfigpanel()
    {
        configPanel.SetActive(true);
        Debug.Log("show");


    }
    public void HideConfigpanel()
    {
        configPanel.SetActive(false);
        Debug.Log("hide");
    }
}
