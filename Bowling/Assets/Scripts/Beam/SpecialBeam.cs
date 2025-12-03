using UnityEngine;
using System.Collections;
using UnityEngine.VFX;

public class SpecialBeam : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public VisualEffect vfxPrefab;      //エフェクト
    public Transform    player;         //プレイヤーの情報
    private bool        isActive;       //エフェクトが活動中か
    private int         activeTime;     //エフェクトの生存時間

    void Start()
    {
        isActive = false;
        activeTime = 0;
    }

    void Update()
    {
        // ボタン押したらビーム発射＆活動中でないなら
        if (Input.GetKeyDown("joystick button 5") && !isActive)
        {
            //ビームエフェクト再生
            ShotSpecialBeam();
            isActive = true;
        }

        BeamLimt();
    }

    //ビームエフェクト再生
    public void ShotSpecialBeam()
    {
        //プレイヤーの座標を渡す・エフェクト生成
        var vfx = Instantiate(
            vfxPrefab,
            player.position,
            player.rotation
        );

        //プレイヤーに追従する
        vfx.transform.SetParent(player);
        //Y軸方向に値をプラス（プレイヤーの肩当たり）
        vfx.transform.localPosition = new Vector3(0, 1.0f, 0);

        //再生の合図
        vfxPrefab.SendEvent("OnPlay");
        Debug.Log("Beam!");
    }

    //撃てる回数を制限
    public void BeamLimt()
    {
        //ここはビームのゲージがたまったら使えるようにする
        //以下は仮の再起機能
        if (isActive)
        {
            activeTime++;

            if (activeTime > 1000)
            {
                activeTime = 0;
                isActive = false;
                Debug.Log("ビーム活動終了!");
            }

            
        }
    }
}
