using UnityEngine;
using UnityEngine.VFX;

public class TitleBeamStart : MonoBehaviour
{
    public VisualEffect currentVFX;
    bool FirstFlag = false;

    [SerializeField] private TitlePlayerMove player;

    void Start()
    {
        //始まったときに再生されないように
        currentVFX.Stop();
    }

    void Update()
    {
        //モデルが定位置に着いたか確認
        if (player.GetStopFlag() && !FirstFlag)
        {
            //エフェクト再生
            currentVFX.Play();
            Debug.Log("3:エフェクト開始");

            //一回だけ再生されるように
            FirstFlag = true;
        }
    }
}
