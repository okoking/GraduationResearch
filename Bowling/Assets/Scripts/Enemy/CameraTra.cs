using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player; // プレイヤーのTransformを指定
    [SerializeField] Vector3 offset = new Vector3(0, 5, -10); // プレイヤーからの距離
    [SerializeField] float followSpeed = 5f; // 追従スピード

    void LateUpdate()
    {
        if (player == null) return;

        // 目標位置を計算
        Vector3 targetPos = player.position + offset;

        // スムーズに追従
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);

        // プレイヤーの方向を見る
        transform.LookAt(player);
    }
}