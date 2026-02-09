using UnityEngine;
using UnityEngine.VFX;

public class TitleBeamStart : MonoBehaviour
{
    public VisualEffect currentVFX;
    bool FirstFlag = false;

    [SerializeField] private TitlePlayerMove player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //始まったときに再生されないように
        currentVFX.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //モデルが定位置に着いたか確認
        if (player.GetStopFlag() && !FirstFlag)
        {
            //エフェクト再生
            currentVFX.Play();
            Debug.Log("エフェクト開始");

            //一回だけ再生されるように
            FirstFlag = true;
        }
    }
}
