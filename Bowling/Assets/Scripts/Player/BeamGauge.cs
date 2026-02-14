using UnityEngine;

public class BeamGauge : MonoBehaviour
{
    //[SerializeField] Image gaugeFill;

    [SerializeField] float currentGauge;
    [SerializeField] float maxGauge = 100f;
    [SerializeField] float chargeVal = 20f;          // “G“¢”°‚Ì‰ñ•œ—Ê
    [SerializeField] float consumePerSecond = 30f;   // •bŠÔÁ”ï—Ê


    public float beamLifeTime;

    bool isUsingBeam;

    private void Start()
    {
        currentGauge = 0;
    }

    void Update()
    {
        beamLifeTime = maxGauge / consumePerSecond;
    }

    public void Charge()
    {
        currentGauge = Mathf.Clamp(
            currentGauge + chargeVal,
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

    public float GetGaugeRatio()
    {
        return currentGauge / maxGauge;
    }

    public bool IsLowestValueShotBeam()
    {
        // Å’á‚Q•b‘Å‚Ä‚é•ª‚Ìƒr[ƒ€‚ª‚È‚¯‚ê‚Î‘Å‚Ä‚È‚¢‚æ‚¤‚É‚·‚é
        if (currentGauge <= consumePerSecond * 2f)
            return false;

        return true;
    }
}