using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class BallSelectManager : MonoBehaviour
{
    [SerializeField] private Transform contentParent;     // UI�z�u��
    [SerializeField] private GameObject buttonPrefab;     // �{�^���̃v���n�u
    [SerializeField] private RectTransform cursor;        // �J�[�\��

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

        float startX = 100f;   //���׎n�߂�X���W
        float startY = 100f;      //�Y���W
        float spacing = 200f;   //�{�^���Ԃ̊Ԋu�i���j

        for (int i = 0; i < allBalls.Length; i++)
        {
            var ball = allBalls[i];
            var obj = Instantiate(buttonPrefab, contentParent);
            if (obj == null)
            {
                Debug.LogError("buttonPrefab �� null �ł��I");
                continue;
            }

            var btn = obj.GetComponent<BallSelectButton>();
            if (btn == null)
            {
                Debug.LogError($"{obj.name} �� BallSelectButton ������܂���I");
                continue;
            }

            btn.Setup(ball);
            buttons.Add(btn);

            //=== ���W�𒲐� ===
            RectTransform rt = obj.GetComponent<RectTransform>();
            if (rt != null)
            {
                // �����тɔz�u
                rt.anchoredPosition = new Vector2(startX + i * spacing, startY);
            }
        }
    }

    private void Update()
    {
        if (BowlingUIManager.Instance.CurrentPhase != BowlingPhase.BallSelect)
            return;

        //���E���́i�L�[�{�[�h or �R���g���[���[�z��j
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

        //����
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            SelectBall();
        }

    }

    private void UpdateCursor()
    {
        //�J�[�\����I�𒆂̃A�C�R���̈ʒu�Ɉړ�
        if (buttons.Count == 0) return;
        cursor.position = buttons[currentIndex].transform.position;
    }

    private void SelectBall()
    {
        BallData selected = buttons[currentIndex].GetBallData();

        Debug.Log($"{selected.ballName} ��I�����܂����I");

        //BallData �𒼐ړn��
        BallManager.Instance.SelectBall(selected);

        //�t�F�[�Y�𓊋��ֈڍs
        BowlingUIManager.Instance.SetPhase(BowlingPhase.Throw);
    }
}
