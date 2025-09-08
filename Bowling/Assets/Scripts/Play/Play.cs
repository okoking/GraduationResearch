using UnityEngine;

public class Play : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneController.Instance.ChangeState(GameState.Result);
        }
    }
}
