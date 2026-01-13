using UnityEngine;
using UnityEngine.VFX;

public class TitlePlayerMove : MonoBehaviour
{
    //ロボットの進むスピード
    float speed = 5.0f;

    //アニメーション管理用のコンポーネント変数
    private Animator anim;

    //一度だけ再生したい
    bool kariStop = false;

    //public VisualEffect currentVFX;


    void Start()
    {
        //データ取得
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //左に進んでいる・プレイヤーの前方に進む
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // キャラの進行方向に移動する
        transform.position += speed * Time.deltaTime * transform.forward;

        // X座標が0になったら停止
        if (transform.position.x <= 0.0f && !kariStop)
        {
            speed = 0;
            anim.SetBool("Stop", true); // アニメーション切り替え
            Debug.Log("アニメーション開始");

            kariStop = true;
        }

    }
}
