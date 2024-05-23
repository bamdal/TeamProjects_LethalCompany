
using System.Collections.Generic;
using UnityEngine;

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
    
    public void OnSpawnEnemy(List<EnemySpawnPoint> spawnPoints, Difficulty difficulty)
    {
        Queue<EnemySpawnPoint> enemySpawnPointsQueue = new Queue<EnemySpawnPoint>();
        Shuffle(spawnPoints);
        foreach (EnemySpawnPoint spawnPoint in spawnPoints)
        {
            enemySpawnPointsQueue.Enqueue(spawnPoint);
        }
        foreach (EnemyBase enemyBase in enemyPrefabs)
        {
            IDuengenSpawn duengenSpawn = enemyBase.GetComponent<IDuengenSpawn>();
            for(int i = 0; i < duengenSpawn.MaxSpawnCount; i++)
            {
                
                if(UnityEngine.Random.value<duengenSpawn.SpawnPercent+ 0.05 * (int)difficulty)  // 난이도별 몬스터 소환 판정 시도
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
        while (enemyCount.Count > 0 && enemySpawnPointsQueue.Count >0)
        {
            EnemyBase obj = Instantiate(enemyCount.Dequeue(), enemys);
            obj.gameObject.SetActive(false);
            obj.transform.position = enemySpawnPointsQueue.Dequeue().transform.position; spawnCount++;
            enemies.Enqueue(obj.gameObject);
        }

        while (trapCount.Count > 0 && enemySpawnPointsQueue.Count > 0)
        {
            GameObject obj = Instantiate(trapCount.Dequeue(), enemys);
            obj.gameObject.SetActive(false);
            int index = UnityEngine.Random.Range(0, spawnPoints.Count);
            obj.transform.position = enemySpawnPointsQueue.Dequeue().transform.position; spawnCount++;
            spawnPoints.RemoveAt(index);
            enemies.Enqueue(obj);
        }

    

        Debug.Log($"적 소환 수 : {spawnCount}");

        int count = enemies.Count;
        for(int i = 0; i < count; i++)
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
