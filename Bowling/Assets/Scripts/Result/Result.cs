using UnityEngine;

public class Result : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      

        //ステージセレクトシーンへ
        if (InputManager.Instance.Enter)
        {
            SceneController.Instance.ChangeState(GameState.StageSelect);
        }
    }
}
