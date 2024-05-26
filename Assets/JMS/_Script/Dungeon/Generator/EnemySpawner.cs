using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public EnemyBase[] enemyPrefabs;
    public GameObject[] trapPrefabs;

    Queue<EnemyBase> enemyCount;
    Queue<GameObject> trapCount;

    Transform enemys;
    Queue<GameObject> enemies;

    private void Awake()
    {
        enemyCount = new Queue<EnemyBase>();
        trapCount = new Queue<GameObject>();
        enemies = new Queue<GameObject>();
        if (enemys == null)
        {
            enemys = transform.GetChild(0);
        }
    }

    public void OnSpawnEnemy(List<EnemySpawnPoint> spawnPoints)
    {
        Difficulty difficulty = GameManager.Instance.Difficulty;
        Queue<EnemySpawnPoint> enemySpawnPointsQueue = new Queue<EnemySpawnPoint>();
        Shuffle(spawnPoints);
        foreach (EnemySpawnPoint spawnPoint in spawnPoints)
        {
            enemySpawnPointsQueue.Enqueue(spawnPoint);
        }

        foreach (EnemyBase enemyBase in enemyPrefabs)
        {
            IDuengenSpawn duengenSpawn = enemyBase.GetComponent<IDuengenSpawn>();
            for (int i = 0; i < duengenSpawn.MaxSpawnCount; i++)
            {
                if (UnityEngine.Random.value < duengenSpawn.SpawnPercent + 0.05 * (int)difficulty) // 난이도별 몬스터 소환 판정 시도
                {
                    enemyCount.Enqueue(enemyBase);
                }
            }
        }

        foreach (GameObject temp in trapPrefabs)
        {
            IDuengenSpawn duengenSpawn = temp.GetComponent<IDuengenSpawn>();
            for (int i = 0; i < duengenSpawn.MaxSpawnCount; i++)
            {
                if (UnityEngine.Random.value < duengenSpawn.SpawnPercent + 0.05 * (int)difficulty)
                {
                    trapCount.Enqueue(temp);
                }
            }
        }

        int spawnCount = 0;
        while (enemyCount.Count > 0 && enemySpawnPointsQueue.Count > 0)
        {
            EnemyBase obj = enemyCount.Dequeue();
            Vector3 spawnPosition = enemySpawnPointsQueue.Dequeue().transform.position;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(spawnPosition, out hit, 5.0f, NavMesh.AllAreas))
            {
                EnemyBase instantiatedObj = Instantiate(obj, hit.position, Quaternion.identity, enemys);
                instantiatedObj.transform.position = spawnPosition; 
                instantiatedObj.gameObject.SetActive(false);
                spawnCount++;
                enemies.Enqueue(instantiatedObj.gameObject);
            }
            else
            {
                Debug.LogWarning("Enemy spawn point is too far from the NavMesh.");
            }
        }

        while (trapCount.Count > 0 && enemySpawnPointsQueue.Count > 0)
        {
            GameObject obj = trapCount.Dequeue();
            Vector3 spawnPosition = enemySpawnPointsQueue.Dequeue().transform.position;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(spawnPosition, out hit, 5.0f, NavMesh.AllAreas))
            {
                GameObject instantiatedObj = Instantiate(obj, hit.position, Quaternion.identity, enemys);
                instantiatedObj.transform.position = spawnPosition;
                instantiatedObj.gameObject.SetActive(false);
                spawnCount++;
                enemies.Enqueue(instantiatedObj);
            }
            else
            {
                Debug.LogWarning("Trap spawn point is too far from the NavMesh.");
            }
        }

        Debug.Log($"적 소환 수 : {spawnCount}");

        int count = enemies.Count;
        for (int i = 0; i < count; i++)
        {
            enemies.Dequeue().SetActive(true);
        }
    }

    void Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
