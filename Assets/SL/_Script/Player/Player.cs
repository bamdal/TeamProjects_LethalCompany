using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.XInput;

public class Player : MonoBehaviour, IBattler, IHealth
{
    /// <summary>
    /// 플레이어 체력
    /// </summary>
    public float maxHp = 100.0f;

    private float currentHp = 0.0f;

    public float Hp
    {
        get => currentHp;
        private set
        {
            if (currentHp != value)
            {
                currentHp = Math.Clamp(value, 0, maxHp);
                onHealthChange?.Invoke(Hp);
            }
        }
    }

    /// <summary>
    /// 플레이어 기력
    /// </summary>
    public float maxStamina = 100.0f;

    /// <summary>
    /// 플레이어 현재 기력
    /// </summary>
    private float currentStamina = 0.0f;

    public float Stamina
    {
        get => currentStamina;
        private set
        {
            if(currentStamina != value)
            {
                currentStamina = Math.Clamp(value, 0, maxStamina);
                onStaminaChange?.Invoke(Stamina);
            }
        }
    }

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

    Transform inventoryTransform;

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

    /// <summary>
    /// 중력 가속도 변수
    /// </summary>
    private float gravityForce = -9.8f;

    // 컴포넌트들
    CharacterController characterController;


    // 입력용 인풋 액션
    PlayerInput input;

    /// <summary>
    /// 카메라를 불러오기 위한 변수
    /// </summary>
    public Camera cam;

    /// <summary>
    /// 현재 중력을 담당할 Y값
    /// </summary>
    float gravityY = 0.0f;
    /// <summary>
    /// 이동방향
    /// </summary>
    Vector3 moveDirection;


    /// <summary>
    /// 중력을 담당하는 방향 백터
    /// </summary>
    Vector3 gravityDir = Vector3.zero;

    public float groundCheckDistance = 0.2f;    // 바닥 체크 거리
    public LayerMask groundLayer;               // 바닥을 나타내는 레이어
    Transform groundCheckPosition;              // 바닥 체크할 포지션
    Transform itemRader;
    Inventory inventory;

    Transform currentItem = null;

    public Transform CurrentItem
    {
        get => currentItem;
        set
        {
            if(currentItem != value)
            {
                currentItem = value;
            }
        }
    }
    int currentItemIndex = 0;
    public int CurrentItemIndex
    {
        get => currentItemIndex;
        set
        {
            if(currentItemIndex != value)
            {
                currentItemIndex = value;
            }
        }
    }

    public Action<float> onHealthChange;
    public Action<float> onStaminaChange;


    InventoryUI invenUI;
    private void Awake()
    {
        input = GetComponent<PlayerInput>();

        input.onMove += OnMoveInput;
        input.onMoveModeChange += OnMoveModeChageInput;
        input.onInteract += OnInteractInput;
        input.onJump += OnJumpInput;
        input.onLClick += OnLClickInput;
        input.onRClick += OnRClickInput;
        input.onScroll += OnScrollWheel;
        input.onItemDrop += OnItemDrop;
        input.onInTerminal += OnInTerminal;
        input.onOutTerminal += OnOutTerminal;
        cam = FindAnyObjectByType<Camera>();
        inventoryTransform = transform.Find("Inventory");
//        inventory = inventoryTransform.GetComponent<Inventory>();
        inventory = FindAnyObjectByType<Inventory>();
        Stamina = maxStamina;
        Hp = maxHp;
        characterController = GetComponent<CharacterController>();
        itemRader = transform.GetChild(2);
        groundCheckPosition = transform.GetChild(4);
        gravityY = -1f;
        invenUI = FindAnyObjectByType<InventoryUI>();
    }

    private void Start()
    {
        CurrentItem = inventory.InvenSlots[0];
    }


    private void Update()
    {
        // 걷기 or 달리기 상태일때 스테미나 회복 및 감소
        if (CurrentMoveMode == MoveMode.Run && Stamina > 0 && currentSpeed > 0)
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
        // 이동 방향 계산
        CalculateMoveDirection();
        // 플레이어가 바라보는 방향
        Vector3 playerForward = transform.forward;
        // 입력 방향의 크기만큼 이동 방향 설정
        Vector3 moveDirection = playerForward * inputDirection.z + transform.right * inputDirection.x;
        moveDirection.Normalize();          // 이동 방향을 정규화하여 일정한 속도로 이동하도록 함
        gravityDir.y = gravityY;            // 중력을 담당하는 방향백터에 y값에 중력을 넣음
        ApplyGravity();                     // 공중일때 중력 적용하는 함수
        // 이동 처리
        characterController.Move(currentSpeed * Time.deltaTime * moveDirection);
        // 중력 처리
        characterController.Move(1f * Time.deltaTime * gravityDir);
        //Debug.Log(IsGrounded());
    }

    private void FixedUpdate()
    {
        // 아이템을 상호작용하는 함수 호출
        FindItemRay();
        inventoryTransform.forward = ItemRotation();
    }

    /// <summary>
    /// 스테미나 회복이 가능함을 담당하는 변수를 바꾸는 함수
    /// </summary>
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

