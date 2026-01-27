using UnityEngine;

public class WallVisibilityGroup : MonoBehaviour
{
    Renderer[] renderers;

    //§ŒÀ‚Ì•Ç‚ğŒ©‚¦‚È‚­‚·‚éˆ—

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        SetVisible(false); // ˆê‹C‚É”ñ•\¦
    }

    public void SetVisible(bool visible)
    {
        foreach (var r in renderers)
        {
            r.enabled = visible;
        }
    }
}
