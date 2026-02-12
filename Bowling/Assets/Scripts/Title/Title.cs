using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{

    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("ゲーム開始");
        //UIシーンを合わせている
        SceneManager.LoadScene("TitleUi", LoadSceneMode.Additive);
        //SoundManager.Instance.Request("BGM_Title");

    }

    //Update is called once per frame
    void Update()
    {

        //if (InputManager.Instance.Jump)
        //{
        //    Debug.Log("ボタン押された");
        //}

        ////オプションシーンへ
        //if (InputManager.Instance.Option)
        //{
        //    SceneController.Instance.ChangeState(GameState.Option);
        //}

        //ステージセレクトシーンへ
        if (InputManager.Instance.Enter)
        {
            SceneController.Instance.ChangeState(GameState.Play);
        }
            if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            SceneController.Instance.ChangeState(GameState.Play);
        }
    }
}
