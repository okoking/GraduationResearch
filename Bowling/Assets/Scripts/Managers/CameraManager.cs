using UnityEngine;

//�J�������[�h
public enum CameraMode
{
    FreeLook,       //�R�[�X�m�F
    Play,           //�v���C
    Enemy,          //�G�l�~�[
    Boss,           //�{�X

    //Replay        //�����p
}

//�J�����}�l�[�W���[
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    private Camera freeLookCamera;
    private Camera playerCamera;
    private Camera EnemyCamera;
    private Camera BossCamera;

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
        freeLookCamera = GameObject.Find("FreeCamera")?.GetComponent<Camera>();
        playerCamera = GameObject.Find("PlayerCamera")?.GetComponent<Camera>();
        EnemyCamera = GameObject.Find("EnemyCamera")?.GetComponent<Camera>();
        BossCamera = GameObject.Find("Main Camera")?.GetComponent<Camera>();
        if (freeLookCamera == null) Debug.LogWarning("FreeCamera ��������܂���I");
        if (playerCamera == null) Debug.LogWarning("PlayerCamera ��������܂���I");
        if (EnemyCamera == null) Debug.LogWarning("EnemyCamera ��������܂���I");
        if (BossCamera == null) Debug.LogWarning("BossCamera ��������܂���I");
    }

    public void SwitchCamera(CameraMode mode)
    {
        currentMode = mode;

        //���ׂẴJ�����𖳌���
        if (freeLookCamera != null) freeLookCamera.gameObject.SetActive(false);
        if (playerCamera != null) playerCamera.gameObject.SetActive(false);
        if (EnemyCamera != null) EnemyCamera.gameObject.SetActive(false);
        if (BossCamera != null) BossCamera.gameObject.SetActive(false);

        //���[�h�ɉ����ėL����
        switch (mode)
        {
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

    private void OnGUI()
    {
        if (Instance == null) return;

        CameraMode mode = Instance.GetCurrentMode()/*.ToString()*/;
        GUI.color = Color.red;
        
        GUI.Label(new Rect(10, 5, 300, 30), $"���݂̃J�������[�h: {mode}");

        //���[�h�ɉ����ėL����
        switch (mode)
        {
            case CameraMode.FreeLook:
                GUI.Label(new Rect(10, 20, 300, 30), $"V�L�[�FPlayCamera��");
                GUI.Label(new Rect(10, 35, 300, 30), $"Q�L�[�F�㏸");
                GUI.Label(new Rect(10, 50, 300, 30), $"E�L�[�F���~");
                break;
            case CameraMode.Play:
                GUI.Label(new Rect(10, 20, 300, 30), $"C�L�[�FFreeCamera��");
                GUI.Label(new Rect(10, 35, 300, 30), $"Space�L�[�FMissileIventCamera��");
                break;
        }
    }

    //�J�������[�h�擾�֐�
    public CameraMode GetCurrentMode() => currentMode;
}
