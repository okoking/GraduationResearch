using UnityEngine;

public class BowlingThrow : MonoBehaviour
{
    void Start()
    {
        var ball = BallManager.Instance.selectedBall;
        Debug.Log($"投球に使うボール: {ball.name}, 重さ: {ball.weight}");
    }

    public void Throw()
    {
        
    }
}