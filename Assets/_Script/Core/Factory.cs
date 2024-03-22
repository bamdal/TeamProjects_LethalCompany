using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
}

public class Factory : Singleton<Factory>
{
    //SlimePool slimePool;
    ItemPool itemPool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        //slimePool = GetComponentInChildren<SlimePool>();
        //if( slimePool != null ) slimePool.Initialize();
        itemPool = GetComponentInChildren<ItemPool>();
        if(itemPool != null )
                itemPool.Initialize();
    }
 
    /// <summary>
    /// 풀에 있는 게임 오브젝트 하나 가져오기
    /// </summary>
    /// <param name="type">가져올 오브젝트의 종류</param>
    /// <param name="position">오브젝트가 배치될 위치</param>
    /// <param name="angle">오브젝트의 초기 각도</param>
    /// <returns>활성화된 오브젝트</returns>
    public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? euler = null)
    {
        GameObject result = null;
        // switch (type)
        // {
        //     
        // }

        return result;
    }

    public ItemBase GetItem()
    {
        return itemPool.GetObject();
    }

    public ItemBase GetItem(Vector3 position, float angle = 0.0f)
    {
        return itemPool.GetObject(position, angle * Vector3.forward);
    }
}