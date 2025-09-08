using UnityEngine;

public class Title : MonoBehaviour
{
    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //InputManager.Instance.StartRebind("Player/Jump", () => 
        //{
        //    Debug.Log("Jump が新しいキーに設定されました");
        //});
    }

    //Update is called once per frame
    void Update()
    {
        if(InputManager.Instance.Jump)
        {
            Debug.Log("ジャンプが押されたよ");
        }

        //オプションシーンへ
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SceneController.Instance.ChangeState(GameState.Option);
        }

        //プレイシーンへ
        if (Input.GetKeyDown(KeyCode.Return))
        {

            SceneController.Instance.ChangeState(GameState.Play);
        }

        //if(InputManager.Instance)
        //{

        //}
    }
}
