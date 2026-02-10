using UnityEngine;
using UnityEngine.VFX;

public class TitleBeamStart : MonoBehaviour
{
    public VisualEffect currentVFX;

    [SerializeField] private TitlePlayerMove player;
    [SerializeField] AudioSource se1;
    [SerializeField] AudioSource se2;


    void Start()
    {
        //始まったときに再生されないように
        currentVFX.Stop();
    }

    void Update()
    {
        //モデルが定位置に着いたか確認
        if (player.GetStopFlag())
        {
            //エフェクト再生
            currentVFX.Play();
            se1.Play();
            se2.Play();
            Debug.Log("3:エフェクト開始");
        }
    }
}
