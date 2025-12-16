using Unity.VisualScripting;
using UnityEngine;

public class BossHp : MonoBehaviour
{

    public int maxHp;       //最大体力
    private int currentHp;  //現在体力

    public bool isPerfectInvincible;    //完全に無敵
    public bool isInvicible;            //無敵

    public float maxInvincibleTime;     //最大無敵時間
    private float invincibleTime;       //無敵時間

    private bool isDeath;               //死んでいるか

    public bool GetIsPerfectInvincible() {  return isPerfectInvincible; }
    public void SetIsPerfectInvincible(bool flg) { isPerfectInvincible = flg; }

    public bool GetIsInvicible() { return isInvicible; }
    public bool GetIsDeath() { return isDeath; }

    //Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = maxHp;
        isDeath = false;
    }

    //Update is called once per frame
    void Update()
    {
        if (isInvicible)    //無敵中であれば時間をカウント
        {
            invincibleTime += Time.deltaTime;
        }

        if (invincibleTime > maxInvincibleTime) //無敵時間を超えたら無敵解除
        {
            invincibleTime = 0f;
            isInvicible = false;
        }

        if(currentHp <= 0)  //0以下になったら殺す
        {
            isDeath = true;
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int hp)
    {
        if (isPerfectInvincible) return;    //完全無敵であればダメージ無効

        if (!isInvicible)                   //無敵でなければダメージが通る
        {
            currentHp -= hp;
            Debug.Log(currentHp);
            isInvicible = true;
        }
    }

    public void Recovery(int hp)
    {
        currentHp += hp;

        if (currentHp >= maxHp)  //最大体力を超えないようにする
        {
            currentHp = maxHp;
            Debug.Log(currentHp);
        }
    }
}
