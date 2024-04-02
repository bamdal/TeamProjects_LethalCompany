using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 플레이어 체력
    /// </summary>
    public float hp = 100.0f;

    /// <summary>
    /// 플레이어 기력
    /// </summary>
    public float stamina = 100.0f;

    /// <summary>
    /// 플레이어 현재 기력
    /// </summary>
    private float currnetStamina = 0.0f;

    /// <summary>
    /// 걷는 속도
    /// </summary>
    public float walkSpeed = 3.0f;

    /// <summary>
    /// 달리는 속도
    /// </summary>
    public float runSpeed = 5.0f;

    /// <summary>
    /// 플레이어 점프 높이
    /// </summary>
    public float jumpForce = 5.0f;

    /// <summary>
    /// 걷는 동안의 스테미나 회복 속도
    /// </summary>
    private float staminaRecoveryRate = 10.0f;

    /// <summary>
    /// 달리는 동안의 스테미나 소모 속도
    /// </summary>
    private float staminaConsumptionRate = 10.0f;

    /// <summary>
    /// 스테미나 회복가능상태인지 여부
    /// </summary>
    bool isCanRecovery = true;

    /// <summary>
    /// 현재 속도
    /// </summary>
    [SerializeField]
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

    Transform inventory;

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
    /// 캐릭터 회전 속도
    /// </summary>
    public float turnSpeed = 10.0f;

    // 컴포넌트들
    CharacterController characterController;


    // 입력용 인풋 액션
    PlayerInput input;
    public Camera cam;

    /// <summary>
    /// 이동방향
    /// </summary>
    Vector3 moveDirection;

    Rigidbody rb;

