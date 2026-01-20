using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class TitlePlayerMove : MonoBehaviour
{
    //ロボットの進むスピード
    private float speed = 5.0f;

    //アニメーション管理用のコンポーネント変数
    private Animator anim;
    private AnimatorStateInfo state;
    //一度だけ再生したい
    public bool MoveStopFlag = false;
    public bool EffectStartFlag = false;
    
    private TMP_Text text;
    private float text_alph; //テキストの透明度
    private string currentStateName;

    void Start()
    {
        //データ取得
        anim = GetComponent<Animator>();
        state = anim.GetCurrentAnimatorStateInfo(0);
        //anim = this.gameObject.transform.GetComponent<Animator>();
    }

    void Update()
    {
        //左に進んでいる・プレイヤーの前方に進む
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // キャラの進行方向に移動する
        transform.position += speed * Time.deltaTime * transform.forward;

        // X座標がxxになったら停止
        if (transform.position.x <= 4.0f && !MoveStopFlag)
        {
            speed = 0;
            anim.SetBool("Stop", true); // アニメーション切り替え
            Debug.Log("アニメーション開始");

            //一回だけ来るように
            MoveStopFlag = true;
        }


        //8_beamShotが終了したか確認
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("アーマチュア|8_baemShot") && !EffectStartFlag)
        {
            Debug.Log("特定のアニメーション終了");

            // Attack アニメーションが1回再生し終わった
            EffectStartFlag = true;
        }

    }
    
    public bool GetStopFlag()
    {
        return EffectStartFlag;
    }
    
}

/*
 //if (anim.GetCurrentAnimatorStateInfo(0).IsName("アーマチュア|2_move"))
        //{
        //    currentStateName = "2_move";
        //}
        //else if (anim.GetCurrentAnimatorStateInfo(0).IsName("アーマチュア|7_baem"))
        //{
        //    currentStateName = "7_baem";
        //}
        //else if (anim.GetCurrentAnimatorStateInfo(0).IsName("アーマチュア|8_baemShot"))
        //{
        //    currentStateName = "8_baemShot";
        //}
        //Debug.Log(currentStateName);
 
 
 */