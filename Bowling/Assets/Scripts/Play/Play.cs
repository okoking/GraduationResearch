using UnityEngine;

public class Play : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ////プレイヤーセット
        //EnemyManager.Instance.SetPlayer(
        //    GameObject.Find("Player").transform);
        SoundManager.Instance.Request("BGMPlayFirstArea", true);

    }

    // Update is called once per frame
    void Update()
    {
       

        //ステージセレクトシーンへ
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SceneController.Instance.ChangeState(GameState.StageSelect);
        }

        //リザルトシーンへ
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            SceneController.Instance.ChangeState(GameState.Result);
            EnemySpawn.Instance.ClearEnemies();
            Destroy(CameraManager.Instance);
        }

        //フリーカメラに変更
        if(Input.GetKeyDown(KeyCode.C))
        {
            //CameraManager.Instance.SwitchCamera(CameraMode.FreeLook);
            //Debug.Log("フリーカメラへ変更");
        }

        ////フリーカメラに変更
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    CameraManager.Instance.SwitchCamera(CameraMode.Player);
        //}
    }
}
