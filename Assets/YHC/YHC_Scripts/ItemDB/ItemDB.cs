using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Scrap,
    Equipment,
    Hardware,
}

public enum ItemCode
{
    Barrel = 0,
    CableDrum,
    GarbageCart,
    GasTank,
    PalletJack,
    FlashLight = 100,
    FlashLightUp,
    Shovel,
    ZapGun,
    Grenade,
    Labber,
}

[CreateAssetMenu(fileName = "new Item Data", menuName = "Scriptable Object/Item Data", order = 0)]
public class ItemDB : ScriptableObject
{
    [Header("아이템 공통 데이터")]
    public string itemName = "_";
    public string itemText = "아이템 정보";
    public float weight = 0;
    public uint price = 0;
    public uint stackCount = 1;
    public float battery = 0;
    public ItemType itemType;
    public ItemCode itemCode;
    public bool isConductive = false;
    public Sprite itemIcon;
    public GameObject itemModel;

    [Header("무기 관련 데이터")]
    public uint damage = 0;
}

