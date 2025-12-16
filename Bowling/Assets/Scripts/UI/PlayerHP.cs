using TMPro;
using UnityEngine;
using UnityEngine.UI;


//HPバーの増減を行う
//HPの数値を表示する


public class PlayerHP : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Slider HpSlinder;   //バー
    [SerializeField] private TextMeshProUGUI textHP;            //数値で表示
    private PlayerHealth player;                                //プレイヤーの情報

    private int currentHp;  //現在のHP


    void Start()
    {
        player = GetComponent<PlayerHealth>();
        currentHp = player.GetHealth();

        //バー
        HpSlinder.value = currentHp;    //現在のHPを反映

        //数値
        textHP.text = currentHp.ToString();
    }

    void Update()
    {
        TakeDamage();
    }

    //ダメージ処理
    void TakeDamage()
    {
        currentHp = player.GetHealth();

        // スライダーに現在のHPを反映
        HpSlinder.value = currentHp;
        textHP.text = currentHp.ToString();
        //4桁で0埋めする
        textHP.text = currentHp.ToString("D4");
    }
    
}


/*
 * // HPを減らす処理
        // 仮：マウスをクリックした場合
        if (Input.GetMouseButtonDown(0))    
        {
            //仮：どれだけダメージが入るのかは今後する
            //TakeDamage(10);
        }
 * public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp < 0) currentHp = 0;

        // スライダーに現在のHPを反映
        HpSlinder.value = currentHp;
        textHP.text = currentHp.ToString();
        //4桁で0埋めする
        textHP.text = currentHp.ToString("D4");

        // HPが0になったときの処理
        if (currentHp == 0)
        {
        }
*/