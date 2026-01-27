using UnityEngine;

public class PlayAreaTrigger : MonoBehaviour
{
    [SerializeField] private string areaName;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayAreaCheck.Instance.Show(areaName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayAreaCheck.Instance.Hide();
        }
    }
}
