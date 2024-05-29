using System;
using UnityEngine;

public class ItemBox : MonoBehaviour, IInteraction
{
    GameManager gameManager;

    public Action requestItem;

    Transform[] itemLocation;

    bool isOpen = false;
    Rigidbody rb;
    public Action onRequest { get; set; }

    void Awake()
    {
        itemLocation = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            itemLocation[i] = transform.GetChild(i);
        }
        transform.position = new Vector3(0, 1, 0);
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    public void Interaction(GameObject target)
    {
        if (!isOpen)
        {
            int temp = gameManager.ItemsQueue.Count;
            if (temp > 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    Factory.Instance.GetItem(gameManager.ItemsQueue.Dequeue(), itemLocation[i].position);
                }

            }
            else
            {
                for (int i = 0; i < temp; i++)
                {
                    Factory.Instance.GetItem(gameManager.ItemsQueue.Dequeue(), itemLocation[i].position);
                    Debug.Log(gameManager.ItemsQueue.Count);
                }
            }
            if (gameManager.ItemsQueue.Count > 0)
            {
                onRequest?.Invoke();
            }
        }
        rb.AddForce(new Vector3(UnityEngine.Random.Range(0, 1), UnityEngine.Random.Range(0, 1), UnityEngine.Random.Range(0, 1)) * 1000f, ForceMode.Impulse);
        isOpen = true;
        Destroy(this.transform.gameObject, 1f);
        Debug.Log(gameManager.ItemsQueue.Count);
    }
}
