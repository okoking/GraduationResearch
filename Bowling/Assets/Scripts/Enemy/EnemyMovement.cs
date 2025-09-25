using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Transform[] waypoints; // 通るオブジェクト（順番）
    [SerializeField] float speed = 2f;      // 移動速度
    [SerializeField] float arriveDistance = 0.1f; // 到着判定の距離

    int currentIndex = 0; // 今向かっているウェイポイント

    void Update()
    {
        if (waypoints.Length == 0) return;

        // 今の目標
        Transform target = waypoints[currentIndex];

        // ターゲットの方向
        Vector3 dir = (target.position - transform.position).normalized;

        // 移動
        transform.position += dir * speed * Time.deltaTime;

        // 近づいたら次へ
        if (Vector3.Distance(transform.position, target.position) < arriveDistance)
        {
            currentIndex++;
            if (currentIndex >= waypoints.Length)
            {
                currentIndex = 0; // 最初に戻る
            }
        }
    }
}
