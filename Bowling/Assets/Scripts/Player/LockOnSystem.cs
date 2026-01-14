using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockOnSystem : MonoBehaviour
{
    public Transform lockOnTarget;
    public float lockOnRange = 20f;
    [SerializeField] private LockOn3DMarker markerSystem;
    private BeamCamera beamCamera;

    private void Start()
    {
        beamCamera = GetComponent<BeamCamera>();
    }

    void Update()
    {
        if (!beamCamera.isSootBeam)
        {
            lockOnTarget = FindLockOnTarget();
        }

        // 対象が消えたら解除
        if (lockOnTarget == null)
        {
            markerSystem.ClearLockOn();
            return;
        }

        // 一定距離外れたら解除
        if (beamCamera.isSootBeam || Vector3.Distance(transform.position, lockOnTarget.position) > lockOnRange)
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

    Transform FindLockOnTarget()
    {
        // 半径内のオブジェクトを全部取得（LayerMaskなし）
        Collider[] hits = Physics.OverlapSphere(transform.position, lockOnRange);

        if (hits.Length == 0)
            return null;

        // 敵だけに絞る（タグで判定）
        List<Transform> enemies = hits
            .Where(h => h.CompareTag("Enemy"))
            .Select(h => h.transform)
            .OrderBy(t => Vector3.Distance(transform.position, t.position))
            .ToList();

        if (enemies.Count == 0)
            return null;

        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.position);

            // ★ 一定距離以上ならロックオンしない
            if (dist > lockOnRange)
                continue;

            Vector3 dir = (enemy.position - transform.position).normalized;

            // ★ 遮蔽物チェック（Enemy 以外に当たったら遮蔽物）
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, dist))
            {
                // Enemy じゃないものに当たった → 遮蔽物
                if (!hit.collider.CompareTag("Enemy"))
                {
                    continue;
                }
            }

            // 遮蔽物なし → これがロックオン対象
            return enemy;
        }

        return null;
    }
}
