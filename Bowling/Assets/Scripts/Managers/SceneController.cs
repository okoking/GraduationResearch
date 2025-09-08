using UnityEngine;
//シーン管理機能
using UnityEngine.SceneManagement;
//型安全なコレクション
using System.Collections.Generic;
//非同期処理や並列処理管理機能
using System.Threading.Tasks;

////シーングループ
//public enum SceneGroup
//{
//    Title,          //タイトル
//    StageSelect,    //ステージセレクト
//    Play,           //プレイ
//    Result,         //リザルト

//}
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
<<<<<<< Updated upstream
<<<<<<< Updated upstream
              { GameState.Title, new List<string> { /*"Title_UI", "Title_BG", */"Title"} },
              { GameState.Option, new List<string> { "OptionUI" } },
              { GameState.StageSelect, new List<string> { "StageSelect" } },
              { GameState.Play, new List<string> { /*"Play_UI", "Play_BG", */"Player","EnemyTest", "Gimmick" } },
              { GameState.Result, new List<string> { "Result_UI", "Resultr_BG" } },
=======
=======
>>>>>>> Stashed changes
              { GameState.Title, new List<string>       { "Title", "TitleUI" } },
              { GameState.Option, new List<string>      { "Option", "OptionUI" } },
              { GameState.StageSelect, new List<string> { "StageSelect", "StageSelectUI" } },
              { GameState.Play, new List<string>        { "Play", "PlayUI", "Player","EnemyTest",} },
              { GameState.Result, new List<string>      { "Result", "Result_UI", "Resultr_BG" } },
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
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
        CurrentState = GameState.Title;
        _ = OnEnterTitleAsync();
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
            case GameState.Play:
                _ = OnEnterPlayAsync();
                break;
            case GameState.Result:
                OnEnterResult();
                break;
            
        }
    }
    void Update()
    {
      
    }

    private async Task OnEnterTitleAsync()
    {
        await Instance.GoTo(GameState.Title);
        
        Debug.Log("タイトル画面へ");
    }
    private async Task OnEnterOptionAsync()
    {
        await Instance.GoTo(GameState.Option);
      
        Debug.Log("オプション画面へ");
    }

    private async Task OnEnterPlayAsync()
    {
        await Instance.GoTo(GameState.Play);
       
        Debug.Log("ゲーム開始！");
    }

    private void OnEnterResult()
    {
      
        Debug.Log("リザルト！");
      
        //_ = SceneController.Instance.SwitchScenesAsync(
        //    new string[] { "Game_Stage", "Game_Player", "Game_Enemy", "Game_UI" },
        //    new string[] { "GameOver_UI", "GameOver_BG" }
        //);
    }
 
}
