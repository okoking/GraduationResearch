using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawn : MonoBehaviour
{
    [Header("�G�v���n�u")]
    [SerializeField] private GameObject enemyPrefab;
    [Header("�X�|�[���n�_�i�����w��\�j")]
    [SerializeField] private Transform[] spawnPoints;
    [Header("1�񂠂���̐�����")]
    [SerializeField] private int spawnCount = 5;
    [Header("�����Ԋu�i�b�j")]
    [SerializeField] private float spawnInterval = 10f;
    [Header("�����ɐ�������")]
    [SerializeField] private bool infiniteSpawn = true;
    [Header("�����_�����a�i�X�|�[���n�_�̎��́j")]
    [SerializeField] private float randomRadius = 5f;
    [Header("NavMesh�T�����a")]
    [SerializeField] private float navMeshSearchRadius = 3f;

    private float timer;
    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        SpawnEnemies();
    }

    void Update()
    {
        if (infiniteSpawn)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnEnemies();
                timer = 0f;
            }
        }
    }
    void SpawnEnemies()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("EnemySpawn: �X�|�[���n�_���ݒ肳��Ă��܂���B");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            //�����_���ȃX�|�[���n�_��I��
            Transform basePoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            //���͂ɏ��������_���I�t�Z�b�g��������iBoids�Q�����W�������Ȃ��悤�Ɂj
            Vector3 randomOffset = Random.insideUnitSphere * randomRadius;
            randomOffset.y = 0;
            Vector3 candidatePos = basePoint.position + randomOffset;

            //NavMesh��̗L���Ȓn�_��T��
            if (NavMesh.SamplePosition(candidatePos, out NavMeshHit hit, navMeshSearchRadius, NavMesh.AllAreas))
            {
                GameObject enemy = Instantiate(enemyPrefab, hit.position, Quaternion.identity);
                activeEnemies.Add(enemy);
            }
            else
            {
                Debug.LogWarning($"EnemySpawn: {basePoint.name} ���ӂŗL����NavMesh�ʒu��������܂���ł����B");
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
