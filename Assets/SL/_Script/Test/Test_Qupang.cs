using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Qupang : TestBase
{
    Player player;
    ItemDrop itemDrop;
    GameManager gameManager;
    private void Start()
    {
        player = GameManager.Instance.Player;
        itemDrop = FindAnyObjectByType<ItemDrop>();
        gameManager = GameManager.Instance;
    }
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        gameManager.ItemsQueue.Enqueue(ItemCode.Shovel);
        Debug.Log(gameManager.ItemsQueue);
    }
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        gameManager.ItemsQueue.Enqueue(ItemCode.FlashLight);
        Debug.Log(gameManager.ItemsQueue);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        
        Debug.Log(gameManager.ItemsQueue);
    }



}
