using System;
using System.Collections;
using UnityEngine;

public class DropBox : MonoBehaviour
{
    GameManager gameManager;

    public GameObject ItemBoxPrepab;


    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.onBuy += DropItemBox;
    }
    IInteraction temp;
    void DropItemBox()
    {
        StartCoroutine(Drop());
    }
    IEnumerator Drop()
    {
        yield return new WaitForSeconds(3f);
        GameObject itemTemp = Instantiate(ItemBoxPrepab);
         temp = itemTemp.GetComponent<IInteraction>();
        temp.request += DropItemBox;
    }
}
