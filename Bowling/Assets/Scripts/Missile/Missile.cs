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
        //thisTransform = transform;
        //position = thisTransform.position;
        ////球の軌道の初期振れ幅
        //velocity = new Vector3(Random.Range(minInitVelocity.x, maxInitVelocity.x), Random.Range(minInitVelocity.y, maxInitVelocity.y), Random.Range(minInitVelocity.z, maxInitVelocity.z));
        //target = FindRandomTarget();

        //StartCoroutine(nameof(Timer));

        thisTransform = transform;
        position = thisTransform.position;

        velocity = new Vector3(
            Random.Range(minInitVelocity.x, maxInitVelocity.x),
            Random.Range(minInitVelocity.y, maxInitVelocity.y),
            Random.Range(minInitVelocity.z, maxInitVelocity.z)
        );

        // ★ すでに設定されていなければ探す
        if (target == null)
        {
            target = FindRandomTarget();
        }

        StartCoroutine(nameof(Timer));

        currentTime = timeToTarget;
    }

    public void Update()
    {
        //ターゲットがなければミサイルを壊す
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        //加速度を計算
        acceleration = 2f / (time * time) * (target.position - position - time * velocity);

        if (limitAcceleration && acceleration.sqrMagnitude > maxAcceleration * maxAcceleration)
        {
            acceleration = acceleration.normalized * maxAcceleration;
        }

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            return;
        }

        velocity += acceleration * Time.deltaTime;
        position += velocity * Time.deltaTime;
        thisTransform.position = position;
        thisTransform.rotation = Quaternion.LookRotation(velocity);
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(lifeTime);

        Destroy(gameObject);
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
