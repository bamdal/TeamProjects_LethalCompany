using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flash : MonoBehaviour, IEquipable
{
    public Tool tool;
    bool isActivate = true;
    public bool IsActivate
    {
        get => isActivate;
        set
        {
            if(isActivate != value)
            {
                isActivate = value;
            }
        }
    }
    new Light light;

    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        light = GetComponentInChildren<Light>();
    }

    /// <summary>
    /// 임시용 인풋시스템
    /// </summary>
    private void OnEnable()
    {
        inputActions.Player.Interact.Enable();
        inputActions.Player.Interact.performed += OnUse;
    }

    private void OnDisable()
    {
        inputActions.Player.Interact.performed -= OnUse;
        inputActions.Player.Interact.Disable();
    }

    /// <summary>
    /// 임시용 인풋시스템 실제 처리는 플레이어에서 델리게이트 신호받아 사용 예정
    /// </summary>
    /// <param name="context"></param>
    private void OnUse(InputAction.CallbackContext context)
    {
        Use();
    }
    public void Equip()
    {
    }

    public void Use()
    {
        if (IsActivate)
        {
            // 켜져있다.
            IsActivate = false;     // 끄기
            light.enabled = false;

        }
        else
        {
            // 꺼져있다.
            IsActivate = true;      // 켜기
            light.enabled = true;
        }
    }
}
