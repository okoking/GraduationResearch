using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BallSelectButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;

    private BallData ballData;

    public void Setup(BallData data)
    {
        ballData = data;
        iconImage.sprite = data.icon;
        nameText.text = data.ballName;
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
