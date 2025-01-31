using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Modul : TestBase
{
#if UNITY_EDITOR
    public GameObject target;
    public Door Door;
    public GenerationPointNav generationPointNav;
    public CoilHead coilHeadPrefab;


    public Enemy_Spider spiderPrefab;    // 적 프리팹
    //private Enemy_Spider spiderEnemy;         // 생성된 적의 참조를 저장하는 변수


    private void Start()
    {
        DungeonGenerator dungeonGenerator = FindAnyObjectByType<DungeonGenerator>();
        dungeonGenerator.StartGame();
        GameManager.Instance.GameState = GameState.GameStart;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        /*DungeonGenerator dungeonGenerator = FindAnyObjectByType<DungeonGenerator>();
        dungeonGenerator.StartGame();*/
        Player player = GameManager.Instance.Player;
        player.IsInDungeon = true;

    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        /*Transform[] child = generationPointNav.transform.GetComponentsInChildren<Transform>();
        foreach (Transform child2 in child)
        {
            if(child2.gameObject != generationPointNav.gameObject)
                Destroy(child2.gameObject);
        }*/
        Player player = GameManager.Instance.Player;
        player.IsInDungeon = false;

    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        GameManager.Instance.ItemsQueue.Enqueue(ItemCode.Shovel);
        GameManager.Instance.ItemsQueue.Enqueue(ItemCode.FlashLight);
        Debug.Log(GameManager.Instance.ItemsQueue.Count);
        /*Instantiate(coilHeadPrefab, generationPointNav.transform);*/
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        GameManager.Instance.OnUse();
        /*        Instantiate(spiderPrefab, generationPointNav.transform);
        */
    }
#endif
}
