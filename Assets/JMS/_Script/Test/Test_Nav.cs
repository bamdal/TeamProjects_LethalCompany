
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Nav : TestBase
{
#if UNITY_EDITOR

    public GenerationPointNav pointNav;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        // 자식 오브젝트들을 반복하여 네비게이션 메쉬를 생성하고 할당

        NavMeshSurface surface = pointNav.GetComponent<NavMeshSurface>();
        if (surface != null)
        {
            surface.BuildNavMesh();
        }

    }
#endif
}