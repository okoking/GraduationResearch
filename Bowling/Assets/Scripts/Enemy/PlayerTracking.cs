using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerTracking : MonoBehaviour
{
    //移動速度
    public float speed = 3f;

    //プレイヤーにどこまで近づくか,攻撃可能にするか
    public float stopDistance = 1.5f;

    //追跡対象(プレイヤー)
    Transform target;

    //プレイヤーが指定範囲内にいるか
    bool withinRange = false;

    void Start()
    {
        //プレイヤータグを持つオブジェクトを探す
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Playerタグを持つオブジェクトが見つかりませんでした");
        }
    }

    void Update()
    {
        if (target == null) return;

        //距離
        float distance = Vector3.Distance(transform.position, target.position);
        //一定距離に近づくまで追跡
        if (distance > stopDistance)
        {
            //対象(プレイヤー)の方向を見続ける
            transform.LookAt(target);
            //進行方向に進む
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            //入ってない
            withinRange = false;
        }
        //そうでなければ
        else
        {
            //入っている
            withinRange = true;
        }
    }

    public bool GetRange()
    {
        return withinRange;
    }
}
