using System;
using UnityEngine;

public class ItemDrop : MonoBehaviour, IInteraction
{
    GameManager gameManager;

    public GameObject ItemBoxPrepab;

    public Action requestItem;

    Transform[] itemLocation;

    bool isOpen = true;

    public Action request { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    void Awake()
    {
        itemLocation = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            itemLocation[i] = transform.GetChild(i);
        }
        transform.position = new Vector3(0, 100, 0);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    private void Update()
    {

    }
    public void Interaction(GameObject target)
    {
        isOpen = false;   
        if (gameManager.ItemsQueue.Count > 4)
        {
            for (int i = 0; i < 4; i++)
            {
                Factory.Instance.GetItem(gameManager.ItemsQueue.Dequeue(), itemLocation[i].position).transform.SetParent(itemLocation[i]);
            }

        }
        else
        {
            int count = 0;
            foreach (var item in gameManager.ItemsQueue)
            {
                Factory.Instance.GetItem(item, itemLocation[count].position).transform.SetParent(itemLocation[count]);
                count++;
            }
        }
    }
}
