using UnityEngine;
using TMPro;

public enum BowlingPhase
{
    BallSelect,
    Throw
}

public class BowlingUIManager : MonoBehaviour
{
    public static BowlingUIManager Instance;

    [SerializeField] private GameObject ballSelectPanel; //ボール選択用UI
    //[SerializeField] private GameObject throwPanel;      //投球用UI
     [SerializeField] private TMP_Text messageText;       //画面中央のメッセージ

    private BowlingPhase currentPhase; //内部で制御
    public BowlingPhase CurrentPhase => currentPhase; //外部は読み取り専用

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        SetPhase(BowlingPhase.BallSelect); //最初はボール選択
    }

    public void SetPhase(BowlingPhase phase)
    {
        currentPhase = phase;

        //UI切り替え
        ballSelectPanel.SetActive(phase == BowlingPhase.BallSelect);
        //throwPanel.SetActive(phase == BowlingPhase.Throw);
    }
}