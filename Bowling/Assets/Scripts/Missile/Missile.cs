using System.Collections;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    //ターゲット
    Transform target;
    [SerializeField, Min(0)]
    //何秒で着弾させるか
    float time = 1;
    [SerializeField]
    //生存可能時間
    float lifeTime = 2;
    [SerializeField]
    bool limitAcceleration = false;
    [SerializeField, Min(0)]
    float maxAcceleration = 100;
    [SerializeField]
    //最小振れ幅
    Vector3 minInitVelocity;
    [SerializeField]
    //最大振れ幅
    Vector3 maxInitVelocity;
    [SerializeField, Min(0)]
    float homingDuration = 0.5f; // 何秒追跡するか

    float homingTimer;
    bool isHoming = true;

    [SerializeField, Min(0)]
    float timeToTarget = 1f;

    float currentTime;

    Vector3 position;
    Vector3 velocity;
    Vector3 acceleration;
    Transform thisTransform;

    public Transform Target
    {
        set
        {
            target = value;
        }
        get
        {
            return target;
        }
    }

    void Start()
    {
        thisTransform = transform;
        position = thisTransform.position;

        velocity = new Vector3(
            Random.Range(minInitVelocity.x, maxInitVelocity.x),
            Random.Range(minInitVelocity.y, maxInitVelocity.y),
            Random.Range(minInitVelocity.z, maxInitVelocity.z)
        );

        if (target == null)
        {
            target = FindRandomTarget();
        }

        homingTimer = homingDuration;
    }

    public void Update()
    {
        // 追跡中でターゲットが消えたら破壊
        if (isHoming && target == null)
        {
            Destroy(gameObject);
            return;
        }

        if (isHoming)
        {
            // ホーミング時間を減らす
            homingTimer -= Time.deltaTime;
            if (homingTimer <= 0f)
            {
                isHoming = false; // ★追跡終了
            }
            else
            {
                // ホーミング中のみ加速度を更新
                acceleration = 2f / (time * time)
                    * (target.position - position - time * velocity);

                if (limitAcceleration && acceleration.sqrMagnitude > maxAcceleration * maxAcceleration)
                {
                    acceleration = acceleration.normalized * maxAcceleration;
                }

                velocity += acceleration * Time.deltaTime;
            }
        }

        // ★追跡が切れても「速度」はそのまま使う
        position += velocity * Time.deltaTime;
        thisTransform.position = position;

        if (velocity.sqrMagnitude > 0.001f)
        {
            thisTransform.rotation = Quaternion.LookRotation(velocity);
        }
    }

    void OnTriggerEnter(Collider other)
    {

        // ターゲットに当たった
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            target.gameObject.GetComponent<PlayerHealth>().TakeDamage(20);
            EffectManager.instance.Play("SmallExplosion", gameObject.transform.position);
            return;
        }

        // 壁・地形など
        if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
            EffectManager.instance.Play("SmallExplosion", gameObject.transform.position);
            return;
        }

        
    }

    Transform FindRandomTarget()
    {
        //敵のタグが付いたオブジェクトを探して格納
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
        //ターゲットとの距離が0ならnullを返す
        if (targets.Length == 0) return null;
        //ランダムなターゲットのトランスフォームを返す
        return targets[Random.Range(0, targets.Length)].transform;
    }
}
