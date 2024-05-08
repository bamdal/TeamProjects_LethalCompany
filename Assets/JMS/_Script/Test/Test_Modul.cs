using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Modul : TestBase
{
    public GameObject target;
    public Door Door;
    public GenerationPointNav generationPointNav;
    public CoilHead coilHeadPrefab;
    private void Start()
    {
        DungeonGenerator dungeonGenerator = FindAnyObjectByType<DungeonGenerator>();
        dungeonGenerator.StartGame();

    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        DungeonGenerator dungeonGenerator = FindAnyObjectByType<DungeonGenerator>();
        dungeonGenerator.StartGame();

    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Transform[] child = generationPointNav.transform.GetComponentsInChildren<Transform>();
        foreach (Transform child2 in child)
        {
            if(child2.gameObject != generationPointNav.gameObject)
                Destroy(child2.gameObject);
        }
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Instantiate(coilHeadPrefab, generationPointNav.transform);
    }
}
