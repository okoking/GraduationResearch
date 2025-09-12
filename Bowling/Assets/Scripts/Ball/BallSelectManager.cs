using UnityEngine;
using UnityEngine.UI;

public class BallSelectManager : MonoBehaviour
{
    [SerializeField] private Image[] ballIcons;      //ボールのアイコンUI
    [SerializeField] private BallData[] ballDatas;   //対応するボールデータ
    [SerializeField] private RectTransform cursor;   //カーソル（ハイライト用）

    private int currentIndex = 0;

    private void OnEnable()
    {
        UpdateCursor();
    }

    void Start()
    {
        
    }

    private void Update()
    {
        if (BowlingUIManager.Instance.CurrentPhase != BowlingPhase.BallSelect)
            return;

        //左右入力（キーボード or コントローラー想定）
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = (currentIndex + 1) % ballIcons.Length;
            UpdateCursor();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex = (currentIndex - 1 + ballIcons.Length) % ballIcons.Length;
            UpdateCursor();
        }

        //決定
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            SelectBall();
        }
    }
    private void UpdateCursor()
    {
        //カーソルを選択中のアイコンの位置に移動
        cursor.position = ballIcons[currentIndex].transform.position;
    }

    private void SelectBall()
    {
        BallData selected = ballDatas[currentIndex];

        Debug.Log($"{selected.ballName} を選択しました！");

        //BallData を直接渡す
        BallManager.Instance.SelectBall(selected);

        //フェーズを投球へ移行
        BowlingUIManager.Instance.SetPhase(BowlingPhase.Throw);
    }
}
