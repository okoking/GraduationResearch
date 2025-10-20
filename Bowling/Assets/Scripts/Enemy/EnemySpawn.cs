using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    //オリジンプレハブ
    public GameObject enemyPrefab;

    //何体生むか
    public int spawnNum;

    //敵生成座標
    public Vector3 spawnPos;

    //何秒おきに生成するか
    public float spawnSpeed;

    //秒数カウント変数
    float count = 0.0f;

    //スポーン完了したかフラグ
    bool finSpawn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //完了してなければ
        if (!finSpawn)
        {
            for (int i = 0; i < spawnNum; i++)
            {
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            }
            //完了したら終了
            finSpawn = true;
        }

        //完了していれば一定期間後に再生成
        if (finSpawn)
        {
            count++;
            //カウントが指定秒数に達したら
            if (count > spawnSpeed) {
                finSpawn = false;
                count = 0f;
            }
        }
    }
}