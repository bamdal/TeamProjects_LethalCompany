using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.InputSystem;

public class GenerationPointNav : MonoBehaviour
{
    NavMeshSurface surface;

    private void Awake()
    {
        //네비게이션 메쉬를 생성하고 할당
        surface = GetComponent<NavMeshSurface>();


    }

    /// <summary>
    /// 현재 오브젝트의 자식들만 네비메시를 까는 함수
    /// </summary>
    public void CompliteGenerationDungeon()
    {
        if (surface != null)
        {
            surface.BuildNavMesh();
        }
    }

}
