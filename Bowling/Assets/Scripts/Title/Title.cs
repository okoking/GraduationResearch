using UnityEngine;

public class Title : MonoBehaviour
{
    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    //Update is called once per frame
    void Update()
    {
        if(InputManager.Instance.Jump)
        {
            Debug.Log("�W�����v�������ꂽ��");
        }

        //�I�v�V�����V�[����
        if (InputManager.Instance.KeyBoardOption || InputManager.Instance.PadOption)
        {
            SceneController.Instance.ChangeState(GameState.Option);
        }

        //�v���C�V�[����
        if (InputManager.Instance.KeyBoardEnter || InputManager.Instance.PadEnter)
        {

            SceneController.Instance.ChangeState(GameState.Play);
        }

        //if(InputManager.Instance)
        //{

        //}
    }
}
