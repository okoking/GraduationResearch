using System.Collections;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{

    
    Transform target;
    
    [SerializeField] private GameObject missilePrefab;
    [SerializeField, Min(1)]
    int iterationCount = 10;
    [SerializeField]
    float interval = 0.1f;

    bool isSpawning = false;
    Transform thisTransform;
    WaitForSeconds intervalWait;

    public int MeterMax = 100;

    int missileMeter;

    void Start()
    {
        thisTransform = transform;
        intervalWait = new WaitForSeconds(interval);

        if (missilePrefab == null)
        {
            Debug.LogError("MissileSpawner: missilePrefab が設定されていません");
            enabled = false;
            return;
        }

        var player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("MissileSpawner: Player が見つかりません");
            enabled = false;
            return;
        }

        target = player.transform;
    }

    void Update()
    {
        if (isSpawning)
        {
            return;
        }

        //if (missileMeter > MeterMax)
        //{
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SpawnMissile());
            missileMeter = 0;
        }
        //}

    }

    IEnumerator SpawnMissile()
    {
        isSpawning = true;

        for (int i = 0; i < iterationCount; i++)
        {
            Vector3 spawnPos = transform.position + Vector3.up * 1.5f;

            var go = Instantiate(missilePrefab, spawnPos, Quaternion.identity);
            var missile = go.GetComponent<Missile>();
            missile.Target = target;
            if (missile == null)
            {
                Debug.LogError("Missile prefab に Missile コンポーネントがありません");
                Destroy(go);
                continue;
            }
        }

        isSpawning = false;
        yield return null;
    }

    public void Fire()
    {
        if (isSpawning) return;

        StartCoroutine(SpawnMissile());
    }

    public bool GetFlg()
    {
        return isSpawning;
    }

    public void MeterPlus(int num)
    {
        missileMeter += num;
    }
}