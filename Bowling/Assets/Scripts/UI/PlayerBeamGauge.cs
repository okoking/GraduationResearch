using TMPro;
using UnityEngine;

public class PlayerBeamGauge : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Slider HpSlinder;   //バー

    private BeamGauge beamGauge;                                //プレイヤーの情報



    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        beamGauge = playerObj.GetComponent<BeamGauge>();


        //バー
        HpSlinder.value = beamGauge.GetGaugeRatio();
    }

    void Update()
    {
        TakeDamage();
    }

    //ダメージ処理
    void TakeDamage()
    {
        // スライダーに現在のHPを反映
        HpSlinder.value = beamGauge.GetGaugeRatio();    //現在のHPを反映
    }
}
