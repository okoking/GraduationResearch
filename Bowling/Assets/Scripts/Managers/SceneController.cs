using UnityEngine;
//�V�[���Ǘ��@�\
using UnityEngine.SceneManagement;
//�^���S�ȃR���N�V����
using System.Collections.Generic;
//�񓯊���������񏈗��Ǘ��@�\
using System.Threading.Tasks;

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
        CurrentState = GameState.Play;
        _ = OnEnterPlayAsync();
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

        GUI.Label(new Rect(200, 5, 300, 30), $"���݂̃V�[��: {mode}");

        ////���[�h�ɉ����ėL����
        //switch (mode)
        //{
        //    case GameState.Select:
        //        GUI.Label(new Rect(10, 20, 300, 30), $"C�L�[�FFreeCamera��");
        //        GUI.Label(new Rect(10, 35, 300, 30), $"V�L�[�FPlayCamera��");
        //        break;
        //    case GameState.FreeLook:
        //        GUI.Label(new Rect(10, 20, 300, 30), $"V�L�[�FPlayCamera��");
        //        GUI.Label(new Rect(10, 35, 300, 30), $"Q�L�[�F�㏸");
        //        GUI.Label(new Rect(10, 50, 300, 30), $"E�L�[�F���~");
        //        break;
        //    case GameState.Play:
        //        GUI.Label(new Rect(10, 20, 300, 30), $"C�L�[�FFreeCamera��");
        //        break;
        //}
    }

    private async Task OnEnterTitleAsync()
    {
        await Instance.GoTo(GameState.Title);
        
        Debug.Log("�^�C�g�����!");
    }
    private async Task OnEnterOptionAsync()
    {
        await Instance.GoTo(GameState.Option);
      
        Debug.Log("�I�v�V�������!");
    }
    private async Task OnEnterStageSelectAsync()
    {
        await Instance.GoTo(GameState.StageSelect);

        Debug.Log("�X�e�[�W�Z���N�g���!");
    }
    private async Task OnEnterPlayAsync()
    {
        await Instance.GoTo(GameState.Play);

        CameraManager.Instance.RegisterCameras();
        CameraManager.Instance.SwitchCamera(CameraMode.Play);

        Debug.Log("�Q�[���J�n�I");

    }

    private async Task OnEnterResult()
    {
        await Instance.GoTo(GameState.Result);

        Debug.Log("���U���g�I");
      
      
    }
 
}
