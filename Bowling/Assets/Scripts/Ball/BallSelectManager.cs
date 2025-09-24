using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class BallSelectManager : MonoBehaviour
{
    [SerializeField] private Transform contentParent;     // UI配置先
    [SerializeField] private GameObject buttonPrefab;     // ボタンのプレハブ
    [SerializeField] private RectTransform cursor;        // カーソル

    private List<BallSelectButton> buttons = new List<BallSelectButton>();
    private int currentIndex = 0;

    private void OnEnable()
    {
     
    }

    void Start()
    {
        LoadBallData();
        if (buttons.Count > 0)
        {
            UpdateCursor();
        }
    }

    private void LoadBallData()
    {
        BallData[] allBalls = Resources.LoadAll<BallData>("Balls");

        float startX = 100f;   //並べ始める基準X座標
        float startY = 100f;      //基準Y座標
        float spacing = 200f;   //ボタン間の間隔（幅）

        for (int i = 0; i < allBalls.Length; i++)
        {
            var ball = allBalls[i];
            var obj = Instantiate(buttonPrefab, contentParent);
            if (obj == null)
            {
                Debug.LogError("buttonPrefab が null です！");
                continue;
            }

            var btn = obj.GetComponent<BallSelectButton>();
            if (btn == null)
            {
                Debug.LogError($"{obj.name} に BallSelectButton がありません！");
                continue;
            }

            btn.Setup(ball);
            buttons.Add(btn);

            //=== 座標を調整 ===
            RectTransform rt = obj.GetComponent<RectTransform>();
            if (rt != null)
            {
                // 横並びに配置
                rt.anchoredPosition = new Vector2(startX + i * spacing, startY);
            }
        }
    }

    private void Update()
    {
        if (BowlingUIManager.Instance.CurrentPhase != BowlingPhase.BallSelect)
            return;

        //左右入力（キーボード or コントローラー想定）
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = (currentIndex + 1) % buttons.Count;
            UpdateCursor();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex = (currentIndex - 1 + buttons.Count) % buttons.Count;
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
        if (buttons.Count == 0) return;
        cursor.position = buttons[currentIndex].transform.position;
    }

    private void SelectBall()
    {
        BallData selected = buttons[currentIndex].GetBallData();

        Debug.Log($"{selected.ballName} を選択しました！");

        //BallData を直接渡す
        BallManager.Instance.SelectBall(selected);

        //フェーズを投球へ移行
        BowlingUIManager.Instance.SetPhase(BowlingPhase.Throw);
    }
}
