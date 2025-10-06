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
            turnText.text = $"ターン: {currentTurn}/{maxTurns}";

        if (statusText)
            statusText.text = $"現在: {state}";
    }

    public void EndPlayerTurn()
    {
        if (state != TurnState.PlayerTurn) return;

        state = TurnState.EnemyTurn;
        UpdateUI();
        Invoke(nameof(StartEnemyTurn), 1.0f); //少し待ってから敵ターンへ
    }

    private void StartEnemyTurn()
    {
        Debug.Log("敵のターン開始！");
        //敵の行動呼び出し
        Invoke(nameof(EndEnemyTurn), 1.0f); //行動後にターン終了
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
        Debug.Log("プレイヤーのターンに戻る");
        UpdateUI();
    }

    private void GameOver()
    {
        state = TurnState.GameOver;
        Debug.Log("ゲームオーバー！ターン数を超過！");
        if (statusText)
            statusText.text = "ゲームオーバー！";
    }
}
