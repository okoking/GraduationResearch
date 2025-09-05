using UnityEngine;
//���̓V�X�e��
using UnityEngine.InputSystem;
//�t�@�C���E�f�B���N�g������p
using System.IO;

public class InputManager : MonoBehaviour
{
    //�V���O���g��
    public static InputManager Instance { get; private set; }

    //�C���v�b�g�L�[���
    private GameInput inputActions;

    private string savePath;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            inputActions = new GameInput();
            inputActions.Enable();

            savePath = Path.Combine(Application.persistentDataPath, "rebinds.json");

            //�ۑ��f�[�^�ǂݍ���
            LoadRebinds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Vector2 Move => inputActions.Player.Move.ReadValue<Vector2>();
    public bool Jump => inputActions.Player.Jump.triggered;

    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //Update is called once per frame
    void Update()
    {
        
    }

    //���o�C���h�J�n
    public void StartRebind(string actionName, System.Action onComplete = null)
    {
        var action = inputActions.asset.FindAction(actionName);
        if (action == null)
        {
            Debug.LogError($"Action {actionName} ��������܂���");
            return;
        }

        action.PerformInteractiveRebinding()
            .OnComplete(operation =>
            {
                operation.Dispose();
                SaveRebinds();
                onComplete?.Invoke();
            })
            .Start();
    }

    //�ۑ�
    public void SaveRebinds()
    {
        string rebinds = inputActions.asset.SaveBindingOverridesAsJson();
        //�t�@�C����������
        File.WriteAllText(savePath, rebinds);
    }

    //�ǂݍ���
    public void LoadRebinds()
    {
        if (File.Exists(savePath))
        {
            //�t�@�C���ǂݍ���
            string rebinds = File.ReadAllText(savePath);
            inputActions.asset.LoadBindingOverridesFromJson(rebinds);
        }
    }

    //UI�Ō��݂̃L�[�\���Ɏg��
    public string GetBindingDisplayName(string actionName, int bindingIndex = 0)
    {
        var action = inputActions.asset.FindAction(actionName);
        return action != null ? action.GetBindingDisplayString(bindingIndex) : "N/A";
    }
}
