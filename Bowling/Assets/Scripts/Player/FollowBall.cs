using UnityEngine;

public class FollowBall : MonoBehaviour
{
    public Transform target;     // ボール（追従対象）
    public Vector3 offset;       // 相対位置（例: (0, 5, -10)）

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LateUpdate()
    {
        if (target == null) return;

        // ボールの位置 + オフセット にカメラを移動
        transform.position = target.position + offset;

        // 常にボールの方向を向く
        transform.LookAt(target);
    }
}
