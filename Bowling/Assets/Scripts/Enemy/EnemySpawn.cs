using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawn : MonoBehaviour
{
    public static EnemySpawn Instance { get; private set; }
    [Header("敵プレハブ")]
    [SerializeField] private GameObject meleeEnemyPrefab;
    [SerializeField] private GameObject rangedEnemyPrefab;
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
    [Header("同時出現できる最大敵数")]
    [SerializeField] private int maxEnemyCount = 100;

    [System.Serializable]
    public class SpawnRange
    {
        public int startIndex; // 開始インデックス
        public int count;      // 何個使うか
    }

    [Header("ステージごとのスポーン範囲")]
    [SerializeField] private List<SpawnRange> stageSpawnRanges;

    [SerializeField] public int currentStage = 0; // 0 = ステージ1

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
        //すでに最大数なら何もしない
        if (activeEnemies.Count >= maxEnemyCount)
            return;

        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("EnemySpawn: スポーン地点が設定されていません。");
            return;
        }

        // 生成可能な残り数を計算
        int canSpawn = maxEnemyCount - activeEnemies.Count;
        int spawnNum = Mathf.Min(spawnCount, canSpawn);

        for (int i = 0; i < spawnNum; i++)
        {
            //ランダムなスポーン地点を選ぶ
            Transform basePoint = GetRandomSpawnPoint();
            if (basePoint == null) return;

            //周囲に少しランダムオフセットを加える（Boids群が密集しすぎないように）
            Vector3 randomOffset = Random.insideUnitSphere * randomRadius;
            randomOffset.y = 0;
            Vector3 candidatePos = basePoint.position + randomOffset;
            
            //NavMesh上の有効な地点を探す
            if (NavMesh.SamplePosition(candidatePos, out NavMeshHit hit, navMeshSearchRadius, NavMesh.AllAreas))
            {
                //敵タイプを先に決定
                bool isMelee = Random.value < meleeRate;

                GameObject prefab = isMelee
                    ? meleeEnemyPrefab
                    : rangedEnemyPrefab;

                GameObject enemy = Instantiate(prefab, hit.position, Quaternion.identity);

                var enemyAI = enemy.GetComponent<EnemyAI>();

                enemyAI.SetEnemyType(isMelee ? EnemyType.Melee : EnemyType.Ranged);
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
            if (e != null)
            {
                var ai = e.GetComponent<EnemyAI>();
                if (ai != null)
                    EnemyManager.Instance.UnregisterEnemy(ai);

                Destroy(e);
            }
        }

        activeEnemies.Clear();
    }

    Transform GetRandomSpawnPoint()
    {
        if (stageSpawnRanges.Count <= currentStage)
        {
            Debug.LogWarning("ステージのスポーン範囲が未設定です");
            return null;
        }

        SpawnRange range = stageSpawnRanges[currentStage];

        int min = range.startIndex;
        int max = range.startIndex + range.count;

        // 安全対策
        max = Mathf.Min(max, spawnPoints.Length);

        int index = Random.Range(min, max);
        return spawnPoints[index];
    }

    public void SetStage(int stage)
    {
        currentStage = stage;
    }

    public void UnregisterEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
            activeEnemies.Remove(enemy);
    }
}
