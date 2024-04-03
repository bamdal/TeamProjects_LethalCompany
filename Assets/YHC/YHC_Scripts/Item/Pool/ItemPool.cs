using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : ObjectPool<ItemBase>
{
    public ItemDB[] itemDBs;

    protected override void GenerateRecycleObjects(int startIndex, int endIndex, ItemBase[] expandedPool)
    {
        recycleObject = itemDBs[(int)ItemCode.FlashLight_0].itemPrefab;
        base.GenerateRecycleObjects(startIndex, endIndex, expandedPool);
    }
}
