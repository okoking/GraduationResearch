using UnityEngine;

public class Option : MonoBehaviour
{
    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //Update is called once per frame
    void Update()
    {
        //�^�C�g���V�[����
        if (InputManager.Instance.Enter)
        {
            SceneController.Instance.ChangeState(GameState.Title);
        }
    }
}
