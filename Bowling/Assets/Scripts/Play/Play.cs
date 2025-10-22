using UnityEngine;

public class Play : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�v���C���[�Z�b�g
        EnemyManager.Instance.SetPlayer(
            GameObject.Find("Player").transform);
    }

    // Update is called once per frame
    void Update()
    {
       

        //�X�e�[�W�Z���N�g�V�[����
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SceneController.Instance.ChangeState(GameState.StageSelect);
        }

        //���U���g�V�[����
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            SceneController.Instance.ChangeState(GameState.Result);
        }

        //�t���[�J�����ɕύX
        if(Input.GetKeyDown(KeyCode.C))
        {
            //CameraManager.Instance.SwitchCamera(CameraMode.FreeLook);
            //Debug.Log("�t���[�J�����֕ύX");
        }

        //�t���[�J�����ɕύX
        if (Input.GetKeyDown(KeyCode.V))
        {
            CameraManager.Instance.SwitchCamera(CameraMode.Play);
        }
    }
}
