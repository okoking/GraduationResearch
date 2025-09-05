using UnityEngine;
//入力システム
using UnityEngine.InputSystem;
//ファイル・ディレクトリ操作用
using System.IO;

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

    //リバインド開始
    public void StartRebind(string actionName, System.Action onComplete = null)
    {
        var action = inputActions.asset.FindAction(actionName);
        if (action == null)
        {
            Debug.LogError($"Action {actionName} が見つかりません");
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
    public string GetBindingDisplayName(string actionName, int bindingIndex = 0)
    {
        var action = inputActions.asset.FindAction(actionName);
        return action != null ? action.GetBindingDisplayString(bindingIndex) : "N/A";
    }
}
