using UnityEngine;
//入力システム
using UnityEngine.InputSystem;
//ファイル・ディレクトリ操作用
using System.IO;
using System;

public class InputManager : MonoBehaviour
{
    //シングルトン
    public static InputManager Instance { get; private set; }

    //インプットキー情報
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

            //保存データ読み込み
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

    //リバインド開始
    public void StartRebind(string actionName, int bindingIndex = 0, Action onComplete = null)
    {
        var action = GetAction(actionName);
        if (action == null)
        {
            Debug.LogError($"リバインド対象のアクションが見つかりません: {actionName}");
            return;
        }

        //既存のリバインドをキャンセル
        action.Disable();

        //インタラクティブリバインド開始
        action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("<Mouse>/position") // 不要なら除外
            .WithControlsExcluding("<Mouse>/delta")
            .OnComplete(operation =>
            {
                action.Enable();
                operation.Dispose();

                Debug.Log($"{actionName} のリバインド完了: {action.bindings[bindingIndex].effectivePath}");

                onComplete?.Invoke();
            })
            .Start();
    }

    //保存
    public void SaveRebinds()
    {
        string rebinds = inputActions.asset.SaveBindingOverridesAsJson();
        //ファイル書き込み
        File.WriteAllText(savePath, rebinds);
    }

    //読み込み
    public void LoadRebinds()
    {
        if (File.Exists(savePath))
        {
            //ファイル読み込み
            string rebinds = File.ReadAllText(savePath);
            inputActions.asset.LoadBindingOverridesFromJson(rebinds);
        }
    }

    //UIで現在のキー表示に使う
    public string GetBindingDisplayName(string actionName, int bindingIndex)
    {
        var action = GetAction(actionName);
        if (action == null) return "N/A";

        if (bindingIndex < 0 || bindingIndex >= action.bindings.Count) return "N/A";

        return action.GetBindingDisplayString(bindingIndex);
    }

    //アクションを取得する
    //actionName は "Player/Jump" のようにマップ名/アクション名で指定
    public InputAction GetAction(string actionName)
    {
        if (string.IsNullOrEmpty(actionName) || inputActions == null)
            return null;

        var parts = actionName.Split('/');
        if (parts.Length != 2)
        {
            Debug.LogError($"ActionName の形式が不正です: {actionName}");
            return null;
        }

        var mapName = parts[0];
        var actionNameOnly = parts[1];

        var map = inputActions.asset.FindActionMap(mapName, true);
        if (map == null)
        {
            Debug.LogError($"アクションマップ {mapName} が見つかりません");
            return null;
        }

        var action = map.FindAction(actionNameOnly, true);
        if (action == null)
        {
            Debug.LogError($"アクション {actionNameOnly} が見つかりません");
            return null;
        }

        return action;
    }

    //入力一覧
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
