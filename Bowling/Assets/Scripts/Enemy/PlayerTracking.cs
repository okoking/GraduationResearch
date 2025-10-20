using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerTracking : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float stopDistance = 1.5f;

    Transform target;

    bool withinRange = false;

    void Start()
    {

        // シーン上の「Player」タグを持つオブジェクトを探す
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

        float distance = Vector3.Distance(transform.position, target.position);
        //一定距離に近づくまで追跡
        if (distance > stopDistance)
        {
            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );
            withinRange = false;
        }
        //そうでなければ
        else
        {
            withinRange = true;
        }
    }

    public bool GetRange()
    {
        return withinRange;
    }
}
