using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ladder : ItemBase
{
    ItemDB ladderDB;

    TextMeshProUGUI stateText;

    private void Start()
    {
        ladderDB = ItemDataManager.Instance.GetItemDB(ItemCode.Ladder);
    }


    public ItemDB GetItemDB()
    {
        return ladderDB;
    }
}
