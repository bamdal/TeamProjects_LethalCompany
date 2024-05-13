using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyBase[] enemyPrefabs;
    public GameObject[] trapPrefabs;

    Queue<EnemyBase> enemyCount;
    Queue<GameObject> trapCount;

    Transform enemys;

    private void Awake()
    {
        enemyCount = new Queue<EnemyBase>();
        trapCount = new Queue<GameObject>();
        if (enemys == null)
        {
            enemys = transform.GetChild(0);
        }
    }
    public void OnSpawnEnemy(List<EnemySpawnPoint> spawnPoints, Difficulty difficulty)
    {
        foreach (EnemyBase enemyBase in enemyPrefabs)
        {
            IDuengenSpawn duengenSpawn = enemyBase.GetComponent<IDuengenSpawn>();
            for(int i = 0; i < duengenSpawn.MaxSpawnCount; i++)
            {
                
                if(Random.value<duengenSpawn.SpawnPercent+ 0.05 * (int)difficulty)
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

                if (Random.value < duengenSpawn.SpawnPercent + 0.05 * (int)difficulty)
                {
                    trapCount.Enqueue(temp);
                }
               
            }
        }

        int spawnCount = 0;
        while (enemyCount.Count > 0)
        {
            EnemyBase obj = Instantiate(enemyCount.Dequeue(), enemys);
            obj.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position; spawnCount++;
        }

        while (trapCount.Count > 0)
        {
            GameObject obj = Instantiate(trapCount.Dequeue(), enemys);
            obj.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position; spawnCount++;
        }

    

        Debug.Log($"적 소환 수 : {spawnCount}");
    }
}
