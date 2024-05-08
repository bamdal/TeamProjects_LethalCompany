using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : WeaponBase, IEquipable, IItemDataBase, IBattler
{
    ItemDB shovelData;

    uint damage = 0;
    public uint Damage => damage;

    float weight;
    public float Weight => weight;

    BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    private void Start()
    {
        shovelData = GameManager.Instance.ItemData.GetItemDB(ItemCode.Shovel);
        weight = shovelData.weight;
        damage = shovelData.damage;
    }


    public void Equip()
    {
        
    }

    public void Use()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            IBattler target = other.GetComponent<IBattler>();
            if(target != null)
            {
                Attack(target);
            }
        }
    }


    public ItemDB GetItemDB()
    {
        return shovelData;
    }

    public void Attack(IBattler target)
    {
        throw new System.NotImplementedException();
    }

    public void Defense(float attackPower)
    {
        throw new System.NotImplementedException();
    }
}
