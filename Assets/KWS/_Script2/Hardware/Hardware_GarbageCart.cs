using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hardware_GarbageCart : ItemBase, IItemDataBase
{
    //public ItemDB itemDB;
    public ItemDB GetItemDB()
    {
        return GameManager.Instance.ItemData.GetItemDB(ItemCode.GarbageCart);
    }
}
