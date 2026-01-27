using UnityEngine;

public class PlayAreaTrigger : MonoBehaviour
{
    //トリガーとなるオブジェクトにつけるもの

    [SerializeField] private string areaName;

    //文字を表示する
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayAreaCheck.Instance.Show(areaName);
        }
    }

    //離れたら文字を隠す
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        PlayAreaCheck.Instance.Hide();
    //    }
    //}
}
