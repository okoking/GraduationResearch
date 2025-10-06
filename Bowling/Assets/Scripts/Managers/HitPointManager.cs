using UnityEngine;

public class HitPointManager : MonoBehaviour
{

    public  int  MaxHp;      //最大HP
    private int  currentHp;  //現在のHP
    private bool isActive;   //生存判定(falseで死亡)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = MaxHp;  //最初は最大HPから始まる
        isActive = true;    //生存している
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ダメージを受ける処理
    public void TakeDamage(int damage)
    {
        //もし生きていなければ実行しない
        if (!isActive) return;
        //与えるダメージは負の値はダメ
        if (damage < 0) damage = 0;
        //ダメージを受ける
        currentHp -= damage;
        //HPが0以下になったら
        if (currentHp <= 0)
        {
            //0以下にはならない
            currentHp = 0;
            //死//
            Die();
        }
    }

    //死亡処理
    private void Die()
    {
        isActive = false;   //死んだことにする
        Destroy(gameObject);//オブジェクトを消す
    }

    
    public int GetCurrentHp() {  return currentHp; }
}
