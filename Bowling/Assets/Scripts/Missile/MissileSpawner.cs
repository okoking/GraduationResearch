using System.Collections;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{

    [SerializeField]
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
        thisTransform = transform;
        intervalWait = new WaitForSeconds(interval);
    }

    void Update()
    {
        if (isSpawning)
        {
            return;
        }

        if (missileMeter > MeterMax)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                StartCoroutine(nameof(SpawnMissile));
                missileMeter = 0;
            }
        }
        
    }

    IEnumerator SpawnMissile()
    {
        isSpawning = true;

        Missile homing;

        for (int i = 0; i < iterationCount; i++)
        {
            homing = Instantiate(prefab, thisTransform.position, Quaternion.identity).GetComponent<Missile>();
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