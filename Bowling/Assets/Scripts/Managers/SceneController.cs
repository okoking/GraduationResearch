using UnityEngine;
//�V�[���Ǘ��@�\
using UnityEngine.SceneManagement;
//�^���S�ȃR���N�V����
using System.Collections.Generic;
//�񓯊���������񏈗��Ǘ��@�\
using System.Threading.Tasks;

////�V�[���O���[�v
//public enum SceneGroup
//{
//    Title,          //�^�C�g��
//    StageSelect,    //�X�e�[�W�Z���N�g
//    Play,           //�v���C
//    Result,         //���U���g

//}
//�Q�[���X�e�[�g
public enum GameState
{
    Title,              //�^�C�g��
    Option,             //�I�v�V����
    StageSelect,        //�X�e�[�W�Z���N�g
    Play,               //�v���C
    Result,             //���U���g

}

//�V�[���O���[�v
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
    //�V���O���g��
    public static SceneController Instance { get; private set; }

    //���݂̃V�[��
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
        _ = OnEnterTitleAsync();
        Debug.Log("�ŏ��̃V�[���̓^�C�g���ł�");
    }

    //�񓯊��ǂݍ���
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

    //�񓯊�����
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

    //��Ԃ�ύX����
    public void ChangeState(GameState newState)
    {
        //���݂̃V�[�����ύX���悤�Ƃ���V�[���Ɠ����ꍇ
        if (CurrentState == newState) return;

        //�V�[���ύX���O
        Debug.Log($"GameState: {CurrentState} �� {newState}");
        //�V�[����ύX
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
        
        Debug.Log("�^�C�g����ʂ�");
    }
    private async Task OnEnterOptionAsync()
    {
        await Instance.GoTo(GameState.Option);
      
        Debug.Log("�I�v�V������ʂ�");
    }

    private async Task OnEnterPlayAsync()
    {
        await Instance.GoTo(GameState.Play);
       
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
