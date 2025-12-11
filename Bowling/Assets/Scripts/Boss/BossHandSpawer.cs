using UnityEngine;

public class BossHandSpawer : MonoBehaviour
{

    //ボスの手のオブジェクト
    public GameObject bossHandPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //手を生成
        GameObject hand = Instantiate(bossHandPrefab, transform.position, Quaternion.identity);

        //手のスクリプトを取得して自らに対応させる
        BossHand handScript = hand.GetComponent<BossHand>();
        if (handScript != null)
        {
            handScript.bossHandSpawn = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
