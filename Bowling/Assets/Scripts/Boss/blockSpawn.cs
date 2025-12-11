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
        GameObject obj = Instantiate(block, blockPos, transform.rotation);

        currentHp = maxHp;

        blockhp = obj.GetComponent<blockHp>();
    }

    void Update()
    {
        if (currentHp <= 0)
        {
            Destroy(gameObject);
            return;
        }

        coolTimeCnt += Time.deltaTime;

        if (coolTimeCnt > coolTime)
        {
            coolTimeCnt = 0;
            blockhp.HealHp();
        }
    }

    public void TakeDamage(int i)
    {
        currentHp -= i;
        Debug.Log(currentHp);
    }
}