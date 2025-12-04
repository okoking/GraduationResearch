using UnityEngine;

public class blockHp : MonoBehaviour
{

    public int maxHp;

    private int currentHp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //HPが0以下になればこいつ自身を消す
        if (currentHp <= 0)
        {
            Destroy(this);
        }

        //Debug.Log("ブロックの現在HP" + currentHp);
    }

    public void HealHp()
    {
        currentHp++;
        if (currentHp >= maxHp)
        {
            currentHp = maxHp;
        }
    }

    public void TakeDamage(int i)
    {
        currentHp = -i;
    }
}
