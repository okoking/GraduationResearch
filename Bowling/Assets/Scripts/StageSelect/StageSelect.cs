using UnityEngine;

public class StageSelect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //タイトルシーンへ
        if (InputManager.Instance.Option)
        {
            SceneController.Instance.ChangeState(GameState.Title);
        }

        //プレイシーンへ
        if (InputManager.Instance.Enter)
        {
            SceneController.Instance.ChangeState(GameState.Play);
        }
    }
}
