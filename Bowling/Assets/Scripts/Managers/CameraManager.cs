using UnityEngine;

//�J�������[�h
public enum CameraMode
{
    Select,     //�{�[���I��UI
    FreeLook,   //�R�[�X�m�F
    Play,       //������
    Replay      //�����p
}

//�J�����}�l�[�W���[
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    private Camera selectCamera;
    private Camera freeLookCamera;
    private Camera playerCamera;

    private CameraMode currentMode;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
       
    }

    void Update()
    {
        //�K�v�ɉ����ăJ�����؂�ւ��̓��͏����Ȃ�
       
    }

    //���I�ɃV�[�����̃J������T���ēo�^
    public void RegisterCameras()
    {
        selectCamera = GameObject.Find("SelectCamera")?.GetComponent<Camera>();
        freeLookCamera = GameObject.Find("FreeCamera")?.GetComponent<Camera>();
        playerCamera = GameObject.Find("PlayerCamera")?.GetComponent<Camera>();

        if (selectCamera == null) Debug.LogWarning("SelectCamera ��������܂���I");
        if (freeLookCamera == null) Debug.LogWarning("FreeCamera ��������܂���I");
        if (playerCamera == null) Debug.LogWarning("PlayerCamera ��������܂���I");
    }

    public void SwitchCamera(CameraMode mode)
    {
        currentMode = mode;

        //���ׂẴJ�����𖳌���
        if (selectCamera != null) selectCamera.gameObject.SetActive(false);
        if (freeLookCamera != null) freeLookCamera.gameObject.SetActive(false);
        if (playerCamera != null) playerCamera.gameObject.SetActive(false);

        //���[�h�ɉ����ėL����
        switch (mode)
        {
            case CameraMode.Select:
                if (selectCamera != null) selectCamera.gameObject.SetActive(true);
                Debug.Log("�Z���N�g�J�����ɕύX");
                break;
            case CameraMode.FreeLook:
                if (freeLookCamera != null) freeLookCamera.gameObject.SetActive(true);
                Debug.Log("�t���[�J�����ɕύX");
                break;
            case CameraMode.Play:
                if (playerCamera != null) playerCamera.gameObject.SetActive(true);
                Debug.Log("�v���C�J�����ɕύX");
                break;
        }
    }

    //�J�������[�h�擾�֐�
    public CameraMode GetCurrentMode() => currentMode;
}
