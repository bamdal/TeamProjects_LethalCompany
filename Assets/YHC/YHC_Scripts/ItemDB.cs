using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Scrap,
    Equipment,
}

[CreateAssetMenu(fileName = "ItemDB", menuName = "Scriptable Object/ItemDB", order = int.MaxValue)]
public class ItemDB : ScriptableObject
{
    [Header("아이템 데이터베이스")]
    public string itemName = "_";
    public string itemText = "아이템 정보";
    public float weight = 0;
    public uint price = 0;
    public uint stackCount = 1;
    public float battery = 0;
    public Sprite itemIcon;
    public GameObject itemModel;
    public ItemType itemType;
}
