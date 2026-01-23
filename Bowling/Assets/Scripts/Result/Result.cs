using UnityEngine;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("ゲーム開始");
        //UIシーンを合わせている
        SceneManager.LoadScene("ResultUI", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
      
        //ステージセレクトシーンへ
        //if (InputManager.Instance.Enter)
        //{
        //    //SceneController.Instance.ChangeState(GameState.StageSelect);
        //}
    }
}
