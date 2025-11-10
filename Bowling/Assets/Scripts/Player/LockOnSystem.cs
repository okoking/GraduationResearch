using UnityEngine;

public class LockOnSystem : MonoBehaviour
{
    public Transform lockOnTarget;
    [SerializeField] private float lockOnRange = 20f;
    [SerializeField] private LockOn3DMarker markerSystem;

    void Update()
    {
        //if (Input.GetKeyDown("joystick button 1"))
        //{
        lockOnTarget = FindClosestEnemy()?.transform;
        //}

        // ‘ÎÛ‚ªÁ‚¦‚½‚ç‰ðœ
        if (lockOnTarget == null)
        {
            markerSystem.ClearLockOn();
            return;
        }

        // ˆê’è‹——£ŠO‚ê‚½‚ç‰ðœ
        if (Vector3.Distance(transform.position, lockOnTarget.position) > lockOnRange)
        {
            lockOnTarget = null;
        }
        else
        {
            markerSystem.SetLockOn(lockOnTarget);
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDist && dist < lockOnRange)
            {
                closest = enemy;
                minDist = dist;
            }
        }

        return closest;
    }
}
