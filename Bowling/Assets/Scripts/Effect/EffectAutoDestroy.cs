using UnityEngine;

public class EffectAutoDestroy : MonoBehaviour
{
    int effectId;
    ParticleSystem ps;

    public void Init(int id)
    {
        effectId = id;
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (ps == null) return;

        // çƒê∂èIóπÇµÇΩÇÁ
        if (!ps.IsAlive(true))
        {
            EffectManager.instance.Stop(effectId);
        }
    }
}
