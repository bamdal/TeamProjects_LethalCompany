using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using static UnityEditor.Progress;

public class Store : MonoBehaviour
{
    /// <summary>
    /// Store에 올려진 오브젝트의 무게를 누적할 변수
    /// </summary>
    public float totalWeight = 0.0f;

    /// <summary>
    /// Store에 올려진 오브젝트의 가격을 누적할 변수
    /// </summary>
    public float totalPrice = 0.0f;

    /// <summary>
    /// 여태 판매한 금액을 누적할 변수
    /// </summary>
    public float totalMoney = 0.0f;

    /// <summary>
    /// 충돌한 모든 오브젝트를 추적하기 위한 리스트
    /// </summary>
    public List<GameObject> collidedObjects = new List<GameObject>();        


    /// <summary>
    /// 아이템 데이터 매니저를 참조하기 위한 변수
    /// </summary>
    ItemDataManager itemDataManager;

    /// <summary>
    /// 플레이어 인풋 액션
    /// </summary>
    private PlayerInputActions playerInputActions;

    // 델리게이트들 ---------------------------------------------------------------------------------------------------

    /// <summary>
    /// 팔린 물건의 총 가격을 전달할 델리게이트(게임매니저가 구독해야 함)
    /// </summary>
    public Action<float, float> onMoneyEarned;

    public Action onRequest { get; set; }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        //playerInputActions.Enable();
        //playerInputActions.Player.Interact.performed += OnSellClick;
    }

    private void OnDisable()
    {
        //playerInputActions.Disable();
        //playerInputActions.Player.Interact.performed -= OnSellClick;
    }

    void Start()
    {
        // 게임 매니저 오브젝트 찾기
        GameObject gameManager = GameObject.Find("GameManager");

        if (gameManager != null)
        {
            // 게임 매니저 오브젝트에서 ItemDataManager 컴포넌트 가져오기
            itemDataManager = gameManager.GetComponent<ItemDataManager>();

            if (itemDataManager != null)
            {
                Debug.Log("ItemDataManager 찾음");

                // ItemDataManager에서 itemDataBases 배열 가져오기
                if (itemDataManager.itemDataBases != null)
                {
                    Debug.Log("ItemDB 배열 찾음");

                    /*// ItemDB 배열의 각 요소를 디버그로 출력
                    for (int i = 0; i < itemDataManager.itemDataBases.Length; i++)
                    {
                        Debug.Log($"ItemDB[{i}] 이름: {itemDataManager.itemDataBases[i].itemName}");
                        Debug.Log($"ItemDB[{i}] 무게: {itemDataManager.itemDataBases[i].weight}");
                        Debug.Log($"ItemDB[{i}] 가격: {itemDataManager.itemDataBases[i].price}");
                    }*/
                }
                else
                {
                    Debug.LogError("ItemDB 배열을 찾을 수 없습니다.");
                }
            }
            else
            {
                Debug.LogError("GameManager에 ItemDataManager 컴포넌트가 없습니다.");
            }
        }
        else
        {
            Debug.LogError("GameManager 오브젝트를 찾을 수 없습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null && other.gameObject.CompareTag("Hardware"))
        {
            // 충돌한 오브젝트의 ItemBase 컴포넌트 가져오기
            ItemBase itemBase = other.gameObject.GetComponent<ItemBase>();      // 충돌한 오브젝트는 공통적으로 ItemBase를 상속받고 있음
            if (itemBase != null)
            {
                // 충돌한 오브젝트의 ItemDB 가져오기
                ItemDB itemDB = itemBase.itemDB;

                if (itemDB != null)
                {
                    // 충돌한 오브젝트의 아이템 코드 가져오기
                    ItemCode itemCode = itemDB.itemCode;

                    Debug.Log($"충돌한 {itemDB.itemName} 무게: {itemDB.weight}");
                    Debug.Log($"충돌한 {itemDB.itemName} 가격: {itemDB.price}");

                    totalWeight += itemDB.weight;
                    Debug.Log($"누적된 무게: {totalWeight}");

                    totalPrice += itemDB.price;
                    Debug.Log($"누적된 가격: {totalPrice}");

                    collidedObjects.Add(other.gameObject);
                }
                else
                {
                    Debug.LogWarning("충돌한 오브젝트의 ItemDB를 가져올 수 없습니다.");
                }
            }
            else
            {
                Debug.LogWarning("충돌한 오브젝트에 ItemBase 컴포넌트가 없습니다.");
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        // 충돌한 오브젝트의 ItemBase 컴포넌트 가져오기
        ItemBase itemBase = other.gameObject.GetComponent<ItemBase>();
        if (itemBase != null)
        {
            // 충돌한 오브젝트의 ItemDB 가져오기
            ItemDB itemDB = itemBase.itemDB;

            if (itemDB != null)
            {
                // 충돌한 오브젝트의 아이템 코드 가져오기
                ItemCode itemCode = itemDB.itemCode;

                Debug.Log($"{itemDB.itemName} 이 떨어졌습니다.");

                totalWeight -= itemDB.weight;
                Debug.Log($"누적된 무게: {totalWeight}");

                totalPrice -= itemDB.price;
                Debug.Log($"누적된 가격: {totalPrice}");

                // 충돌이 끝난 오브젝트를 리스트에서 제거
                collidedObjects.Remove(other.gameObject);
            }
            else
            {
                Debug.LogWarning("충돌한 오브젝트의 ItemDB를 가져올 수 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("충돌한 오브젝트에 ItemBase 컴포넌트가 없습니다.");
        }
    }    

    /// <summary>
    /// 폐철물을 판매하는 함수
    /// </summary>
    public void SellHardware(GameObject hardwareObject)
    {
        totalMoney += totalPrice;
        // 판매 처리
        onMoneyEarned?.Invoke(totalPrice, totalMoney); // 총 가격과 금액을 델리게이트로 전달

        // 판매가 이루어졌으므로 누적된 무게와 가격 초기화
        totalWeight = 0.0f;
        totalPrice = 0.0f;

        hardwareObject.SetActive(false);

        // 판매 후에 리스트에서 해당 오브젝트 제거
        collidedObjects.Remove(hardwareObject);

        Debug.Log($"판매된 총 금액: [{totalPrice}]");
        Debug.Log($"판매된 누적 금액: [{totalMoney}]");
    }

    /*private void OnSellClick(InputAction.CallbackContext context)
    {
        //Debug.Log("트리거 범위에 Hardware가 없습니다.");
        if (collidedObjects.Count > 0)
        {
            //Debug.Log("트리거 범위에 Hardware가 있고, F가 활성화 되었습니다. ");
            // collidedObjects 리스트의 복사본을 만들고
            List<GameObject> collidedObjectsCopy = new List<GameObject>(collidedObjects);

            // 복사본을 이용하여 판매를 수행
            foreach (var obj in collidedObjectsCopy)
            {
                SellHardware(obj);
            }
        }
    }*/

    /// <summary>
    /// 상호작용 인터페이스
    /// </summary>
    /// <param name="target"></param>
    public void StoreInteraction()
    {
        //Debug.Log("실행");
        if (collidedObjects.Count > 0)
        {
            //Debug.Log("트리거 범위에 Hardware가 있고, F가 활성화 되었습니다. ");
            // collidedObjects 리스트의 복사본을 만들고
            List<GameObject> collidedObjectsCopy = new List<GameObject>(collidedObjects);

            // 복사본을 이용하여 판매를 수행
            foreach (var obj in collidedObjectsCopy)
            {
                SellHardware(obj);
            }
        }
    }
}
