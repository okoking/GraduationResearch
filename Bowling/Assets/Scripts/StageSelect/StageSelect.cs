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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SceneController.Instance.ChangeState(GameState.Title);
        }

        //プレイシーンへ
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneController.Instance.ChangeState(GameState.Play);
        }
    }
}
