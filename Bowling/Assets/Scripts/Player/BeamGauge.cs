using UnityEngine;
using UnityEngine.UI;

public class BeamGauge : MonoBehaviour
{
    [SerializeField] Image gaugeFill;

    private float maxGauge = 100f;
    private float currentGauge = 0f;
    private float chargeSpeed = 20f;
    private float chargecost = 1f;

    void Update()
    {
        Charge();
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

    public bool UseBeam(float cost)
    {
        if (currentGauge < cost)
            return false;

        currentGauge -= cost;
        UpdateGauge();
        return true;
    }
    
    public bool UseBeam()
    {
        if (currentGauge < chargecost)
            return false;

        currentGauge -= chargecost;
        UpdateGauge();
        return true;
    }

    void UpdateGauge()
    {
        gaugeFill.fillAmount = currentGauge / maxGauge;
    }
}
