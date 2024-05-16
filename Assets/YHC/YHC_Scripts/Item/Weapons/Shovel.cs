using System;
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

    BoxCollider AttackArea;
    Animator anim;
    readonly int AttackHash = Animator.StringToHash("Attack");


    private void Start()
    {
        AttackArea = GetComponentInChildren<BoxCollider>();
        anim = GetComponent<Animator>();

        AttackArea.enabled = false;

        shovelData = GameManager.Instance.ItemData.GetItemDB(ItemCode.Shovel);
        weight = shovelData.weight;
        damage = shovelData.damage;
    }

    public void Equip()
    {
        
    }

    public void Use()
    {
        anim.SetTrigger(AttackHash);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            IBattler target = other.GetComponent<IBattler>();
            if(target != null)
            {
                target.Defense(Damage);
            }
        }
    }

    void ColliderEnable()
    {
        AttackArea.enabled = true;
    }

    void ColliderDisable()
    {
        AttackArea.enabled = false;
    }

    public void Attack(IBattler target)
    {

    }

    public void Defense(float attackPower)
    {

    }

    public ItemDB GetItemDB()
    {
        return shovelData;
    }
}
