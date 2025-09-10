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
        //�^�C�g���V�[����
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SceneController.Instance.ChangeState(GameState.Title);
        }

        //�v���C�V�[����
        if (InputManager.Instance.KeyBoardEnter || InputManager.Instance.PadEnter)
        {
            SceneController.Instance.ChangeState(GameState.Play);
        }
    }
}
