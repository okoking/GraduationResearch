using UnityEngine;

public enum TurnState
{
    PlayerTurn,
    EnemyTurn,
    TurnEnd,
    GameOver
}

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public int currentTurn { get; private set; } = 1;
    public int maxTurns = 10;
    public TurnState state { get; private set; } = TurnState.PlayerTurn;

    [SerializeField] private TMPro.TextMeshProUGUI turnText;
    [SerializeField] private TMPro.TextMeshProUGUI statusText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (turnText)
            turnText.text = $"�^�[��: {currentTurn}/{maxTurns}";

        if (statusText)
            statusText.text = $"����: {state}";
    }

    public void EndPlayerTurn()
    {
        if (state != TurnState.PlayerTurn) return;

        state = TurnState.EnemyTurn;
        UpdateUI();
        Invoke(nameof(StartEnemyTurn), 1.0f); //�����҂��Ă���G�^�[����
    }

    private void StartEnemyTurn()
    {
        Debug.Log("�G�̃^�[���J�n�I");
        //�G�̍s���Ăяo��
        Invoke(nameof(EndEnemyTurn), 1.0f); //�s����Ƀ^�[���I��
    }

    private void EndEnemyTurn()
    {
        currentTurn++;
        if (currentTurn > maxTurns)
        {
            GameOver();
            return;
        }

        state = TurnState.PlayerTurn;
        Debug.Log("�v���C���[�̃^�[���ɖ߂�");
        UpdateUI();
    }

    private void GameOver()
    {
        state = TurnState.GameOver;
        Debug.Log("�Q�[���I�[�o�[�I�^�[�����𒴉߁I");
        if (statusText)
            statusText.text = "�Q�[���I�[�o�[�I";
    }
}
