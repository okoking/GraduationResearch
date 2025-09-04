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
        if(Instance = null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            inputActions = new GameInput();
            inputActions.Enable();

            savePath = Path.Combine(Application.persistentDataPath, "rebinds.json");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
