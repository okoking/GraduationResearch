using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BallSelectButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;

    private BallData ballData;

    private void Awake()
    {
        if (iconImage == null)
            iconImage = GetComponentInChildren<Image>();

        if (nameText == null)
            nameText = GetComponentInChildren<TMP_Text>();
    }

    public void Setup(BallData data)
    {
        if (data == null)
        {
            Debug.LogError("Setup に渡された BallData が null です！");
            return;
        }

        ballData = data;

        if (iconImage == null || nameText == null)
        {
            Debug.LogError("Prefab の参照が Inspector で未設定です！");
            return;
        }

        if (data.icon == null)
        {
            Debug.LogWarning($"{data.ballName} のアイコンが設定されていません！");
        }

        iconImage.sprite = data.icon;
        nameText.text = data.ballName;

        Debug.Log($"ボタン生成: {data.ballName}, アイコン={(data.icon != null ? data.icon.name : "なし")}, テキスト={nameText.text}");
    }

    public BallData GetBallData() => ballData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
