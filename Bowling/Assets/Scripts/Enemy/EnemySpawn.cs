using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawn : MonoBehaviour
{
    public static EnemySpawn Instance { get; private set; }
    [Header("敵プレハブ")]
    [SerializeField] private GameObject enemyPrefab;
    [Header("スポーン地点（複数指定可能）")]
    [SerializeField] private Transform[] spawnPoints;
    [Header("1回あたりの生成数")]
    [SerializeField] private int spawnCount = 5;
    [Header("生成間隔（秒）")]
    [SerializeField] private float spawnInterval = 10f;
    [Header("無限に生成する")]
    [SerializeField] private bool infiniteSpawn = true;
    [Header("ランダム半径（スポーン地点の周囲）")]
    [SerializeField] private float randomRadius = 5f;
    [Header("NavMesh探索半径")]
    [SerializeField] private float navMeshSearchRadius = 3f;
    [Header("敵タイプ出現率")]
    [SerializeField] private float meleeRate = 0.6f; // 60% Melee

    private float timer;
    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        if (infiniteSpawn && GameStartDirector.IsGameStarted)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnEnemies();
                timer = 0f;
            }
        }
    }
    public void SpawnEnemies()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("EnemySpawn: スポーン地点が設定されていません。");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            //ランダムなスポーン地点を選ぶ
            Transform basePoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            //周囲に少しランダムオフセットを加える（Boids群が密集しすぎないように）
            Vector3 randomOffset = Random.insideUnitSphere * randomRadius;
            randomOffset.y = 0;
            Vector3 candidatePos = basePoint.position + randomOffset;
            
            //NavMesh上の有効な地点を探す
            if (NavMesh.SamplePosition(candidatePos, out NavMeshHit hit, navMeshSearchRadius, NavMesh.AllAreas))
            {
                GameObject enemy = Instantiate(enemyPrefab, hit.position, Quaternion.identity);

                var enemyAI = enemy.GetComponent<EnemyAI>();

                // ランダムでタイプ決定
                EnemyType type =
                    Random.value < meleeRate ? EnemyType.Melee : EnemyType.Ranged;

                enemyAI.SetEnemyType(type);

                // パトロール中心設定
                enemyAI.SetPatrolCenter(basePoint);

                activeEnemies.Add(enemy);

                EffectManager.instance.Play("Teleport", enemy.transform.position);
            }
            else
            {
                Debug.LogWarning($"EnemySpawn: {basePoint.name} 周辺で有効なNavMesh位置が見つかりませんでした。");
            }
        }
    }

    public void ClearEnemies()
    {
        foreach (var e in activeEnemies)
        {
            if (e != null) Destroy(e);
        }
        activeEnemies.Clear();
    }
}
