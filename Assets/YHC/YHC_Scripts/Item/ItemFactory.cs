using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : Factory
{
    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

    public GameObject SpawnItem(ItemCode itemCode)
    {
        ItemDB data = GameManager.Instance.ItemData[itemCode];
        // ItemObject obj = itemPool.GetObject();
        // obj.ItemData = data;                    // 풀에서 하나 꺼내고 데이터 설정
        return null;
    }
}
