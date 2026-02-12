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
    [SerializeField]
    float targetRadius = 1.5f;   // プレイヤー周囲の半径

    Vector3 aimPoint;            // 実際の着弾地点

    [SerializeField]
    GameObject warningMarkerPrefab;

    GameObject warningMarker;

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

        //着弾地点を1回だけ決定
        if (target != null)
        {
            Vector2 rand = Random.insideUnitCircle * targetRadius;
            aimPoint = target.position + new Vector3(rand.x, 0f, rand.y);
            aimPoint.y = 0.01f;
            //着弾予告マーカー生成
            warningMarker = Instantiate(
                warningMarkerPrefab,
                aimPoint,
                Quaternion.Euler(90f, 0f, 0f)
            );
        }

        homingTimer = homingDuration;

        //サウンドを再生
        SoundManager.Instance.Request("MissileShot", gameObject.transform.localPosition);
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
                    * (aimPoint - position - time * velocity);

                if (limitAcceleration && acceleration.sqrMagnitude > maxAcceleration * maxAcceleration)
                {
                    acceleration = acceleration.normalized * maxAcceleration;
                }

                velocity += acceleration * Time.deltaTime;
            }
        }

        //追跡が切れても「速度」はそのまま使う
        position += velocity * Time.deltaTime;
        thisTransform.position = position;

        if (velocity.sqrMagnitude > 0.001f)
        {
            thisTransform.rotation = Quaternion.LookRotation(velocity);
        }

        float t = 1f - (homingTimer / homingDuration);
        float scale = Mathf.Lerp(0.7f, 0.5f, t);
        warningMarker.transform.localScale = Vector3.one * scale;

        
    }

    void OnTriggerEnter(Collider other)
    {
        //サウンドを再生
        SoundManager.Instance.Request("MissileImpact", gameObject.transform.localPosition);

        //ターゲットに当たった
        if (other.gameObject.CompareTag("Player"))
        {
            if (warningMarker != null)
                Destroy(warningMarker);

            Destroy(gameObject);
            target.gameObject.GetComponent<PlayerHealth>().TakeDamage(20);
            EffectManager.instance.Play("MissileHit", gameObject.transform.position);
            return;
        }

        //壁・地形など
        if (other.gameObject.CompareTag("Ground"))
        {
            if (warningMarker != null)
                Destroy(warningMarker);

            Destroy(gameObject);
            EffectManager.instance.Play("MissileHit", gameObject.transform.position);
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
