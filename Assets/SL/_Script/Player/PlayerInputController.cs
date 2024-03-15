using Cinemachine;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    /// <summary>
    /// 걷는 속도
    /// </summary>
    public float walkSpeed = 3.0f;

    /// <summary>
    /// 달리는 속도
    /// </summary>
    public float runSpeed = 5.0f;

    /// <summary>
    /// 현재 속도
    /// </summary>
    float currentSpeed = 0.0f;

    /// <summary>
    /// 이동 모드
    /// </summary>
    enum MoveMode
    {
        Walk = 0,   // 걷기 모드
        Run         // 달리기 모드
    }

    Vector3 mouseDir = Vector3.zero;

    Quaternion cameraRotation = Quaternion.identity;

    /// <summary>
    /// 현재 이동 모드
    /// </summary>
    MoveMode currentMoveMode = MoveMode.Run;

    /// <summary>
    /// 현재 이동 모드 확인 및 설정용 프로퍼티
    /// </summary>
    MoveMode CurrentMoveMode
    {
        get => currentMoveMode;
        set
        {
            currentMoveMode = value;    // 상태 변경
            if (currentSpeed > 0.0f)     // 이동 중인지 아닌지 확인
            {
                // 이동 중이면 모드에 맞게 속도와 애니메이션 변경
                MoveSpeedChange(currentMoveMode);
            }
        }
    }

    /// <summary>
    /// 입력된 이동 방향
    /// </summary>
    Vector3 inputDirection = Vector3.zero;  // y는 무조건 바닥 높이

    /// <summary>
    /// 캐릭터의 목표방향으로 회전시키는 회전
    /// </summary>
    Quaternion targetRotation = Quaternion.identity;

    /// <summary>
    /// 캐릭터 회전 속도
    /// </summary>
    public float turnSpeed = 10.0f;

    // 컴포넌트들
    CharacterController characterController;


    // 입력용 인풋 액션
    PlayerInputActions inputActions;
    public Camera cam;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();
        Transform child = transform.Find("Main Camera");
        cam = child.GetComponent<Camera>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.MoveModeChange.performed += OnMoveModeChange;
        inputActions.Player.Interact.performed += OnInteract;
        inputActions.Player.Interact.canceled += OnInteract;

    }


    private void OnDisable()
    {
        inputActions.Player.Interact.canceled -= OnInteract;
        inputActions.Player.Interact.performed -= OnInteract;
        inputActions.Player.MoveModeChange.performed -= OnMoveModeChange;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        // 이동 방향 계산
        Vector3 moveDirection = CalculateMoveDirection();

        // 이동 처리
        characterController.Move(Time.deltaTime * currentSpeed * moveDirection);
        // 아이템을 상호작용하는 함수 호출
        FindItemRay();
    }

    /// <summary>
    /// 바라보고있는 오브젝트가 아이템이면 반응하는 함수
    /// </summary>
    private void FindItemRay()
    {
        // LayerMask를 설정하여 item 레이어만 검출하도록 합니다.
        int layerMask = 1 << LayerMask.NameToLayer("Item");

        // 카메라의 정 중앙을 기준으로 레이를 쏩니다.
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Physics.Raycast 메서드에 layerMask를 추가하여 해당 레이어만 검출하도록 합니다.
        if (Physics.Raycast(ray, out hit, 5.0f, layerMask))
        {
            Debug.Log("아이템입니다.");

            // 레이를 그려줍니다.
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
        }
        else
        {
            Debug.Log("레이캐스트 실패!");

            // 실패 시에도 레이를 그려줍니다.
            Debug.DrawRay(ray.origin, ray.direction * 5.0f, Color.red);
        }
    }


    /// <summary>
    /// 카메라의 방향을 기준으로 이동 방향을 계산합니다.
    /// </summary>
    /// <returns>이동 방향</returns>
    private Vector3 CalculateMoveDirection()
    {
        Vector3 cameraForward = cam.transform.forward;
        Vector3 cameraRight = cam.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;
        moveDirection.Normalize();

        return moveDirection;
    }




    /// <summary>
    /// 이동 입력 처리용 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector2>();
        inputDirection.x = input.x;     // 입력 방향 저장
        inputDirection.y = 0;
        inputDirection.z = input.y;

        if (!context.canceled)
        {
            MoveSpeedChange(CurrentMoveMode);
        }
        else
        {
            // 입력을 끝낸 상황
            currentSpeed = 0.0f;    // 정지 시키기
        }
    }

    /// <summary>
    /// 이동 모드 변경용 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMoveModeChange(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (CurrentMoveMode == MoveMode.Walk)
        {
            CurrentMoveMode = MoveMode.Run;
        }
        else
        {
            CurrentMoveMode = MoveMode.Walk;
        }
    }

    /// <summary>
    /// 모드에 따라 이동 속도를 변경하는 함수
    /// </summary>
    /// <param name="mode">설정된 모드</param>
    void MoveSpeedChange(MoveMode mode)
    {
        switch (mode) // 이동 모드에 따라 속도와 애니메이션 변경
        {
            case MoveMode.Walk:
                currentSpeed = walkSpeed;
                break;
            case MoveMode.Run:
                currentSpeed = runSpeed;
                break;
        }
    }

    /// <summary>
    /// 상호작용 처리용 함수
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("f키 눌렀음!");

            // 카메라의 정 중앙을 기준으로 레이를 쏩니다.
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 3.0f))
            {
                if (hit.collider.CompareTag("Item"))
                {
                    Debug.Log(hit);
                    // 충돌한 객체를 자식으로 만듭니다.
                    hit.collider.transform.SetParent(transform);

                    // 자식으로 만든 객체를 비활성화합니다.
                    hit.collider.gameObject.SetActive(false);
                    Debug.Log("아이템을 획득했습니다!");

                }
            }
            else
            {
                Debug.Log("레이캐스트 실패!");
            }
        }
        if (context.canceled)
        {
            Debug.Log("f키 떨어짐!");
        }
    }
}