using UnityEngine;

public class BeamGauge : MonoBehaviour
{
    public bool isChargeComplete = false;

    private int BeamGaugeValue;
    private int BeamGaugeMaxValue = 100;

    private int ChargeQuantity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ChargeBeamGauge();
    }

    void ChargeBeamGauge()
    {
        BeamGaugeValue += ChargeQuantity;
        BeamGaugeValue = Mathf.Clamp(BeamGaugeValue, 0, BeamGaugeMaxValue); // 0～maxに制限

        Debug.Log("現在のチャージ量：" + BeamGaugeValue);

        if (BeamGaugeValue >= BeamGaugeMaxValue)
        {
            ChargeComplete();
        }
    }

    void ChargeComplete()
    {
        isChargeComplete = true;
    }
}
