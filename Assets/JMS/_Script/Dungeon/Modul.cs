using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Modul : MonoBehaviour
{
    /// <summary>
    /// 각 이동 모듈별 연결 지점
    /// </summary>
    ModulConnector[] connectors;

    ItemSpawnPoint[] itemSpawnPoint;

    public ItemSpawnPoint[] ItemSpawnPoints => itemSpawnPoint;

    EnemySpawnPoint[] enemySpawnPoint;    
    public EnemySpawnPoint[] EnemySpawnPoints => enemySpawnPoint;

    public int ConnectorsCount
    {
        get => connectors.Length;
    }

    Transform[] connectorsTransform;


    /// <summary>
    /// true일경우 이 모듈은 1번만 소환되고 가능하면 소환해둔다
    /// </summary>
    public bool thisUnique = false;


    public ModulConnector[] Connectors => connectors;



    /// <summary>
    /// 모듈의 사이즈를 구하기 위한 랜더러
    /// </summary>
    //public Collider rend;

    /// <summary>
    /// 모듈의 중심점
    /// </summary>
    //Vector3 center;

    //public Vector3 Center => rend.bounds.center;

    /// <summary>
    /// 모듈의 사이즈
    /// </summary>
    //Vector3 size;

    //public Vector3 Size => rend.bounds.size;    

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        if(connectors == null)
        {
            connectors = child.GetComponentsInChildren<ModulConnector>();
        }

        itemSpawnPoint = child.GetComponentsInChildren<ItemSpawnPoint>();
        enemySpawnPoint = child.GetComponentsInChildren<EnemySpawnPoint>();
        //if (rend == null)
        //{

        //    rend = GetComponent<Collider>();
        //}
    }


    ///// <summary>
    ///// 모듈의 사이즈를 구하는 함수
    ///// </summary>
    ///// <returns>모듈의 x,y,z크기</returns>
    //public Vector3 GetSize()
    //{
    //    return size;
    //}





}
