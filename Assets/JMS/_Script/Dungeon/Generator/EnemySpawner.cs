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
        foreach (EnemyBase enemyBase in enemyPrefabs)
        {
            IDuengenSpawn duengenSpawn = enemyBase.GetComponent<IDuengenSpawn>();
            for(int i = 0; i < duengenSpawn.MaxSpawnCount; i++)
            {
                
                if(Random.value<duengenSpawn.SpawnPercent+ 0.05 * (int)difficulty)  // 난이도별 몬스터 소환 판정 시도
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
            obj.gameObject.SetActive(false);
            int index = Random.Range(0, spawnPoints.Count);
            obj.transform.position = spawnPoints[index].transform.position; spawnCount++;
            spawnPoints.RemoveAt(index);    // 같은 위치에 중복 소환 방지
            enemies.Enqueue(obj.gameObject);
        }

        while (trapCount.Count > 0)
        {
            GameObject obj = Instantiate(trapCount.Dequeue(), enemys);
            obj.gameObject.SetActive(false);
            int index = Random.Range(0, spawnPoints.Count);
            obj.transform.position = spawnPoints[index].transform.position; spawnCount++;
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
}
