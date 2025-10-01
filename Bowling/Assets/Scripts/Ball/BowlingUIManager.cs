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

    [SerializeField] private GameObject ballSelectPanel;  //�{�[���I��pUI
    //[SerializeField] private GameObject throwPanel;     //�����pUI
     [SerializeField] private TMP_Text messageText;       //��ʒ����̃��b�Z�[�W

    private BowlingPhase currentPhase;                    //�����Ő���
    public BowlingPhase CurrentPhase => currentPhase;     //�O���͓ǂݎ���p

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        SetPhase(BowlingPhase.BallSelect); //�ŏ��̓{�[���I��
    }

    public void SetPhase(BowlingPhase phase)
    {
        currentPhase = phase;

        //UI�؂�ւ�
        ballSelectPanel.SetActive(phase == BowlingPhase.BallSelect);

        switch (phase)
        {
            case BowlingPhase.BallSelect:
                messageText.text = "�{�[����I�����Ă�������";
                break;
            case BowlingPhase.Throw:
                messageText.text = "�X�y�[�X�L�[�܂���Enter�œ����܂�";
                break;
            default:
                messageText.text = "";
                break;
        }
    }
}