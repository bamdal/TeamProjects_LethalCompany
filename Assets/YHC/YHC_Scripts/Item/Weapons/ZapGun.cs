using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZapGun : WeaponBase, IEquipable, IItemDataBase
{
    enum GunState
    {
        Load = 0,
        Scan,
        Release,
        Shot
    }

    GunState state = GunState.Load;

    /// <summary>
    /// 총의 데이터
    /// </summary>
    ItemDB zapGunData;

    float stunnedTick = 1.0f;

    float currentBattery;
    float CurrentBattery
    {
        get => currentBattery;
        set
        {
            if(currentBattery != value)
            {
                currentBattery = value;
            }
        }
    }

    float MaxBattery;

    EnemyBase targetEnemy;

    private void Awake()
    {
        MaxBattery = zapGunData.battery;
    }

    private void Start()
    {
        zapGunData = GameManager.Instance.ItemData.GetItemDB(ItemCode.ZapGun);
    }

    private void Update()
    {
        if(state == GunState.Shot)
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ObjectScan();
    }

    public void Equip()
    {

    }

    public void Use()
    {
        switch(state)
        {
            case GunState.Load:
                state = GunState.Scan;
                break;
            case GunState.Scan:
                if(ObjectScan())
                {
                    state = GunState.Release;
                }
                else
                {
                    state = GunState.Load;
                }
                break;
            case GunState.Release:
                state = GunState.Shot;
                break;
            case GunState.Shot:
                state = GunState.Load;
                break;
                
        }
    }

    bool ObjectScan()
    {
        bool result = false;
        Collider[] colliders = Physics.OverlapBox(transform.position + transform.forward * 5.0f, Vector3.one * 0.5f);
        targetEnemy = null;

        foreach(Collider collider in colliders)
        {
            targetEnemy = collider.GetComponent<EnemyBase>();
            if(targetEnemy != null)
            {
                result = true;
                break;
            }
        }

        return result;
    }

    void Shot()
    {

    }

    void Release()
    {
        if(targetEnemy != null)
        {
            // if(damageTick < 0.0f && battery > 0.0f)
            // {
            //     attackTarget.Defense(damage);
            //     damageTick = 1.0f;
            //     battery -= 20.0f;
            //     targetEnemy.onDebuffAttack?.Invoke();
            // }
        }
    }

    public ItemDB GetItemDB()
    {
        return zapGunData;
    }
}
