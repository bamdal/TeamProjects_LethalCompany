using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : WeaponBase, IEquipable
{
    uint damage = 0;
    public uint Damage => damage;

    float weight;
    public float Weight => weight;

    private void OnEnable()
    {
        weight = weaponData.weight;
    }
    private void Start()
    {
    }

    public void Equip()
    {

    }

    public void Use()
    {
        
    }
}
