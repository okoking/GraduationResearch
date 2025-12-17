using UnityEngine;
using UnityEngine.UI;

public class BeamGauge : MonoBehaviour
{
    [SerializeField] Image gaugeFill;

    [SerializeField] float maxGauge = 100f;
    [SerializeField] float chargeSpeed = 20f;        // ïbä‘âÒïúó 
    [SerializeField] float consumePerSecond = 30f;   // ïbä‘è¡îÔó 

    float currentGauge;
    bool isUsingBeam;

    void Update()
    {
        if (!isUsingBeam)
            Charge();

        Debug.Log(currentGauge);

        UpdateGauge();
    }

    void Charge()
    {
        currentGauge = Mathf.Clamp(
            currentGauge + chargeSpeed * Time.deltaTime,
            0,
            maxGauge
        );
    }

    public bool TryConsume()
    {
        float cost = consumePerSecond * Time.deltaTime;

        if (currentGauge < cost)
            return false;

        currentGauge -= cost;
        return true;
    }

    public void SetUsingBeam(bool value)
    {
        isUsingBeam = value;
    }

    void UpdateGauge()
    {
        //gaugeFill.fillAmount = currentGauge / maxGauge;
    }
}
