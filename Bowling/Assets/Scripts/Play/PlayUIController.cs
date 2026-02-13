using System.Collections;
using UnityEngine;

public class PlayUIController : MonoBehaviour
{
    [SerializeField] private Canvas playCanvas;
    [SerializeField] private float delay = 2f;

    void Start()
    {
        playCanvas.gameObject.SetActive(false);
        StartCoroutine(ShowUIAfterDelay());
    }

    IEnumerator ShowUIAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        playCanvas.gameObject.SetActive(true);
    }
}
