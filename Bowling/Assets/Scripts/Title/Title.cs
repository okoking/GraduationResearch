using UnityEngine;

public class Title : MonoBehaviour
{
    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //InputManager.Instance.StartRebind("Player/Jump", () => 
        //{
        //    Debug.Log("Jump ���V�����L�[�ɐݒ肳��܂���");
        //});
    }

    //Update is called once per frame
    void Update()
    {
        if(InputManager.Instance.Jump)
        {
            Debug.Log("�W�����v�������ꂽ��");
        }

        //�I�v�V�����V�[����
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SceneController.Instance.ChangeState(GameState.Option);
        }

        //�v���C�V�[����
        if (Input.GetKeyDown(KeyCode.Return))
        {

            SceneController.Instance.ChangeState(GameState.Play);
        }

        //if(InputManager.Instance)
        //{

        //}
    }
}
