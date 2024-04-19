using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZapGun : WeaponBase, IEquipable
{
    uint damage;
    public uint Damage => damage;

    float weight;
    public float Weight => weight;

    public float shootingRange = 15.0f;
    public float shootingAngle = 30.0f;

    private void OnEnable()
    {
        weight = weaponData.weight;
        damage = weaponData.damage;
    }

    public void Equip()
    {

    }

    public void Use()
    {
        
    }

    private void OnMouseDrag()
    {
        shootingAngle += Time.deltaTime;
    }

    void ObjectScan()
    {

    }
}
