using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    private List<GameObject> collidedObjects = new List<GameObject>();

    /// <summary>
    /// 팔린 물건의 총 가격을 전달할 델리게이트(플레이어가 구독해야 함)
    /// </summary>
    public Action<float, float> onMoneyEarned;

    void Start()
    {
        // 경로 설정
        //string folderPath = "Assets/KWS/Resources/ItemDB";
        string folderPath = "ItemDB";

        // 해당 폴더 내 모든 스크립터블 오브젝트 찾기
        ItemDB[] scriptableObjects = Resources.LoadAll<ItemDB>(folderPath);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 상대방 오브젝트가 있는지 확인
        if (other.gameObject != null)
        {
            // 충돌한 상대방 오브젝트가 Hardware인지 확인
            if (other.gameObject.CompareTag("Hardware"))
            {
                // 해당 Hardware에 연결된 ItemDB 스크립터블 오브젝트 찾기
                string itemName = other.gameObject.name;
                ItemDB itemDB = Resources.Load<ItemDB>($"ItemDB/{itemName}");

                if (itemDB != null)
                {
                    // ItemDB 정보 출력
                    Debug.Log($"충돌한 {itemDB.itemName} 무게: {itemDB.weight}");
                    Debug.Log($"충돌한 {itemDB.itemName} 가격: {itemDB.price}");

                    // 무게를 누적
                    totalWeight += itemDB.weight;
                    Debug.Log($"누적된 무게: {totalWeight}");

                    // 가격을 누적
                    totalPrice += itemDB.price;
                    Debug.Log($"누적된 가격: {totalPrice}");

                    // 충돌한 오브젝트를 리스트에 추가
                    collidedObjects.Add(other.gameObject);
                }
                else
                {
                    Debug.LogWarning("Hardware 오브젝트에 ItemDB 스크립트가 연결되어 있지 않습니다.");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 해당 Hardware에 연결된 ItemDB 스크립터블 오브젝트 찾기
        string itemName = other.gameObject.name;
        ItemDB itemDB = Resources.Load<ItemDB>($"ItemDB/{itemName}");

        Debug.Log($"{itemDB.itemName} 이 떨어졌습니다.");

        totalWeight -= itemDB.weight;
        Debug.Log($"누적된 무게: {totalWeight}");

        totalPrice -= itemDB.price;
        Debug.Log($"누적된 가격: {totalPrice}");

        // 충돌이 끝난 오브젝트를 리스트에서 제거
        collidedObjects.Remove(other.gameObject);
    }

    /// <summary>
    /// 이 부분은 나중에 플레이어 인풋액션으로 수정해야 함(되는지 확인용)
    /// </summary>
    void Update()
    {
        // F 키를 누르면 충돌한 모든 오브젝트를 판매
        if (Input.GetKeyDown(KeyCode.F))
        {
            // collidedObjects 리스트의 복사본을 만듭니다.
            List<GameObject> collidedObjectsCopy = new List<GameObject>(collidedObjects);

            // 복사본을 이용하여 판매를 수행합니다.
            foreach (var obj in collidedObjectsCopy)
            {
                SellHardware(obj);
            }
        }
    }

    /// <summary>
    /// 폐철물을 판매하는 함수
    /// </summary>
    public void SellHardware(GameObject hardwareObject)
    {
        totalMoney += totalPrice;
        // 판매 처리
        onMoneyEarned?.Invoke(totalPrice, totalMoney); // 총 가격과 금액을 델리게이트로 전달(onMoneyEarned 구독하는 부분 나중에 플레이어가 받게 수정 필요)

        // 판매가 이루어졌으므로 누적된 무게와 가격 초기화
        totalWeight = 0.0f;
        totalPrice = 0.0f;

        hardwareObject.SetActive(false);

        // 판매 후에 리스트에서 해당 오브젝트 제거
        collidedObjects.Remove(hardwareObject);
    }
}
/// 주말에 할 것: 플레이어의 판매 상호작용을 받을 버튼 추가?