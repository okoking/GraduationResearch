using UnityEngine;

public class ClearUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            canvasGroup.alpha = 1f;
        }
    }

}

