using UnityEngine;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("ゲーム開始");

        SoundManager.Instance.Request("BGMResult");
    }

    // Update is called once per frame
    void Update()
    {

        //ステージセレクトシーンへ
        //if (InputManager.Instance.Enter)
        //{
        //    //SceneController.Instance.ChangeState(GameState.Title);
        //}

        //仮追加
        if (InputManager.Instance.Enter)
        {
            SoundManager.Instance.Stop("BGMResult");
        }
    }
}
