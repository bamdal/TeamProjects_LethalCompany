using System;
using UnityEngine;

public class ItemDrop : MonoBehaviour, IInteraction
{
    GameManager gameManager;
    ItemDB itemDatabase;
    ItemDataManager itemDataManager;
    ItemCode[] tempItemCode;

    public GameObject ItemBoxPrepab;

    public Action requestItem;

    public Action request { get; set; }

    Transform[] itemLocation;
    void Awake()
    {
        tempItemCode = new ItemCode[4];
        itemLocation = new Transform[4];
        for(int i = 0; i < 4; i++)
        {
            itemLocation[i] = transform.GetChild(i);
        }
    }
    
    private void Start()
    {
        gameManager = GameManager.Instance;
        itemDataManager = GameManager.Instance.ItemData;
    }

    public void Interaction(GameObject target)
    {
        if (gameManager.ItemsQueue.Count > 4)
        {
            for( int i = 0; i < 4; i++)
            {
                tempItemCode[i] = gameManager.ItemsQueue.Dequeue();
            }
            
        }
        else
        {
            foreach (var item in gameManager.ItemsQueue)
            {
                int i = 0;
                tempItemCode[i] = item;
            }
        }
        foreach (var itemCode in tempItemCode)
        {
            
        }
    }
}
