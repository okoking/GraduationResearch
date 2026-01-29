using UnityEngine;

public class LockOn3DMarker : MonoBehaviour
{
    public GameObject markerPrefab;
    private GameObject currentMarker;
    private Transform oldMarker;
    private void Update()
    {
        if (currentMarker != null)
            currentMarker.transform.position = oldMarker.position + Vector3.up * 1.5f;
        //if (currentMarker == null) return;

        ////Destroy 済みチェック
        //if (!oldMarker)
        //{
        //    ClearLockOn();
        //    return;
        //}

        //currentMarker.transform.position = oldMarker.position + Vector3.up * 1.5f;
    }

    public void SetLockOn(Transform enemy)
    {
        if (currentMarker != null) Destroy(currentMarker);
        if (oldMarker != enemy)
        {
            Destroy(currentMarker);
            currentMarker = Instantiate(markerPrefab,
                enemy.position + Vector3.up * 1.5f, Quaternion.identity);
            oldMarker = enemy;
        }
        //if (!enemy) return;

        //if (oldMarker != enemy)
        //{
        //    if (currentMarker != null)
        //        Destroy(currentMarker);

        //    currentMarker = Instantiate(
        //        markerPrefab,
        //        enemy.position + Vector3.up * 1.5f,
        //        Quaternion.identity
        //    );

        //    oldMarker = enemy;
        //}
    }

    public void ClearLockOn()
    {
        if (currentMarker != null)
        {
            Destroy(currentMarker);
            currentMarker = null;
            oldMarker = null;
        }
    }
}