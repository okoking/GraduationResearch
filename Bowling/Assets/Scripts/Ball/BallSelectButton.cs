using UnityEngine;
using UnityEngine.UI;

public class BallSelectButton : MonoBehaviour
{
    [SerializeField] private BallData ballData;
    [SerializeField] private BallManager uiManager;
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        uiManager.SelectBall(ballData);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
