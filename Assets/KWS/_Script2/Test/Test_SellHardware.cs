using UnityEngine;
using UnityEngine.InputSystem;

// 이 코드의 판매하고 금액 산정하는 부분은 나중에 플레이어한테 이식하기
public class Test_SellHardware : TestBase
{
    private Store store;

    private void Start()
    {
        // Store 스크립트의 인스턴스를 찾거나 연결합니다.
        store = FindObjectOfType<Store>();

        // onMoneyEarned 델리게이트에 OnMoneyEarned 메서드를 구독합니다.
        store.onMoneyEarned += OnMoneyEarned;
    }

    private void OnDestroy()
    {
        // 스크립트가 소멸될 때 이벤트 구독을 해제합니다.
        if (store != null)
        {
            store.onMoneyEarned -= OnMoneyEarned;
        }
    }

    /*private void OnEnable()
    {
        //여기 앞에 뭐 쓰는거 있었는데 뭐였지...

        store = FindObjectOfType<Store>(); // Store 클래스의 인스턴스를 찾음
        if (store != null)
        {
            // Store 클래스의 onMoneyEarned 델리게이트에 이벤트를 추가하여 판매된 금액을 출력
            store.onMoneyEarned += OnMoneyEarned;
        }
        else
        {
            Debug.LogError("Store 클래스를 찾을 수 없습니다.");
        }
    }

    private void OnDisable()
    {
        store.onMoneyEarned -= OnMoneyEarned;
        //얘도 삭제할 때 뭐 있었는데...
    }*/

    // Store 스크립트의 onMoneyEarned 이벤트에서 호출되는 메서드입니다.
    private void OnMoneyEarned(float totalPrice, float totalMoney)
    {
        // 받은 돈을 디버그 로그로 출력합니다.
        //Debug.Log("Earned Money: " + tatalMoney);
        Debug.Log($"판매된 총 금액: [{totalPrice}]");
        Debug.Log($"판매된 누적 금액: [{totalMoney}]");
    }
}
