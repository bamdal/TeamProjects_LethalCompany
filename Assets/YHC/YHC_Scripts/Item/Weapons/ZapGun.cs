using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class ZapGun : WeaponBase, IEquipable, IItemDataBase
{
    int enemyStunnedTime = 15;
    Transform effectPrefab;

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
            if (currentBattery != value)
            {
                currentBattery = value;
                currentBattery = Mathf.Clamp(value, 0, MaxBattery);
                if (currentBattery == 0)
                {
                    state = GunState.Load;
                    StopAllCoroutines();
                }
                onBatterChange?.Invoke(currentBattery / MaxBattery);

            }
        }
    }

    /// <summary>
    /// 잽건이 사용 가능한 상태인지 확인하는 프로퍼티 -> 배터리가 0보다 많으면 사용가능
    /// </summary>
    bool IsUsable { get => CurrentBattery > 0; }

    /// <summary>
    /// 잽건의 이펙트가 켜진상태
    /// </summary>
    bool effectActivate = false;

    float MaxBattery;

    public Action<float> onBatterChange;

    float batteryUse = 5.0f;

    EnemyBase targetEnemy;
    private void Start()
    {
        zapGunData = GameManager.Instance.ItemData.GetItemDB(ItemCode.ZapGun);
        MaxBattery = zapGunData.battery;
        currentBattery = MaxBattery;
        effectPrefab = transform.GetChild(3);
        effectPrefab.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (state == GunState.Shot)
        {
            CurrentBattery -= Time.deltaTime * batteryUse;
        }

        if (effectActivate)
        {
            effectPrefab.gameObject.SetActive(true);
            if (targetEnemy != null)
            {
                effectPrefab.transform.position = targetEnemy.transform.position;
            }
        }
    }

    public void Equip()
    {

    }

    public void Use()
    {
        switch (state)
        {
            case GunState.Load:
                Debug.Log("로딩");
                targetEnemy = null;
                if (IsUsable)
                {
                    state = GunState.Scan;
                    Debug.Log("스캔");
                }
                break;
            case GunState.Scan:
                Debug.Log("로딩 스캔");
                if (ObjectScan())
                {
                    state = GunState.Release;
                    Debug.Log("릴리즈");
                    Release();
                }
                else
                {
                    state = GunState.Load;
                }
                break;
            case GunState.Release:
                Debug.Log("샷");
                state = GunState.Shot;
                Shot();
                break;
            case GunState.Shot:
                Debug.Log("load");
                state = GunState.Load;
                break;

        }
    }

    bool ObjectScan()
    {
        bool result = false;
        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward * 3.0f, 3.0f);
        targetEnemy = null;

        foreach (Collider collider in colliders)
        {
            targetEnemy = collider.GetComponent<EnemyBase>();
            if (targetEnemy != null)
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
            StopAllCoroutines();
            StartCoroutine(EnemyStunned(enemyStunnedTime));
        }
    }

    void Release()
    {
        Debug.Log("적 스캔 완료");
    }

    IEnumerator EnemyStunned(int stunnedTime)
    {
        NavMeshAgent agent = targetEnemy.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            targetEnemy.OnDebuff(agent, stunnedTime);
        }

        effectActivate = true;      // 이펙트 켜졌다고 표시
        yield return new WaitForSeconds(stunnedTime);
        effectPrefab.gameObject.SetActive(false);   // 이펙트 끄기
        effectActivate = false;     // 이펙트 꺼졌다고 표시
    }

    public ItemDB GetItemDB()
    {
        return zapGunData;
    }
}
