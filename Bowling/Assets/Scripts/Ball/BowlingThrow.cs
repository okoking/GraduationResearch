using UnityEngine;

public class BowlingThrow : MonoBehaviour
{
    void Start()
    {
        var ball = BallManager.Instance.selectedBall;
        Debug.Log($"�����Ɏg���{�[��: {ball.name}, �d��: {ball.weight}");
    }

    public void Throw()
    {
        
    }
}