using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flash : ToolBase, IEquipable, IItemDataBase
{
    /// <summary>
    /// 후레쉬의 데이터
    /// </summary>
    ItemDB flashData;

    // 배터리 관련 ----------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 현재 남아있는 배터리
    /// </summary>
    float currentBattery;

    /// <summary>
    /// 배터리 확인, 설정용 프로퍼티
    /// </summary>
    public float CurrentBattery
    {
        get => currentBattery;
        set
        {
            if(currentBattery != value)
            {
                currentBattery = value;
                currentBattery = Math.Clamp(value, 0, maxBattery);
                onBatteryChange?.Invoke(currentBattery / maxBattery);
            }
        }
    }

    /// <summary>
    /// 최대 배터리 용량
    /// </summary>
    float maxBattery;

    /// <summary>
    /// 배터리 변화를 알리는 델리게이트
    /// </summary>
    public Action<float> onBatteryChange;

    /// <summary>
    /// 배터리가 사용 가능한 상태인지 아닌지 확인할 프로퍼티
    /// </summary>
    bool IsAvailable => CurrentBattery > 0;

    /// <summary>
    /// 손전등이 켜진 상태, 꺼져있는 상태가 기본
    /// </summary>
    bool isActivated = false;
    bool IsActivated
    {
        get => isActivated;
        set
        {
            /*if (IsActivated != value)
            {
                IsActivated = value;                
            }*/
            if(isActivated != value)
            {
                isActivated = value;
            }
        }
    }

    /// <summary>
    /// 후레쉬의 무게
    /// </summary>
    float weight;
    public float Weight => weight;

    Light lightComp;

    private void Awake()
    {
        lightComp = GetComponentInChildren<Light>();
    }

    private void Start()
    {
        flashData = GameManager.Instance.ItemData.GetItemDB(ItemCode.FlashLight);

        maxBattery = flashData.battery;
        CurrentBattery = maxBattery;
        weight = flashData.weight;

        IsActivated = false;
        lightComp.enabled = IsActivated;
    }


    private void Update()
    {
        if(IsActivated)
        {
            CurrentBattery -= Time.deltaTime;
        }
    }

    public void Equip()
    {

    }

    public void Use()
    {
        if (IsAvailable)
        {
            lightComp.enabled = !lightComp.enabled;
            IsActivated = lightComp.enabled;
/*            string temp = IsActivated ? "켜짐" : "꺼짐";
            Debug.Log($"{temp}");*/
        }
    }

    public ItemDB GetItemDB()
    {
        return flashData;
    }
}