//    Collider itemRader;
    public float groundCheckDistance = 1f;    // 바닥 체크 거리
    public LayerMask groundLayer;               // 바닥을 나타내는 레이어

    [SerializeField] private float gravityMultiplier = 2.0f;

    Transform itemRader;
    private void Awake()
    {
        input = GetComponent<PlayerInput>();

        input.onMove += OnMoveInput;
        input.onMoveModeChange += OnMoveModeChageInput;
        input.onInteract += OnInteractInput;
        input.onJump += OnJumpInput;
        input.onLClick += OnLClickInput;
        input.onRClick += OnRClickInput;
        cam = FindAnyObjectByType<Camera>();
        inventory = transform.Find("Inventory");
        currnetStamina = stamina;
        rb = GetComponent<Rigidbody>();

        itemRader = transform.GetChild(2);

        //Collider[] colliders = GetComponents<Collider>();
        //itemRader = colliders[1];
    }




    private void Update()
    {
        // 걷기 or 달리기 상태일때 스테미나 회복 및 감소
        if (CurrentMoveMode == MoveMode.Run && currnetStamina > 0 && currentSpeed > 0)
        {
            ConsumeStamina();
        }
        else if (CurrentMoveMode == MoveMode.Walk && isCanRecovery)
        {
            RecoverStamina();
        }
        else
        {
            currentMoveMode = MoveMode.Walk;
        }

        Debug.Log(currnetStamina);
        // 이동 방향 계산
        CalculateMoveDirection();
        // 플레이어가 바라보는 방향
        Vector3 playerForward = transform.forward;
        // 입력 방향의 크기만큼 이동 방향 설정s
        Vector3 moveDirection = playerForward * inputDirection.z + transform.right * inputDirection.x;
        moveDirection.Normalize(); // 이동 방향을 정규화하여 일정한 속도로 이동하도록 함
        // 이동 처리
        //characterController.Move(currentSpeed * Time.deltaTime * moveDirection);
        rb.velocity = moveDirection * currentSpeed;

        Debug.Log(IsGrounded());
    }

    private void FixedUpdate()
    {
        // 아이템을 상호작용하는 함수 호출
        FindItemRay();
        if (!IsGrounded())
        {
            rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
        }
    }


    void EnableStaminaRecovery()
    {
        isCanRecovery = true;
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


            // 레이를 그려줍니다.
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
        }
        else
        {


            // 실패 시에도 레이를 그려줍니다.
            Debug.DrawRay(ray.origin, ray.direction * 5.0f, Color.red);
        }
    }


    /// <summary>
    /// 카메라의 방향을 기준으로 이동 방향을 계산합니다.
    /// </summary>
    /// <returns>이동 방향</returns>
    private void CalculateMoveDirection()
    {
        // 카메라가 바라보는 방향
        Vector3 cameraForward = Camera.main.transform.forward;
        // Y축은 고려하지 않음
        cameraForward.y = 0f;
        // 정규화하여 방향 벡터를 얻음
        cameraForward.Normalize();

        // 플레이어가 바라볼 방향 설정
        transform.forward = cameraForward;
    }

    /// <summary>
    /// 이동 입력 처리용 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMoveInput(Vector2 input, bool isPress)
    {
        inputDirection.x = input.x;     // 입력 방향 저장
        inputDirection.y = 0;
        inputDirection.z = input.y;

        if (isPress)
        {
            MoveSpeedChange(CurrentMoveMode);
        }
        else
        {
            // 입력을 끝낸 상황
            currentSpeed = 0.0f;    // 정지 시키기
        }
    }

    private void OnJumpInput()
    {
        if (IsGrounded())
        {
            Debug.Log("점프");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    bool IsGrounded()
    {
        // 캐릭터의 아래에 레이캐스트를 쏴서 바닥에 닿았는지 확인
        return Physics.Raycast(transform.position + Vector3.down * 0.5f, Vector3.down, 0.7f, layerMask: groundLayer);
    }

    private void OnDrawGizmos()
    {
        // 캐릭터의 아래에 레이캐스트를 쏴서 바닥에 닿았는지 확인
        bool isGrounded = IsGrounded();

        // 기즈모 색상 설정
        Gizmos.color = isGrounded ? Color.green : Color.red;

        // 레이캐스트의 시작점과 끝점을 계산하여 기즈모로 그리기
        Vector3 startPos = transform.position + Vector3.down * 0.5f;
        Vector3 endPos = startPos + Vector3.down * 0.7f;
        Gizmos.DrawLine(startPos, endPos);
    }
    /// <summary>
    /// 이동 모드 변경 입력에 대한 델리게이트로 실행되는 함수
    /// </summary>
    private void OnMoveModeChageInput()
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
    /// 달리는 동안 스테미나 소모하는 함수
    /// </summary>
    void ConsumeStamina()
    {
        // 달리는 동안 스테미나 소모
        currnetStamina -= staminaConsumptionRate * Time.deltaTime;
        if (currnetStamina < 0.1f)
        {
            currnetStamina = 0.0f;
            CurrentMoveMode = MoveMode.Walk;
            isCanRecovery = false;
            Debug.Log("3초간 스테미나를 회복할 수 없습니다.");
            Invoke(nameof(EnableStaminaRecovery), 3.0f);
        }
    }


    /// <summary>
    /// 걷는 동안 스테미나 회복
    /// </summary>
    void RecoverStamina()
    {
        currnetStamina += staminaRecoveryRate * Time.deltaTime;
        if (currnetStamina > stamina)
        {
            currnetStamina = stamina;
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
    private void OnInteractInput(bool isClick)
    {
        if (isClick)
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
                    hit.collider.transform.SetParent(inventory);

                    // 자식으로 만든 객체를 비활성화합니다.
                    hit.collider.gameObject.SetActive(false);
                    Debug.Log("아이템을 획득했습니다!");

                }
                IInteraction interaction = hit.collider.gameObject.GetComponent<IInteraction>();

                // 상호작용이 가능한 물체일때 
                if (interaction != null)
                {
                    // 상호작용 
                    interaction.Interaction(transform.gameObject);
                }
            }
            else
            {
                Debug.Log("레이캐스트 실패!");
            }
        }
        if (!isClick)
        {
            Debug.Log("f키 떨어짐!");
        }
        
    }

    private void OnLClickInput()
    {
    }

    private void OnRClickInput()
    {


        itemRader.gameObject.SetActive(true);
        if (DisableItemRaderAfterDelayCoroutine != null)
        {
            StopCoroutine(DisableItemRaderAfterDelayCoroutine);
        }
        DisableItemRaderAfterDelayCoroutine = StartCoroutine(DisableItemRaderAfterDelay());



    }

    Coroutine DisableItemRaderAfterDelayCoroutine;
    private IEnumerator DisableItemRaderAfterDelay()
    {
        yield return new WaitForSeconds(0.05f); // 변경하고자 하는 시간으로 수정 가능
        itemRader.gameObject.SetActive(false);

    }





    private void OnTriggerEnter(Collider collision)
    {


    }


}
