using System;
using System.Collections;
using UnityEngine;

public class DropBoxManager : MonoBehaviour
{
    GameManager gameManager;

    public GameObject itemBoxPrepab;
    public Transform dropPosition;

    Coroutine dropItem;

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.onBuy += DropItemBox;
    }
    IInteraction temp;
    void DropItemBox()
    {
        if(gameManager.ItemsQueue.Count > 0 && gameManager.GameState == GameState.GameStart && dropItem ==null) 
        {

            dropItem = StartCoroutine(Drop());
        }
    }
    IEnumerator Drop()
    {
        yield return new WaitForSeconds(3f);
        GameObject itemTemp = Instantiate(itemBoxPrepab, dropPosition.position, dropPosition.rotation, null);
        itemTemp.transform.position = dropPosition.localPosition;
         temp = itemTemp.GetComponent<IInteraction>();
        temp.onRequest += DropItemBox;
        dropItem = null;
    }
}
