using UnityEngine;
using UnityEngine.UI;

public class BallSelectManager : MonoBehaviour
{
    [SerializeField] private Image[] ballIcons;      //�{�[���̃A�C�R��UI
    [SerializeField] private BallData[] ballDatas;   //�Ή�����{�[���f�[�^
    [SerializeField] private RectTransform cursor;   //�J�[�\���i�n�C���C�g�p�j

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

        //���E���́i�L�[�{�[�h or �R���g���[���[�z��j
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

        //����
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            SelectBall();
        }
    }
    private void UpdateCursor()
    {
        //�J�[�\����I�𒆂̃A�C�R���̈ʒu�Ɉړ�
        cursor.position = ballIcons[currentIndex].transform.position;
    }

    private void SelectBall()
    {
        BallData selected = ballDatas[currentIndex];

        Debug.Log($"{selected.ballName} ��I�����܂����I");

        //BallData �𒼐ړn��
        BallManager.Instance.SelectBall(selected);

        //�t�F�[�Y�𓊋��ֈڍs
        BowlingUIManager.Instance.SetPhase(BowlingPhase.Throw);
    }
}
