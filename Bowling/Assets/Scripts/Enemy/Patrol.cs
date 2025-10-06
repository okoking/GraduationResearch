using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] Transform target; // 追いかけたいオブジェクト
    [SerializeField] Vector3 offset = new Vector3(0, 5, -10); // カメラの位置補正
    [SerializeField] float smoothSpeed = 5f; // 補間の速さ

    void LateUpdate()
    {
        if (target == null) return;

        // 目標位置（ターゲットの位置＋オフセット）
        Vector3 desiredPosition = target.position + offset;

        // 補間してスムーズに追従
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;

        // 常にターゲットを注視
        transform.LookAt(target);
    }
}
