using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hardware_Barrel : ItemBase, IItemDataBase
{
    /*[SerializeField]
    private ItemDB hardware1;
    public ItemDB ItemDB { set { hardware1 = value; } }*/

    //public ItemDB itemDB;
    public ItemDB GetItemDB()
    {
        return itemDB;
    }

    //Transform childTransform;

    //private void Start()
    //{
    //    // 자식 오브젝트가 존재하는 경우에만 실행
    //    childTransform = transform.GetChild(0);
    //}

    //private void FixedUpdate()
    //{
    //    transform.position = childTransform.position;
    //}
}
