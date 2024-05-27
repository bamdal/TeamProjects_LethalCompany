using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    bool IsUsable { get => CurrentBattery > 0; }

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
        targetEnemy.OnDebuff(stunnedTime);
        effectPrefab.gameObject.SetActive(true);
        effectPrefab.transform.position = targetEnemy.transform.position;
        yield return new WaitForSeconds(stunnedTime);
        effectPrefab.gameObject.SetActive(false);
    }

    public ItemDB GetItemDB()
    {
        return zapGunData;
    }
}
