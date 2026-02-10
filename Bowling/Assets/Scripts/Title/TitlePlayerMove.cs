using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class TitlePlayerMove : MonoBehaviour
{
    //モデルが停止する場所
    const float STOP_POSX = 4.0f;
    //ショットポーズで待機する時間
    private int StopTime = 800;
    const int STOP_TIME = 800;

    //ロボットの進むスピード
    private float speed = 5.0f;

    //アニメーション管理用のコンポーネント変数
    private Animator anim;
    private AnimatorStateInfo state;

    //一度だけ再生したい
    bool EffectStartFlag = false;
    
    

    //全体のサイクルの時間・アニメーション再開
    int CycleTime = 0;
    const int CYCLE_TIME = 3000;

    enum MoveStaging
    {
        WALK,
        SHOT_ANIM_START,
        EFFECT_START,
        POSE_STOP,
        RESET
    }
    MoveStaging tagMoveStaging;

    void Start()
    {
        //データ取得
        anim = GetComponent<Animator>();
        state = anim.GetCurrentAnimatorStateInfo(0);

        CycleTime = CYCLE_TIME;
        StopTime = STOP_TIME;

        tagMoveStaging = MoveStaging.WALK;
    }

    void Update()
    {
        switch (tagMoveStaging)
        { 
            case MoveStaging.WALK:
                Walk();

                //歩き終わった
                if (transform.position.x <= STOP_POSX)
                {
                    tagMoveStaging = MoveStaging.SHOT_ANIM_START;
                }
                break;

            case MoveStaging.SHOT_ANIM_START:

                //ショットアニメ開始
                ShotAnimStart();

                //ショットアニメ終了したか
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("アーマチュア|8_baemShot"))
                    tagMoveStaging = MoveStaging.EFFECT_START;

                break;
            case MoveStaging.EFFECT_START:

                EffectStart();
                tagMoveStaging = MoveStaging.POSE_STOP;

                break;
            case MoveStaging.POSE_STOP:
                //撃つポーズで停止中
                StopTime--;
                //エフェクトのフラグをおる
                EffectStartFlag = false;
                //停止時間が来たら
                if (StopTime < 0 )
                {
                    //アニメーション再開
                    anim.speed = 1f;

                    tagMoveStaging = MoveStaging.RESET;

                    Debug.Log("4:ポーズ");
                }
                break;

            case MoveStaging.RESET:
                CycleTime--;

                //ショットアニメーションを再開させたい
                //全体的なサイクル・リセット
                if (CycleTime < 0)
                {
                    EffectStartFlag = false;
                    anim.SetBool("Stop", false); //閉める
                    anim.SetBool("Reset", true); //開ける

                    CycleTime = CYCLE_TIME;
                    StopTime = STOP_TIME;

                    Debug.Log("5:リセット");
                    tagMoveStaging = MoveStaging.SHOT_ANIM_START;
                }
                break;
        
        }
        
    }
    
    public bool GetStopFlag()
    {
        return EffectStartFlag;
    }

    void Walk()
    {
        //左に進んでいる・プレイヤーの前方に進む
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // キャラの進行方向に移動する
        transform.position += speed * Time.deltaTime * transform.forward;
    }

    //ショットアニメーション開始関数
    void ShotAnimStart()
    {
        //ショットアニメ開始
        speed = 0;
        anim.SetBool("Stop", true);     // アニメーション開始・開ける
        anim.SetBool("Reset", false);   //閉める

        Debug.Log("1:開始");
    }

    //エフェクト再生開始
    void EffectStart()
    {
        // Attack アニメーションが1回再生し終わった
        EffectStartFlag = true;

        //アニメーション停止
        anim.speed = 0;
        Debug.Log("2:特定のアニメ終了");
    }
   
}

