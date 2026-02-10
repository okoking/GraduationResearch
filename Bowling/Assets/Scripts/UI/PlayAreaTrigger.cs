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

            if(areaName == "第1エリア")
            {
                if (EnemySpawn.Instance.currentStage != 0)
                {
                    EnemySpawn.Instance.ClearEnemies();
                    Debug.LogWarning("敵を消去しました");
                }
                EnemySpawn.Instance.SetStage(0);
               
            }
            else if(areaName == "第2エリア")
            {
                if (EnemySpawn.Instance.currentStage != 1)
                {
                    EnemySpawn.Instance.ClearEnemies();
                    Debug.LogWarning("敵を消去しました");
                }
                EnemySpawn.Instance.SetStage(1);
                
            }
            else if(areaName == "第3エリア")
            {
                if (EnemySpawn.Instance.currentStage != 2)
                {
                    EnemySpawn.Instance.ClearEnemies();
                    Debug.LogWarning("敵を消去しました");
                }
                EnemySpawn.Instance.SetStage(2);
               
            }
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
