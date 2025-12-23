using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private BossHpBar bossHpBar;
    [SerializeField] private BossDeath bossDeath;

    private bool isSpawned = false;

    public void SpawnBoss()
    {
        if (isSpawned) return;

        GameObject bossObj = Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
        isSpawned = true;

        BossHp bossHp = bossObj.GetComponent<BossHp>();
        bossDeath.SetBoss(bossHp);

        bossHpBar.SetBoss(bossHp);
    }
}