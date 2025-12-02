using UnityEngine;

public class blockSpawn : MonoBehaviour
{

    public GameObject block;

    blockHp blockhp;

    public Vector3 blockPos;

    public float coolTime;

    private float coolTimeCnt;

    public int maxHp;

    private int currentHp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ブロックを生成
        Instantiate(block, blockPos, transform.rotation);

        blockhp = GameObject.Find("wall").GetComponent<blockHp>();

        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        //HPが0以下になればこいつ自身を消す
        if(currentHp <= 0)
        {
            Destroy(this);
        }

        coolTimeCnt++;
        if(coolTimeCnt > coolTime)
        {
            coolTimeCnt = 0;
            blockhp.HealHp();
        }
    }
}