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
      

        //�X�e�[�W�Z���N�g�V�[����
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneController.Instance.ChangeState(GameState.StageSelect);
        }
    }
}
