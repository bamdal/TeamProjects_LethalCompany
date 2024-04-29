using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : Singleton<ItemDataManager>
{
    public ItemDB[] itemDataBases = null;

    // public ItemDB this[ItemCode code] => itemDataBases[(int)code];
    public ItemDB this[int index] => itemDataBases[index];

    // 아이템의 이름을 받아 해당 아이템의 ItemDB를 반환하는 메서드
    public ItemDB GetItemDB(string itemName)
    {
        // Resources 폴더에서 아이템 데이터 로드
        ItemDB itemDB = Resources.Load<ItemDB>($"ItemDB/{itemName}");

        if (itemDB != null)
        {
            return itemDB;
        }
        else
        {
            // 해당하는 아이템이 없는 경우
            Debug.LogWarning($"ItemDB에서 {itemName}을 찾을 수 없습니다.");
            return null;
        }
    }
    
    public ItemDB GetItemDB(ItemCode itemCode)
    {
        int index = Array.IndexOf(itemDataBases, itemCode);
        return itemDataBases[index];
    }
}
