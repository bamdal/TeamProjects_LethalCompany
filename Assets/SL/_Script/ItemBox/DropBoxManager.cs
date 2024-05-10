using System;
using System.Collections;
using UnityEngine;

public class DropBoxManager : MonoBehaviour
{
    GameManager gameManager;

    public GameObject ItemBoxPrepab;

    Coroutine dropItem;

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.onBuy += DropItemBox;
    }
    IInteraction temp;
    void DropItemBox()
    {
        if(gameManager.ItemsQueue.Count > 0 && gameManager.OnGameState == GameState.GameStart && dropItem ==null) 
        {

            dropItem = StartCoroutine(Drop());
        }
    }
    IEnumerator Drop()
    {
        yield return new WaitForSeconds(3f);
        GameObject itemTemp = Instantiate(ItemBoxPrepab);
         temp = itemTemp.GetComponent<IInteraction>();
        temp.onRequest += DropItemBox;
        dropItem = null;
    }
}
