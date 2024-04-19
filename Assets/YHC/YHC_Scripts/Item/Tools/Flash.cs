using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flash : ToolBase, IEquipable, IItemDataBase
{
    public float currentBattery;
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

    float weight;
    public float Weight => weight;

    // float rotateSpeed = 60.0f;
    // int rotateDirection = 1;

    public Action<float> onBatteryChange;

    PlayerInputActions inputActions;
    Transform lightTransform;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        lightTransform = transform.GetChild(0);


        maxBattery = toolData.battery;
        CurrentBattery = maxBattery;
        weight = toolData.weight;
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
        // float rotateValue = Time.deltaTime * rotateDirection;
        // if (rotateValue > 60)
        // {
        //     rotateDirection = -1;
        // }
        // 
        // if (rotateValue < -60)
        // {
        //     rotateDirection = 1;
        // }
        // lightTransform.rotation = Quaternion.Euler(-90 + rotateValue, 0, 0);
    }
    
    /// <summary>
    /// 임시용 인풋시스템
    /// </summary>
    private void OnEnable()
    {
        inputActions.Player.Interact.Enable();
        inputActions.Player.Interact.performed += OnUse;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        inputActions.Player.Interact.performed -= OnUse;
        inputActions.Player.Interact?.Disable();
    }

    /// <summary>
    /// 임시용 인풋시스템 실제 처리는 플레이어에서 델리게이트 신호받아 사용 예정
    /// </summary>
    /// <param name="context"></param>
    private void OnUse(InputAction.CallbackContext context)
    {
        if(IsAvailable)
        {
            Use();
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

    // private void OnMouseDrag()
    // {
    //     float rotateValue = Time.deltaTime * rotateSpeed * rotateDirection;
    //     if(rotateValue > 60)
    //     {
    //         rotateDirection = -1;
    //     }
    //     
    //     if(rotateValue < -60) 
    //     {
    //         rotateDirection = 1;
    //     }
    //     lightTransform.rotation = Quaternion.Euler(-90 + rotateValue, 0, 0);
    // }

    public ItemDB GetItemDB()
    {
        return toolData;
    }
}
