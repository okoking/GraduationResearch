using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //攻撃可能か
    bool isAttack = true;

    //攻撃後のクールタイム
    public float coolTime = 1200f;

    //クールタイムカウント用
    float timeCnt;

    PlayerTracking playerTracking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTracking = FindAnyObjectByType<PlayerTracking>();
    }

    // Update is called once per frame
    void Update()
    {
        //攻撃可能でプレイヤー範囲内にいれば
        if (isAttack && playerTracking.GetRange())
        {
            //攻撃する
            Debug.Log("攻撃です");
            isAttack = false;
        }
        //攻撃不可能であれば
        if (!isAttack)
        {
            //時間をカウント
            timeCnt++;
            //指定時間を超えると攻撃可能になる
            if (timeCnt >= coolTime)
            {
                Debug.Log("いけます!");
                isAttack = true;
                timeCnt = 0f;
            }
        }
    }
}
