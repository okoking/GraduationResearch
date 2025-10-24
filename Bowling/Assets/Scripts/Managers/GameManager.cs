using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartPlayPhase()
    {
        Debug.Log("[GameManager] プレイフェーズ開始！");
        // ボールをセットする処理など
        // 例: BowlingThrow.Instance.ResetBall();
       
    }
}
