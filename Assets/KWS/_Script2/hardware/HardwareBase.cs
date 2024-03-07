using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HardwareBase : MonoBehaviour
{
    /// <summary>
    /// 폐철물의 무게
    /// </summary>
    [Range(10.0f, 50.0f)]
    public float hardwareWeight;

    /// <summary>
    /// 폐철물의 가격(min : 10, max : 50)
    /// </summary>
    [Range(10.0f, 50.0f)]
    public float hardwarePrice;

    /// <summary>
    /// 인풋 액션
    /// </summary>
    HardwareInputActions inputActions;

    private void Awake()
    {
        inputActions = new HardwareInputActions();      // 인풋 액션 생성
    }

    private void OnEnable()
    {
        inputActions.Hardware.Enable();
        inputActions.Hardware.Interactions.performed += OnInteractionStart; // Hardware액션 맵의 Interactions액션에 OnInteractionStart 함수를 연결(눌렀을 때만)
        inputActions.Hardware.Interactions.canceled += OnInteractionEnd;    // Hardware액션 맵의 Interactions액션에 OnInteractionEnd 함수를 연결(땠을 때만)
    }    

    private void OnDisable()
    {
        inputActions.Hardware.Interactions.canceled -= OnInteractionEnd;    // 연결 해제
        inputActions.Hardware.Interactions.performed -= OnInteractionStart; // 연결 해제
        inputActions.Hardware.Disable();
    }


    public void OnInteractionStart(InputAction.CallbackContext _)
    {
        throw new NotImplementedException();
    }

    private void OnInteractionEnd(InputAction.CallbackContext _)
    {
        throw new NotImplementedException();
    }

    private void hardwareSell()
    {

    }
}
