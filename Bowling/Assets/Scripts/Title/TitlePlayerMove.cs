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

    const float STOP_POSX = 4.0f;


    //ショットポーズで待機する時間
    private int StopTime = 800;
    const int STOP_TIME = 800;

    //全体のサイクルの時間・アニメーション再開
    int CycleTime = 0;
    const int CYCLE_TIME = 3000;
    bool OneCycleFlag = false;



    void Start()
    {
        //データ取得
        anim = GetComponent<Animator>();
        state = anim.GetCurrentAnimatorStateInfo(0);
        //anim = this.gameObject.transform.GetComponent<Animator>();

        CycleTime = CYCLE_TIME;
        StopTime = STOP_TIME;

    }

    void Update()
    {

        Walk();

        ShotAnimStart();

        //エフェクト再生開始
        StartEffect();
        //ポーズ停止
        PoseStop();

        ResetCycle();
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
        // X座標がxxになったら停止
        if (transform.position.x <= STOP_POSX && !MoveStopFlag)
        {
            speed = 0;
            anim.SetBool("Stop", true); // アニメーション切り替え
            Debug.Log("アニメーション開始");

            //一回だけ来るように
            MoveStopFlag = true;
        }
    }

    //エフェクト再生開始
    void StartEffect()
    {
        //8_beamShotが終了したか確認
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("アーマチュア|8_baemShot") && !EffectStartFlag)
        {
            Debug.Log("特定のアニメーション終了");

            // Attack アニメーションが1回再生し終わった
            EffectStartFlag = true;

            //アニメーション停止
            anim.speed = 0;
        }
    }

    //ポーズ停止
    void PoseStop()
    {
        //撃つポーズで停止中
        if (EffectStartFlag)
        {
            StopTime--;
            if (StopTime < 0)
            {
                //アニメーション再開
                anim.speed = 1f;

                OneCycleFlag = true;
            }
        }
    }

    void ResetCycle()
    {
        if(OneCycleFlag)
        {
            CycleTime--;
        }
        //ショットアニメーションを再開させたい
        //全体的なサイクル・リセット
        if (CycleTime < 0)
        {
            MoveStopFlag = false;
            EffectStartFlag = false;
            OneCycleFlag = false;

            anim.SetBool("Reset", true); // アニメーション再開


            CycleTime = CYCLE_TIME;
            StopTime = STOP_TIME;

            Debug.Log("リセット");

        }
    }
}

