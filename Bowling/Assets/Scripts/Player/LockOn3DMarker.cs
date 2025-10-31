using UnityEngine;

public class LockOn3DMarker : MonoBehaviour
{
    public GameObject markerPrefab;
    private GameObject currentMarker;

    public void SetLockOn(Transform enemy)
    {
        if (currentMarker != null) Destroy(currentMarker);
        currentMarker = Instantiate(markerPrefab, enemy.position + Vector3.up * 2f, Quaternion.identity);
        currentMarker.transform.SetParent(enemy); // “G‚Ìq‚É‚µ‚Ä’Ç]
    }

    public void ClearLockOn()
    {
        if (currentMarker != null) Destroy(currentMarker);
    }
}
