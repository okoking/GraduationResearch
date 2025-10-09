using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

//�{�[���}�l�[�W���[
public class BallManager : MonoBehaviour
{
    public static BallManager Instance { get; private set; }
    public List<BallData> allBalls { get; private set; } = new List<BallData>();    //�o�^���Ă���S�{�[��
    public BallData selectedBall { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); //�d���h�~
            return;
        }
        Instance = this;
        
        //Resources/Balls �t�H���_���� BallData ��S�ēǂݍ���
        allBalls.AddRange(Resources.LoadAll<BallData>("Balls"));
        Debug.Log($"���[�h�����{�[����: {allBalls.Count}");
    }

    public void SelectBall(BallData ball)
    {
        selectedBall = ball;
        Debug.Log($"�I�񂾃{�[��: {ball.ballName}");

        FindFirstObjectByType<BowlingThrow>()?.Init(ball);
        // �{�[������������
        GameObject ballObj = GameObject.Find("BallShootManager");
        BallSpawner ballSpawner = ballObj.GetComponent<BallSpawner>();
        ballSpawner.Spawn(ball.ballName);

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
