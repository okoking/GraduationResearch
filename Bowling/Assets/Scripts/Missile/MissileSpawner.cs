using System.Collections;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{

    
    Transform target;
    [SerializeField]
    GameObject prefab;
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
        //thisTransform = transform;
        //intervalWait = new WaitForSeconds(interval);
        //var player = GameObject.FindWithTag("Player");

        //target = player.transform;

        thisTransform = transform;
        intervalWait = new WaitForSeconds(interval);

        if (prefab == null)
        {
            Debug.LogError("MissileSpawner: prefab が設定されていません");
            enabled = false;
            return;
        }

        var player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("MissileSpawner: Player タグのオブジェクトが見つかりません");
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
        //isSpawning = true;

        //Missile homing;

        //for (int i = 0; i < iterationCount; i++)
        //{
        //    homing = Instantiate(prefab, thisTransform.position, Quaternion.identity).GetComponent<Missile>();
        //    homing.Target = target;
        //}

        //yield return intervalWait;

        //isSpawning = false;

        isSpawning = true;

        for (int i = 0; i < iterationCount; i++)
        {
            var go = Instantiate(prefab, thisTransform.position, Quaternion.identity);
            var homing = go.GetComponent<Missile>();
            Debug.Assert(homing != null, "Missile が付いていません！");

            if (homing == null)
            {
                Debug.LogError("MissileSpawner: prefab に Missile コンポーネントが付いていません");
                Destroy(go);
                yield break;
            }

            homing.Target = target;
        }

        yield return intervalWait;
        isSpawning = false;
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