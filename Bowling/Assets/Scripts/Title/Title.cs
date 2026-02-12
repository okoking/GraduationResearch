using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{

    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("ゲーム開始");
        SoundManager.Instance.Request("BGMTitle");

    }

    //Update is called once per frame
    void Update()
    {

        //ステージセレクトシーンへ
        if (InputManager.Instance.Enter)
        {
            SceneController.Instance.ChangeState(GameState.Play);
            SoundManager.Instance.Stop("BGMTitle",true);
        }
            if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            SceneController.Instance.ChangeState(GameState.Play);
            SoundManager.Instance.Stop("BGMTitle",true);
        }
    }
}
