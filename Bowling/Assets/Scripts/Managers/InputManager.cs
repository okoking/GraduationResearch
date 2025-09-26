using UnityEngine;
//���̓V�X�e��
using UnityEngine.InputSystem;
//�t�@�C���E�f�B���N�g������p
using System.IO;
using System;

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

    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //Update is called once per frame
    void Update()
    {
        
    }

    //���o�C���h�J�n
    public void StartRebind(string actionName, int bindingIndex = 0, Action onComplete = null)
    {
        var action = GetAction(actionName);
        if (action == null)
        {
            Debug.LogError($"���o�C���h�Ώۂ̃A�N�V������������܂���: {actionName}");
            return;
        }

        //�����̃��o�C���h���L�����Z��
        action.Disable();

        //�C���^���N�e�B�u���o�C���h�J�n
        action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("<Mouse>/position") // �s�v�Ȃ珜�O
            .WithControlsExcluding("<Mouse>/delta")
            .OnComplete(operation =>
            {
                action.Enable();
                operation.Dispose();

                Debug.Log($"{actionName} �̃��o�C���h����: {action.bindings[bindingIndex].effectivePath}");

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
    public string GetBindingDisplayName(string actionName, int bindingIndex)
    {
        var action = GetAction(actionName);
        if (action == null) return "N/A";

        if (bindingIndex < 0 || bindingIndex >= action.bindings.Count) return "N/A";

        return action.GetBindingDisplayString(bindingIndex);
    }

    //�A�N�V�������擾����
    //actionName �� "Player/Jump" �̂悤�Ƀ}�b�v��/�A�N�V�������Ŏw��
    public InputAction GetAction(string actionName)
    {
        if (string.IsNullOrEmpty(actionName) || inputActions == null)
            return null;

        var parts = actionName.Split('/');
        if (parts.Length != 2)
        {
            Debug.LogError($"ActionName �̌`�����s���ł�: {actionName}");
            return null;
        }

        var mapName = parts[0];
        var actionNameOnly = parts[1];

        var map = inputActions.asset.FindActionMap(mapName, true);
        if (map == null)
        {
            Debug.LogError($"�A�N�V�����}�b�v {mapName} ��������܂���");
            return null;
        }

        var action = map.FindAction(actionNameOnly, true);
        if (action == null)
        {
            Debug.LogError($"�A�N�V���� {actionNameOnly} ��������܂���");
            return null;
        }

        return action;
    }

    //���͈ꗗ
    public Vector2 Move => inputActions.Player.Move.ReadValue<Vector2>();
    public bool Jump => inputActions.Player.Jump.triggered;
    public bool KeyBoardEnter => inputActions.Dicide.KeyBoardDicide.triggered;
    public bool PadEnter => inputActions.Dicide.PadDicide.triggered;
    public bool KeyBoardCancel => inputActions.Cancel.KeyBoradCancel.triggered;
    public bool PadCancel => inputActions.Cancel.PadCancel.triggered;
    public bool KeyBoardOption => inputActions.Option.KeyBoard.triggered;
    public bool PadOption => inputActions.Option.Pad.triggered;
    public bool LeftInput => inputActions.LeftRight.Left.triggered;
    public bool RightInput => inputActions.LeftRight.Right.triggered;

}
