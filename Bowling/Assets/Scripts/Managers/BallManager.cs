using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

//ボールマネージャー
public class BallManager : MonoBehaviour
{
    public static BallManager Instance { get; private set; }
    public List<BallData> allBalls { get; private set; } = new List<BallData>();    //登録してある全ボール
    public BallData selectedBall { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); //重複防止
            return;
        }
        Instance = this;
        
        //Resources/Balls フォルダ内の BallData を全て読み込み
        allBalls.AddRange(Resources.LoadAll<BallData>("Balls"));
        Debug.Log($"ロードしたボール数: {allBalls.Count}");
    }

    public void SelectBall(BallData ball)
    {
        selectedBall = ball;
        Debug.Log($"選んだボール: {ball.ballName}");

        FindFirstObjectByType<BowlingThrow>()?.Init(ball);
        // ボールを召喚する
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
