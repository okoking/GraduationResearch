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
        Debug.Log("[GameManager] �v���C�t�F�[�Y�J�n�I");
        // �{�[�����Z�b�g���鏈���Ȃ�
        // ��: BowlingThrow.Instance.ResetBall();
       
    }
}
