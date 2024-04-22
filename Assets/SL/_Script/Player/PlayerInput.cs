using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    /// <summary>
    /// 이동 입력을 전달하는 델리게이트(파라메터 : 이동방향, 누른상황인지 아닌지(true면 눌렀다))
    /// </summary>
    public Action<Vector2, bool> onMove;

    /// <summary>
    /// 이동 모드 변경 입력을 전달하는 델리게이트
    /// </summary>
    public Action onMoveModeChange;


    public Action onJump;

    public Action<bool> onLClick;

    public Action onRClick;

    public Action<bool> onInteract;

    // 입력용 인풋 액션
    PlayerInputActions inputActions;

    public Action<Vector2> onScroll;
    public Action onItemDrop;

    public Action onOutTerminal;
    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.MoveModeChange.performed += OnMoveModeChange;
        inputActions.Player.Interact.performed += OnInteract;
        inputActions.Player.Interact.canceled += OnInteract;
        inputActions.Player.MouseLClick.performed += OnLClick;
        inputActions.Player.MouseLClick.canceled += OnLClick;
        inputActions.Player.MouseRClick.performed += OnRClick;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Wheel.performed += OnScroll;
        inputActions.Player.ItemDrop.performed += OnItemDrop;
    }


    private void OnDisable()
    {
        inputActions.Player.ItemDrop.performed -= OnItemDrop;
        inputActions.Player.Wheel.performed -= OnScroll;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.MouseRClick.performed -= OnRClick;
        inputActions.Player.MouseLClick.canceled -= OnLClick;
        inputActions.Player.MouseLClick.performed -= OnLClick;
        inputActions.Player.Interact.canceled -= OnInteract;
        inputActions.Player.Interact.performed -= OnInteract;
        inputActions.Player.MoveModeChange.performed -= OnMoveModeChange;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }
    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector2>();
        onMove?.Invoke(input, !context.canceled);
    }

    private void OnMoveModeChange(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        onMoveModeChange?.Invoke();
    }

    private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        onJump?.Invoke();
    }
    private void OnRClick(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        onRClick?.Invoke();
    }
    private void OnLClick(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        onLClick?.Invoke(!_.canceled);
    }

    private void OnInteract(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        onInteract?.Invoke(_.performed);
    }
    private void OnScroll(InputAction.CallbackContext context)
    {
        onScroll?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnItemDrop(InputAction.CallbackContext context)
    {
        onItemDrop?.Invoke();
    }
    
    public void OffInputActions()
    {
        inputActions.Player.Disable();
    }
}
