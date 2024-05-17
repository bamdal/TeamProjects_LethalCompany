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

    public float attackCoolTime = 3.0f;
    float currentAttackCool = 0.0f;

    bool IsAttackAvailable { get => currentAttackCool < 0.0f; }
    
    /// <summary>
    /// 삽이 공격할 대상
    /// </summary>
    IBattler target = null;

    ShovelHead head;

    /// <summary>
    /// 공격부위의 콜라이더
    /// </summary>
    BoxCollider AttackArea;

    /// <summary>
    /// 삽 자체의 콜라이더
    /// </summary>
    CapsuleCollider shovelCoillder;

    /// <summary>
    /// 삽 리지드바디(키네마틱으로 사용)
    /// </summary>
    Rigidbody rigid;

    // 애니메이션용
    Animator anim;
    readonly int AttackHash = Animator.StringToHash("Attack");


    private void Start()
    {
        head = GetComponentInChildren<ShovelHead>();
        rigid = GetComponent<Rigidbody>();
        shovelCoillder = GetComponent<CapsuleCollider>();
        AttackArea = transform.GetChild(0).GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();

        AttackArea.enabled = false;

        shovelData = GameManager.Instance.ItemData.GetItemDB(ItemCode.Shovel);
        weight = shovelData.weight;
        damage = shovelData.damage;

        head.onShovelTiggerOn += HeadTriggerOn;
        head.onShovelTiggerOff += HeadTriggerOff;
    }

    public void Use()
    {
        if (IsAttackAvailable)
        {
            anim.SetTrigger(AttackHash);
        }
    }

    void ColliderEnable()
    {
        AttackArea.enabled = true;
        currentAttackCool = attackCoolTime;
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

    public void Equip()
    {
    }

    /// <summary>
    /// 공격부위 콜라이더에 적이 들어왔을 때 실행될 함수
    /// </summary>
    private void HeadTriggerOn(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            target = collider.GetComponent<IBattler>();
            if (target != null)
            {
                target.Defense(damage);
            }
        }
    }

    /// <summary>
    /// 공격부위 콜라이더에서 적이 나갔을 때 실행될 함수
    /// </summary>
    private void HeadTriggerOff(Collider collider)
    {
        if(collider.CompareTag("Enemy"))
        {
            target = collider.GetComponent<IBattler>();
            if (target != null)
            {
                target = null;
            }
        }
    }
}
