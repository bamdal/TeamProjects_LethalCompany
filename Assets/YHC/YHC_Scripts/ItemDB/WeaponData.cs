using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDB", menuName = "Scriptable Object/Weapon", order = 2)]
public class WeaponData : EquipmentDB
{
    [Header("무기 데이터")]
    public uint damage;
}
