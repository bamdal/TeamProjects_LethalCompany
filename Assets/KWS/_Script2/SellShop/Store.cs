using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Store : MonoBehaviour
{
    /*/// <summary>
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
        //inputActions.Hardware.Interactions.canceled += OnInteractionEnd;    // Hardware액션 맵의 Interactions액션에 OnInteractionEnd 함수를 연결(땠을 때만)
    }

    private void OnDisable()
    {
        //inputActions.Hardware.Interactions.canceled -= OnInteractionEnd;    // 연결 해제
        inputActions.Hardware.Interactions.performed -= OnInteractionStart; // 연결 해제
        inputActions.Hardware.Disable();
    }


    public void OnInteractionStart(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            Debug.Log($"F 키가 눌림");
        }
    }

    private void OnInteractionEnd(InputAction.CallbackContext _)
    {
        throw new NotImplementedException();
    }*/




    // private Player player;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        /*player = GetComponent<Player>(); // 더 나은 방법을 사용하도록 수정 가능

        if (player != null)
        {
            // F 액션에 대한 콜백 설정
            player.FAction.performed += OnFKeyPressed;
        }*/
    }

    private void OnFKeyPressed(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            Debug.Log("B: F 키가 눌렸습니다!");
            // 여기에 F 키가 눌렸을 때 이 스크립트에서 수행할 동작을 추가
        }
    }

    private void OnDisable()
    {
        /*if (player != null)
        {
            // 콜백 연결 해제
            player.FAction.performed -= OnFKeyPressed;
        }*/
    }


}
