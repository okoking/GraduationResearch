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
        if (InputManager.Instance.Option)
        {
            SceneController.Instance.ChangeState(GameState.Option);
        }
        
        //�X�e�[�W�Z���N�g�V�[����
        if (InputManager.Instance.Enter)
        {
            SceneController.Instance.ChangeState(GameState.StageSelect);
        }

       
    }
}
