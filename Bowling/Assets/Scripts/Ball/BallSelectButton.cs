using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BallSelectButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text ThrowNumText;

    private BallData ballData;

    private void Awake()
    {
        if (iconImage == null)
            iconImage = GetComponentInChildren<Image>();

        if (nameText == null)
            nameText = GetComponentInChildren<TMP_Text>();

        if (ThrowNumText == null)
            ThrowNumText = GetComponentInChildren<TMP_Text>();
    }

    public void Setup(BallData data)
    {
        if (data == null)
        {
            Debug.LogError("Setup �ɓn���ꂽ BallData �� null �ł��I");
            return;
        }

        ballData = data;

        if (iconImage == null || nameText == null)
        {
            Debug.LogError("Prefab �̎Q�Ƃ� Inspector �Ŗ��ݒ�ł��I");
            return;
        }

        if (data.icon == null)
        {
            Debug.LogWarning($"{data.ballName} �̃A�C�R�����ݒ肳��Ă��܂���I");
        }

        iconImage.sprite = data.icon;
        nameText.text = data.ballName;
        ThrowNumText.text = data.ThrowNum.ToString();

        Debug.Log($"�{�^������: {data.ballName}, �A�C�R��={(data.icon != null ? data.icon.name : "�Ȃ�")}, �e�L�X�g={nameText.text}, ��={ThrowNumText.text}");
    }

    public BallData GetBallData() => ballData;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
