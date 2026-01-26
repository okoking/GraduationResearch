using UnityEngine;

public class WallVisibilityGroup : MonoBehaviour
{
    Renderer[] renderers;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        SetVisible(false); // ˆê‹C‚É”ñ•\Ž¦
    }

    public void SetVisible(bool visible)
    {
        foreach (var r in renderers)
        {
            r.enabled = visible;
        }
    }
}
