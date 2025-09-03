using UnityEngine;
//�V�[���Ǘ��@�\
using UnityEngine.SceneManagement;
//�^���S�ȃR���N�V����
using System.Collections.Generic;
//�񓯊���������񏈗��Ǘ��@�\
using System.Threading.Tasks;

//�V�[���O���[�v
public enum SceneGroup
{
    Title,
    StageSelect,
    Play,
    Result,

}
//�Q�[���X�e�[�g
public enum GameState
{
    Title,
    StageSelect,
    Play,
    Result,

}

public static class SceneGroups
{
    public static readonly Dictionary<SceneGroup, List<string>> Groups =
        new Dictionary<SceneGroup, List<string>>()
        {
              { SceneGroup.Title, new List<string> { /*"Title_UI", "Title_BG", */"Title"} },
              { SceneGroup.StageSelect, new List<string> { "StageSelect" } },
              { SceneGroup.Play, new List<string> { /*"Play_UI", "Play_BG", */"Title", "Map1"} },
              { SceneGroup.Result, new List<string> { "Result_UI", "Resultr_BG" } },
        };
}

public class SceneController : MonoBehaviour
{
    //�V���O���g��
    public static SceneController Instance { get; private set; }

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        //�C���X�^���X����ł����
        if (Instance == null)
        {
            Instance = this;
            //�V�[����J�ڂ��Ă��j������Ȃ��悤�ɂ��邽��
            DontDestroyOnLoad(gameObject);
            Debug.Log("SceneController�C���X�^���X����������܂���");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("��ڂ̃C���X�^���X���������ꂽ���ߍ폜���܂���");
        }
    }

    void Start()
    {
        //�ŏ��^�C�g��
        CurrentState = GameState.Title;
    }

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
    public async Task GoTo(SceneGroup group)
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

    // ��Ԃ�ύX����
    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;

        Debug.Log($"GameState: {CurrentState} �� {newState}");
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Title:
                _ = OnEnterTitleAsync();
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
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Instance.ChangeState(GameState.Play);
        }
    }

    private async Task OnEnterTitleAsync()
    {
        await Instance.GoTo(SceneGroup.Title);
        SceneController.Instance.ChangeState(GameState.Title);
        Debug.Log("�^�C�g����ʂ�");
    }

    private async Task OnEnterPlayAsync()
    {
        await Instance.GoTo(SceneGroup.Play);
        SceneController.Instance.ChangeState(GameState.Play);
        Debug.Log("�Q�[���J�n�I");
    }

    private void OnEnterResult()
    {
      
        Debug.Log("���U���g�I");
      
        //_ = SceneController.Instance.SwitchScenesAsync(
        //    new string[] { "Game_Stage", "Game_Player", "Game_Enemy", "Game_UI" },
        //    new string[] { "GameOver_UI", "GameOver_BG" }
        //);
    }
 
}
