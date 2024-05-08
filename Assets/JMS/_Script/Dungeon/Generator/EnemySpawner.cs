using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyBase[] enemyPrefabs;
    public GameObject[] trapPrefabs;

    [Range(0f, 1f)]
    public float Spawn = 0.2f;
    public void OnSpawnEnemy(List<EnemySpawnPoint> spawnPoints, Difficulty difficulty)
    {
        int maxSpawnCount = 4 * (int)difficulty;
        int spawnCount = 0;
        foreach (EnemySpawnPoint spawnPoint in spawnPoints)
        {
            if (spawnCount < maxSpawnCount && Random.value * (int)difficulty < Spawn)
            {
                GameObject obj = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length - 1)].gameObject, spawnPoint.transform);
                spawnCount++;
                obj.transform.position = spawnPoint.transform.position;
            }
        }
        spawnCount = 0;
        foreach (EnemySpawnPoint spawnPoint in spawnPoints)
        {
            if (spawnCount < maxSpawnCount && Random.value * (int)difficulty < Spawn)
            {
                GameObject obj = Instantiate(trapPrefabs[Random.Range(0, enemyPrefabs.Length - 1)].gameObject, spawnPoint.transform);
                spawnCount++;
                obj.transform.position = spawnPoint.transform.position;
            }
        }

        Debug.Log($"적 소환 수 : {spawnCount}");
    }
}
