using UnityEngine;

public class BowlingThrow : MonoBehaviour
{
    private BallData ball;
    public void Init(BallData selected)
    {
        ball = selected;
        Debug.Log($"投球に使うボール: {ball.ballName}, 重さ: {ball.weight}");
    }

    void Start()
    {
        //ball = BallManager.Instance.selectedBall;
        //if (ball == null)
        //{
        //    Debug.LogWarning("まだボールが選択されていません");
        //    return;
        //}
        //Debug.Log($"投球に使うボール: {ball.name}, 重さ: {ball.weight}");
    }

    public void Throw()
    {
        if (ball == null)
        {
            Debug.LogWarning("ボールが未選択のまま投げようとしました");
            return;
        }

        //投げる処理
        Debug.Log($"ボール {ball.ballName} を投げました！");
    }
}