using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modul : MonoBehaviour
{
    /// <summary>
    /// 각 이동 모듈별 연결 지점
    /// </summary>
    ModulConnector[] connectors;

    public int ConnectorsCount
    {
        get => connectors.Length;
    }

    /// <summary>
    /// true일경우 이 모듈은 1번만 소환되고 가능하면 소환해둔다
    /// </summary>
    public bool thisUnique = false;


    public ModulConnector[] Connectors => connectors;

    /// <summary>
    /// 모듈의 사이즈를 구하기 위한 랜더러
    /// </summary>
    MeshRenderer rend;

    /// <summary>
    /// 모듈의 중심점
    /// </summary>
    Vector3 center;

    /// <summary>
    /// 모듈의 사이즈
    /// </summary>
    Vector3 size;

    private void Awake()
    {
        // 모듈이 연결될수 있는 모든 문
        connectors = GetComponentsInChildren<ModulConnector>();
        //rend = GetComponent<MeshRenderer>();


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