    Vector3 ItemRotation()
    {
        return (Camera.main.transform.forward).normalized;
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

    /// <summary>
    /// 점프 입력에 대한 델리게이트로 실행되는 함수
    /// </summary>
    private void OnJumpInput()
    {
        if (IsGrounded())
        {
            Debug.Log("점프");
            gravityY = 5f;
        }
    }

    /// <summary>
    /// 현재 지금 땅 위인지 확인하는 함수
    /// </summary>
    /// <returns></returns>
    bool IsGrounded()
    {
        // 캐릭터의 아래에 레이캐스트를 쏴서 바닥에 닿았는지 확인
        return Physics.Raycast(groundCheckPosition.position, Vector3.down, groundCheckDistance, layerMask: groundLayer);
    }

    /// <summary>
    /// 공중일때 중력 적용하는 함수
    /// </summary>
    void ApplyGravity()
    {
        if (!IsGrounded())
        {
            gravityY += gravityForce * Time.deltaTime;
        }
    }


    /*    private void OnDrawGizmos()
        {
            // 캐릭터의 아래에 레이캐스트를 쏴서 바닥에 닿았는지 확인
            bool isGrounded = IsGrounded();

            // 기즈모 색상 설정
            Gizmos.color = isGrounded ? Color.green : Color.red;

            // 레이캐스트의 시작점과 끝점을 계산하여 기즈모로 그리기
            Vector3 startPos = groundCheckPosition.position;
            Vector3 endPos = startPos + Vector3.down * 0.2f;
            Gizmos.DrawLine(startPos, endPos);
        }*/



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
        Stamina -= staminaConsumptionRate * Time.deltaTime;
        if (Stamina < 0.1f)
        {
            Stamina = 0.0f;
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
        Stamina += staminaRecoveryRate * Time.deltaTime;
        if (Stamina > maxStamina)
        {
            Stamina = maxStamina;
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

                    for (int i = 0; i < inventory.InvenSlots.Length; i++)
                    {
                        if (inventory.InvenSlots[i].childCount == 0)
                        {
                            // 아이템을 인벤토리 슬롯에 넣습니다.
                            Transform itemTransform = hit.collider.transform;
                            itemTransform.SetParent(inventory.InvenSlots[i]);
                            itemTransform.localPosition = new Vector3(0, -0.5f, 1.5f); // 포지션을 (0, 0, 0)으로 설정합니다.
                            
                            Collider itemCollider = hit.collider.GetComponent<Collider>();
                            if (itemCollider != null)
                                itemCollider.enabled = false;

                            Rigidbody itemRigidbody = hit.collider.GetComponent<Rigidbody>();
                            if (itemRigidbody != null)
                                itemRigidbody.isKinematic = true;
                            itemTransform.rotation = new Quaternion(0, 0, 0, 0);
                            hit.collider.gameObject.SetActive(false);
                            if (inventory.InvenSlots[CurrentItemIndex] != null && inventory.InvenSlots[CurrentItemIndex].childCount > 0)
                            {
                                inventory.InvenSlots[CurrentItemIndex].GetChild(0).gameObject.SetActive(true);
                            }


                            Debug.Log("아이템을 획득했습니다!");
                            break;
                        }

                    }
                    for (int j = 0; j < 4; j++)
                    {
                        if (inventory.InvenSlots[j].childCount > 0)
                        {
                            Transform currentItem = inventory.InvenSlots[j].GetChild(0);
                            if (currentItem != null)
                            {
                                Debug.Log(currentItem);
                                IItemDataBase itemData = currentItem.GetComponent<IItemDataBase>();
                                if (itemData != null)
                                {
                                    Debug.Log(itemData);
                                    inventory.ItemDBs[j] = itemData.GetItemDB();
                                    invenUI.ItemImages[j].sprite = inventory.ItemDBs[j].itemIcon;
                                    Debug.Log(inventory.ItemDBs[j].itemIcon);
                                }
                                
                            }
                        }
                        else
                        {
                            invenUI.ItemImages[j].sprite = null;
                        }
                    }

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

    /// <summary>
    /// 아이템 버리는 함수
    /// </summary>
    private void OnItemDrop()
    {
        if (inventory.InvenSlots[CurrentItemIndex] != null && inventory.InvenSlots[CurrentItemIndex].childCount > 0)    // 인벤토리 인벤슬롯의 현재 인덱스가 널이 아니고, 인벤토리 인벤슬롯안에 아이템이 있다면
        {
            CurrentItem = inventory.InvenSlots[CurrentItemIndex].GetChild(0);   // 현재 아이템은 인벤슬롯의 안에 있는 아이템이다.
            Collider itemCollider = CurrentItem.GetComponent<Collider>();       // 버릴때 콜라이더와 리지드바디 다시 킴
            if (itemCollider != null)
                itemCollider.enabled = true;
            Rigidbody itemRigidbody = CurrentItem.GetComponent<Rigidbody>();
            if (itemRigidbody != null)
                itemRigidbody.isKinematic = false;
            
            CurrentItem.SetParent(null); // 부모에서 떼어냅니다.
            for (int j = 0; j < 4; j++)
            {
                if (inventory.InvenSlots[j].childCount > 0)             // 인벤토리 인벤슬롯에 아이템이 들어있다면
                {
                    Transform tempItem = inventory.InvenSlots[j].GetChild(0);
                    if (tempItem != null)
                    {
                        Debug.Log(tempItem);
                        IItemDataBase itemData = tempItem.GetComponent<IItemDataBase>();
                        if (itemData != null)
                        {
                            Debug.Log(itemData);
                            inventory.ItemDBs[j] = itemData.GetItemDB();                    // ItemDB에서 데이터를 가져와
                            invenUI.ItemImages[j].sprite = inventory.ItemDBs[j].itemIcon;   // 이미지를 인벤토리창에 띄움
                            Debug.Log(inventory.ItemDBs[j].itemIcon);
                        }

                    }
                    
                }
                else
                {
                    invenUI.ItemImages[j].sprite = null;            // 인벤토리 인벤슬롯에 아이템이 비어있으면 인벤토리 이미지 비움
                }
            }
        }
    }


    /// <summary>
    /// 아이템 레이더를 켜고 끌수있게 오른쪽 마우스 버튼 입력에 대한 델리게이트로 연결되어있는 함수
    /// </summary>
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
    /// <summary>
    /// 아이템 레이더 오브젝트를 아주잠깐 켜기위한 코루틴 함수
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisableItemRaderAfterDelay()
    {
        yield return new WaitForSeconds(0.05f); // 변경하고자 하는 시간으로 수정 가능
        itemRader.gameObject.SetActive(false);

    }



    /// <summary>
    /// 좌클릭에 해당하는 입력에 대해 델리게이트로 실행되는 함수
    /// </summary>
    private void OnLClickInput(bool isPressed)
    {
        
        // 아이템 사용 처리
        if (CurrentItem != null && isPressed)
        {
            Debug.Log("찾았음" + CurrentItem);
            IEquipable equipable = CurrentItem.GetComponent<IEquipable>();
            if (equipable != null)
            {
                equipable.Use();
            }
            else
            {
                Debug.Log("이큅터블 없음!");
            }
        }
    }

    private void OnScrollWheel(Vector2 vector)
    {
        Debug.Log(vector.normalized);
        foreach (Transform obj in inventory.InvenSlots)
        {
            if (obj != null && obj.childCount > 0 && obj.GetChild(0).gameObject.activeSelf)
            {
                obj.GetChild(0).gameObject.SetActive(false);
            }
        }

        if (vector.y > 0)
        {
            CurrentItemIndex = PrevIndex(CurrentItemIndex);
            Debug.Log(CurrentItemIndex + " 감소");
        }
        else if (vector.y < 0)
        {
            CurrentItemIndex = NextIndex(CurrentItemIndex);
            Debug.Log(CurrentItemIndex + " 증가");
        }

        if (inventory.InvenSlots[CurrentItemIndex] != null && inventory.InvenSlots[CurrentItemIndex].childCount > 0)    // 인벤토리 슬롯 현재 인덱스가 널값이 아니고, 인벤토리 슬롯 현재 인덱스 내에 아이템이 들어있다면
        {
            CurrentItem = inventory.InvenSlots[CurrentItemIndex].GetChild(0);                                           // 현재 아이템을 인벤토리 슬롯의 현재 인덱스 안에 있는 아이템으로 저장
            
            if (CurrentItem != null)
            {
                CurrentItem.gameObject.SetActive(true);
            }
        }
        for (int i = 0; i < inventory.InvenSlots.Length; i++)
        {
            invenUI.ItemEdgeImages[i].color = invenUI.edgeRedInvisible;     // 일단 인벤토리 테두리 전부다 투명하게
        }
        invenUI.ItemEdgeImages[CurrentItemIndex].color = invenUI.edgeRed;   // 그후 현재 선택된 인덱스의 테두리를 활성화

    }

    /// <summary>
    /// 휠 사용할때 인덱스 증가시키는 함수
    /// </summary>
    /// <param name="index"> 휠 움직임 전 인덱스</param>
    /// <returns>다음 인덱스 값</returns>
    int NextIndex(int index)
    {
        int result;
        if (index + 1 > inventory.InvenSlots.Length - 1)
        {
            result = 0;
        }
        else
        {
            result = index + 1;
        }
        return result;
    }
    /// <summary>
    /// 휠 사용할 때 인덱스 감소시키는 함수
    /// </summary>
    /// <param name="index">휠 움직임 전 인덱스</param>
    /// <returns>이전 인덱스 값</returns>
    int PrevIndex(int index)
    {
        int result;
        if (index - 1 < 0)
        {
            result = inventory.InvenSlots.Length - 1;
        }
        else
        {
            result = index - 1;
        }
        return result;
    }

    
    private void OnInTerminal()
    {

    }

    private void OnOutTerminal()
    {

    }

    public void OnTestDamage()
    {
        Hp -= 10;
    }
    
}
