using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flash_Up : ToolBase, IEquipable, IItemDataBase
{
    /// <summary>
    /// 프로손전등의 데이터
    /// </summary>
    ItemDB flashUpData;

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
            if (currentBattery != value)
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

    bool IsAvailable => currentBattery > 0;
    bool isActivated = false;


    /// <summary>
    /// 후레쉬의 무게
    /// </summary>
    float weight;
    public float Weight => weight;


    Transform lightTransform;

    private void Awake()
    {
        lightTransform = transform.GetChild(0);

        flashUpData = GameManager.Instance.ItemData.GetItemDB(ItemCode.FlashLightUp);

        maxBattery = flashUpData.battery;
        CurrentBattery = maxBattery;
        weight = flashUpData.weight;
    }

    private void Start()
    {
        lightTransform.gameObject.SetActive(true);
        isActivated = true;
    }


    private void Update()
    {
        if (lightTransform.gameObject)
        {
            CurrentBattery -= Time.deltaTime;
        }
    }

    public void Equip()
    {

    }

    public void Use()
    {
        if (isActivated)
        {
            // 켜져있다.
            lightTransform.gameObject.SetActive(false);  // 불 끄기
            isActivated = false;

        }
        else
        {
            // 꺼져있다.
            lightTransform.gameObject.SetActive(true);   // 불 켜기
            isActivated = true;
        }
    }

    public ItemDB GetItemDB()
    {
        return flashUpData;
    }
}
