using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform spawnPoint;

    private bool isSpawned = false;

    void OnEnable()
    {
        DoorDeath.OnDoorOpened += SpawnBoss;
    }

    void OnDisable()
    {
        DoorDeath.OnDoorOpened -= SpawnBoss;
    }

    void SpawnBoss()
    {
        if (isSpawned) return;

        Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
        isSpawned = true;
    }
}
