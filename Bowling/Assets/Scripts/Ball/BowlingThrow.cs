using UnityEngine;

public class BowlingThrow : MonoBehaviour
{
    private BallData ball;
    public void Init(BallData selected)
    {
        ball = selected;
        Debug.Log($"�����Ɏg���{�[��: {ball.ballName}, �d��: {ball.weight}");
    }

    void Start()
    {
        //ball = BallManager.Instance.selectedBall;
        //if (ball == null)
        //{
        //    Debug.LogWarning("�܂��{�[�����I������Ă��܂���");
        //    return;
        //}
        //Debug.Log($"�����Ɏg���{�[��: {ball.name}, �d��: {ball.weight}");
    }

    public void Throw()
    {
        if (ball == null)
        {
            Debug.LogWarning("�{�[�������I���̂܂ܓ����悤�Ƃ��܂���");
            return;
        }

        //�����鏈��
        Debug.Log($"�{�[�� {ball.ballName} �𓊂��܂����I");
    }
}