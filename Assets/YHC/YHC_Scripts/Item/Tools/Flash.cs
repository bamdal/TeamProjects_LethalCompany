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

    bool IsAvailable => currentBattery > 0;
    bool isActivated = false;

    float maxBattery;

    /// <summary>
    /// 후레쉬의 무게
    /// </summary>
    float weight;
    public float Weight => weight;

    public Action<float> onBatteryChange;

    PlayerInputActions inputActions;
    Transform lightTransform;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        lightTransform = transform.GetChild(0);

        flashData = GameManager.Instance.ItemData.GetItemDB(ItemCode.FlashLight);

        maxBattery = flashData.battery;
        CurrentBattery = maxBattery;
        weight = flashData.weight;
    }

    private void Start()
    {
        lightTransform.gameObject.SetActive(true);
        isActivated = true;
    }


    private void Update()
    {
        if(lightTransform.gameObject)
        {
            CurrentBattery -= Time.deltaTime;
        }
    }

    public void Equip()
    {

    }

    public void Use()
    {
        if(isActivated)
        {
            // 켜져있다.
            lightTransform.gameObject.SetActive(false);  // 불 끄기
            isActivated = false;

        }
        else
        {
            // 꺼져있다.
            lightTransform.gameObject.SetActive(true);   // 불 켜기
            isActivated= true;
        }
    }

    public ItemDB GetItemDB()
    {
        return flashData;
    }
}
