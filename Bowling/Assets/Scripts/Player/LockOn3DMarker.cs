using UnityEngine;

public class LockOn3DMarker : MonoBehaviour
{
    public GameObject markerPrefab;
    private GameObject currentMarker;
    private Transform enemyTransform;
    private void Update()
    {
        if (currentMarker != null && enemyTransform != null)
            currentMarker.transform.position = enemyTransform.position + Vector3.up * 1.5f;

        if (enemyTransform != null && currentMarker != null)
        {
            Debug.Log("なんかは入ってるらしい");
        }

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
        //if (currentMarker != null) Destroy(currentMarker);
        if (enemyTransform != enemy)
        {
            Destroy(currentMarker);
            currentMarker = Instantiate(markerPrefab,
                enemy.position + Vector3.up * 1.5f, Quaternion.identity);
            enemyTransform = enemy;
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
            enemyTransform = null;
        }
    }
}