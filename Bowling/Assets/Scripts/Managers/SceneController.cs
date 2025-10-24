using UnityEngine;
//シーン管理機能
using UnityEngine.SceneManagement;
//型安全なコレクション
using System.Collections.Generic;
//非同期処理や並列処理管理機能
using System.Threading.Tasks;

//ゲームステート
public enum GameState
{
    Title,              //タイトル
    Option,             //オプション
    StageSelect,        //ステージセレクト
    Play,               //プレイ
    Result,             //リザルト

}

//シーングループ
public static class SceneGroups
{
    public static readonly Dictionary<GameState, List<string>> Groups =
        new Dictionary<GameState, List<string>>()
        {
              { GameState.Title, new List<string> { "TitleUI", "Title" } },
              { GameState.Option, new List<string> { "Option", "OptionUI" } },
              { GameState.StageSelect, new List<string> { "StageSelect", "StageSelectUI" } },
              { GameState.Play, new List<string> { "Play", "Player","EnemyTest",
                  "Gimmick","Map","BossTest" } },
              { GameState.Result, new List<string> { "Result", "Result_UI" } },
        };
}

public class SceneController : MonoBehaviour
{
    //シングルトン
    public static SceneController Instance { get; private set; }

    //現在のシーン
    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        //インスタンスが空であれば
        if (Instance == null)
        {
            Instance = this;
            //シーンを遷移しても破棄されないようにするため
            DontDestroyOnLoad(gameObject);
            Debug.Log("SceneControllerインスタンスが生成されました");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("二つ目のインスタンスが生成されたため削除しました");
        }
    }

    void Start()
    {
        //最初タイトル
        CurrentState = GameState.Play;
        _ = OnEnterPlayAsync();
        Debug.Log("最初のシーンはタイトルです");
    }

    //非同期読み込み
    public async Task LoadSceneAsync(List<string> scenenames)
    {
        foreach (var scenename in scenenames)
        {
            if (!SceneManager.GetSceneByName(scenename).isLoaded)
            {
                var op = SceneManager.LoadSceneAsync(scenename, LoadSceneMode.Additive);
                while (!op.isDone) await Task.Yield();
            }
        }
    }

    //非同期消去
    public async Task UnloadSceneAsync(List<string> scenenames)
    {
        foreach (var scenename in scenenames)
        {
            if (SceneManager.GetSceneByName(scenename).isLoaded)
            {
                var op = SceneManager.UnloadSceneAsync(scenename);
                while (!op.isDone) await Task.Yield();
            }
        }
    }
    public async Task GoTo(GameState group)
    {
        await UnloadAllExceptManagers();
        await LoadSceneAsync(SceneGroups.Groups[group]);

    }

    private async Task UnloadAllExceptManagers()
    {
        var allScenes = new List<string>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (scene.name != "Managers")
            {
                allScenes.Add(scene.name);
            }
        }
        await UnloadSceneAsync(allScenes);
    }

    //状態を変更する
    public void ChangeState(GameState newState)
    {
        //現在のシーンが変更しようとするシーンと同じ場合
        if (CurrentState == newState) return;

        //シーン変更ログ
        Debug.Log($"GameState: {CurrentState} → {newState}");
        //シーンを変更
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Title:
                _ = OnEnterTitleAsync();
                break;
            case GameState.Option:
                _ = OnEnterOptionAsync();
                break;
            case GameState.StageSelect:
                _ = OnEnterStageSelectAsync();
                break;
            case GameState.Play:
                _ = OnEnterPlayAsync();
                break;
            case GameState.Result:
                _ = OnEnterResult();
                break;
            
        }
    }
    void Update()
    {
      
    }
    private void OnGUI()
    {
        if (Instance == null) return;

        GameState mode = Instance.CurrentState;
        GUI.color = Color.cyan;

        GUI.Label(new Rect(200, 5, 300, 30), $"現在のシーン: {mode}");

        ////モードに応じて有効化
        //switch (mode)
        //{
        //    case GameState.Select:
        //        GUI.Label(new Rect(10, 20, 300, 30), $"Cキー：FreeCameraへ");
        //        GUI.Label(new Rect(10, 35, 300, 30), $"Vキー：PlayCameraへ");
        //        break;
        //    case GameState.FreeLook:
        //        GUI.Label(new Rect(10, 20, 300, 30), $"Vキー：PlayCameraへ");
        //        GUI.Label(new Rect(10, 35, 300, 30), $"Qキー：上昇");
        //        GUI.Label(new Rect(10, 50, 300, 30), $"Eキー：下降");
        //        break;
        //    case GameState.Play:
        //        GUI.Label(new Rect(10, 20, 300, 30), $"Cキー：FreeCameraへ");
        //        break;
        //}
    }

    private async Task OnEnterTitleAsync()
    {
        await Instance.GoTo(GameState.Title);
        
        Debug.Log("タイトル画面!");
    }
    private async Task OnEnterOptionAsync()
    {
        await Instance.GoTo(GameState.Option);
      
        Debug.Log("オプション画面!");
    }
    private async Task OnEnterStageSelectAsync()
    {
        await Instance.GoTo(GameState.StageSelect);

        Debug.Log("ステージセレクト画面!");
    }
    private async Task OnEnterPlayAsync()
    {
        await Instance.GoTo(GameState.Play);

        CameraManager.Instance.RegisterCameras();
        CameraManager.Instance.SwitchCamera(CameraMode.Play);

        Debug.Log("ゲーム開始！");

    }

    private async Task OnEnterResult()
    {
        await Instance.GoTo(GameState.Result);

        Debug.Log("リザルト！");
      
      
    }
 
}
