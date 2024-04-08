using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{   
    Player player;
    Transform[] ItemBox = new Transform[4];

    private void Awake()
    {
        player = GameManager.Instance.Player;
    }

    public void FindItem()
    {
    }
}
