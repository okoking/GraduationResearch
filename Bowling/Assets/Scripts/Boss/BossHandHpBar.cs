using UnityEngine;
using UnityEngine.UI;

public class BossHandHpBar : MonoBehaviour
{
    private BossHandHp handHp;
    [SerializeField] private Image hpFillImage;

    void Start()
    {
        if (handHp == null)
        {
            handHp = GetComponentInParent<BossHandHp>();
        }
    }

    void Update()
    {
        if (handHp == null || hpFillImage == null) return;

        hpFillImage.fillAmount =
            (float)handHp.hp / handHp.maxHp;
    }

    void LateUpdate()
    {
        //í‚ÉƒJƒƒ‰Œü‚«
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}