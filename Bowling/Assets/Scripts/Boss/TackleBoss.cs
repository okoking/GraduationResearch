using UnityEngine;

public class TackleBoss : MonoBehaviour
{
    public float speed = 3f;               //通常速度
    public float tackleSpeed = 10f;        //タックル速度
    public float stopDistance = 1.5f;      //どこまで近づくか
    public float tackleDuration = 1.0f;    //タックルの持続時間
    public float stunTime = 2.0f;          //スタン時間
    public float tackleCooldown = 3.0f;    //タックル再使用までの時間
    public float warnTime = 1.0f;          //タックル前の警告時間

    private Transform target;
    private bool isTackling = false;    //タックル中か
    private bool isStunned = false;     //スタン中か
    private bool isOnCooldown = false;  //クールタイム中か
    private bool isWarning = false;     //予兆中フラグ

    private float tackleTimer = 0f;
    private float stunTimer = 0f;
    private float cooldownTimer = 0f;
    private float warnTimer = 0f;

    private LineRenderer line; //予測線表示用

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;
        else
            Debug.LogWarning("Playerタグを持つオブジェクトが見つかりませんでした");

        //LineRendererの設定
        line = gameObject.AddComponent<LineRenderer>();
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = Color.red;
        line.endColor = Color.red;
        line.enabled = false;
    }

    void Update()
    {
        if (target == null) return;

        //スタン中
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0) isStunned = false;
            return;
        }

        //クールタイム中
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0) isOnCooldown = false;
        }

        //予兆中
        if (isWarning)
        {
            warnTimer -= Time.deltaTime;
            if (warnTimer <= 0)
            {
                line.enabled = false;
                //警告を出したらタックル
                StartTackle();
            }
            return;
        }

        //タックル中
        if (isTackling)
        {
            tackleTimer -= Time.deltaTime;

            Vector3 moveDir = transform.forward;
            moveDir.y = 0;
            transform.position += moveDir.normalized * tackleSpeed * Time.deltaTime;

            if (tackleTimer <= 0)
                EndTackle();

            return;
        }

        //通常追跡
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > stopDistance)
        {
            Vector3 lookPos = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.LookAt(lookPos);

            Vector3 nextPos = Vector3.MoveTowards(transform.position, lookPos, speed * Time.deltaTime);
            nextPos.y = transform.position.y;
            transform.position = nextPos;
        }
        else
        {
            if (!isOnCooldown && !isWarning)
                StartWarning();
        }
    }

    //予兆フェーズ
    void StartWarning()
    {
        isWarning = true;
        warnTimer = warnTime;
        isOnCooldown = true;
        cooldownTimer = tackleCooldown;

        // 向きを固定
        Vector3 lookPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(lookPos);

        // 予測線を出す
        line.enabled = true;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + transform.forward * 10f); //長さ10mの赤線

        Debug.Log("タックル予兆中！");
    }

    //タックル開始
    void StartTackle()
    {
        isWarning = false;
        isTackling = true;
        tackleTimer = tackleDuration;
        Debug.Log("タックル開始！");
    }

    //タックル終了
    void EndTackle()
    {
        isTackling = false;
        Debug.Log("タックル終了");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTackling)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("プレイヤーに命中！");
                //ダメージ処理
            }
            else
            {
                Debug.Log("壁などに衝突 → スタン");
                StartStun();
            }
            EndTackle();
        }
    }

    void StartStun()
    {
        isStunned = true;
        stunTimer = stunTime;
        Debug.Log("スタン状態");
    }
}