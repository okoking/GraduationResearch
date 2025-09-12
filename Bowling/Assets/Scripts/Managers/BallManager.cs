using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "BallData", menuName = "Game/BallData")]
public class BallData : ScriptableObject
{
    public string ballName;
    public Sprite icon;
    public float weight;
    public float speed;
}

//�{�[���}�l�[�W���[
public class BallManager : MonoBehaviour
{
    public static BallManager Instance;
    [SerializeField] public List<BallData> allBalls;     //�o�^���Ă���S�{�[��
    public BallData selectedBall { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void SelectBall(BallData ball)
    {
        selectedBall = ball;
        Debug.Log($"�I�񂾃{�[��: {ball.ballName}");
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
