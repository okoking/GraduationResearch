using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private BossHpBar bossHpBar;
    [SerializeField] private Canvas playCanvas;

    private bool isSpawned = false;

    void Awake()
    {
        bossHpBar.gameObject.SetActive(false);
    }

    public void SpawnBoss()
    {
        if (isSpawned) return;

        GameObject bossObj = Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);

        BossHp bossHp = bossObj.GetComponent<BossHp>();

        bossHpBar.SetBoss(bossHp);
        bossHpBar.gameObject.SetActive(true);

        isSpawned = true;
    }
}