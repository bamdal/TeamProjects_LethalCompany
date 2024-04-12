using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{   
    Player player;

    private void Awake()
    {
        player = GameManager.Instance.Player;
    }

    private void Update()
    {

    }
}
