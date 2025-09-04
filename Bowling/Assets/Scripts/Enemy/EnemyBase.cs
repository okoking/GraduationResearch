using UnityEngine;

public class EnemyBase : MonoBehaviour
{

    private HitPointManager enemyHp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyHp = GetComponent<HitPointManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            enemyHp.TakeDamage(1);
        }

        Debug.Log("Enemy HP: " + enemyHp.GetCurrentHp());
    }
}
