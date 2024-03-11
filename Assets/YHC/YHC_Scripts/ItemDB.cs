using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDB", menuName = "Scriptable Object/ItemDB", order = int.MaxValue)]
public class ItemDB: ScriptableObject
{
    [Header("아이템 데이터베이스")]
    public string itemName = "_";
    public float weight;
    public int price;
    public GameObject itemModel;
    public Sprite itemIcon;
    public string itemText = "아이템 정보";

}
