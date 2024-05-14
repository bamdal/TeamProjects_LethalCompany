using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZapGun : WeaponBase, IEquipable, IItemDataBase
{
    int enemyStunnedTime = 15;

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

    /// <summary>
    /// 배터리 잔량
    /// </summary>
    float currentBattery;
    float CurrentBattery
    {
        get => currentBattery;
        set
        {
            if(currentBattery != value)
            {
                currentBattery = value;
                onBatterChange?.Invoke(currentBattery / MaxBattery);
            }
        }
    }

    float MaxBattery;

    public Action<float> onBatterChange;

    float batteryUse = 5.0f;

    EnemyBase targetEnemy;

    private void Awake()
    {
    }

    private void Start()
    {
        zapGunData = GameManager.Instance.ItemData.GetItemDB(ItemCode.ZapGun);
        MaxBattery = zapGunData.battery;
    }

    private void Update()
    {
        if(state == GunState.Shot)
        {
            CurrentBattery -= Time.deltaTime * batteryUse;
        }
    }

    public void Equip()
    {

    }

    public void Use()
    {
        switch(state)
        {
            case GunState.Load:
                targetEnemy = null;
                state = GunState.Scan;
                break;
            case GunState.Scan:
                if(ObjectScan())
                {
                    state = GunState.Release;
                    Release();
                }
                else
                {
                    state = GunState.Load;
                }
                break;
            case GunState.Release:
                state = GunState.Shot;
                Shot();
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
        if (targetEnemy != null)
        {
            StartCoroutine(EnemyStunned(enemyStunnedTime));
        }
    }

    void Release()
    {
        Debug.Log("적 스캔 완료");

    }

    IEnumerator EnemyStunned(int stunnedTime)
    {
        while (true)
        {
            targetEnemy.OnDebuff(stunnedTime);
            yield return new WaitForSeconds(stunnedTime);
        }
    }

    public ItemDB GetItemDB()
    {
        return zapGunData;
    }
}
