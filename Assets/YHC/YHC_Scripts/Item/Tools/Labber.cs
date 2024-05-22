using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Labber : ItemBase
{
    ItemDB labberDB;

    TextMeshProUGUI stateText;

    private void Start()
    {
        // labberDB = ItemDataManager.Instance.GetItemDB(ItemCode.Labber);
    }


    public ItemDB GetItemDB()
    {
        return labberDB;
    }
}
