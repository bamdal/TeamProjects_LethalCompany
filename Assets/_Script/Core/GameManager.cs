using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;
    public Player Player => player;

    /// <summary>
    /// 아이템 데이터 매니저
    /// </summary>
    ItemDataManager itemDataManager;
    public ItemDataManager ItemData => itemDataManager;

    /// <summary>
    /// 스토어
    /// </summary>
    Store store;
    public Store Store => store;

    /// <summary>
    /// 터미널
    /// </summary>
    Terminal terminal;
    public Terminal Terminal => terminal;
    
    /// <summary>
    /// 게임매니저가 현재 가지고 있는 돈
    /// </summary>
    float money;
    public float Money
    {
        get => money;
        set
        {
            if (money != value)
            {
                money = value;
                OnMoneyChange?.Invoke(money);       // 사용하는 곳 일단 없음
            }
        }
    }

    /// <summary>
    /// 게임매니저가 현재 가지고 있는 돈이 변화하면 실행할 델리게이트
    /// </summary>
    public Action<float> OnMoneyChange;


    DungeonGenerator dungeonGenerator;

    /// <summary>
    /// 터미널 상점에서 산 아이템을 가질 리스트
    /// </summary>
    Queue<ItemCode> items;
    
    public Queue<ItemCode> ItemsQueue
    {
        get => items;
        private set
        {
            if(items != value)
            {
                items = value;
            }
        }
    }
    /// <summary>
    /// 손전등 가격
    /// </summary>
    const float FlashLightPrice = 10.0f;

    /// <summary>
    /// EnemyAI용 배회할 포지션 좌표들
    /// </summary>
    public List<Modul> moduls => dungeonGenerator?.Moduls;

    protected override void OnAddtiveInitialize()
    {
        dungeonGenerator = FindAnyObjectByType<DungeonGenerator>();

    }

    private void Awake()
    {
        items = new Queue<ItemCode>();
    }

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        itemDataManager = GetComponent<ItemDataManager>();
        
        store = FindAnyObjectByType<Store>();        
        if(store != null)
        {
            store.onMoneyEarned += OnMoneyAdd;       // Store 클래스의 델리게이트 연결
        }

        terminal = FindAnyObjectByType<Terminal>();
        if (terminal != null)
        {
            terminal.onFlashLight += OnUseMoney;    // Terminal 클래스의 델리게이트 연결
        }
    }

    /// <summary>
    /// Store 클래스의 델리게이트가 호출될 때 실행되는 함수
    /// </summary>
    /// <param name="totalPrice"></param>
    /// <param name="totalMoney"></param>
    void OnMoneyAdd(float totalPrice, float totalMoney)
    {
        // 게임 매니저에서 관리하는 돈에 store에서 판매된 금액을 더함
        Money += totalPrice;
        Debug.Log($"GameManager에서 판매 정보를 받았다. 총 가격: {totalPrice}, 누적 금액: {totalMoney}");
    }

    /// <summary>
    /// 터미널 상점에서 물건을 구매했을 때 실행될 함수
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void OnUseMoney()
    {
        if(Money >= FlashLightPrice)
        {
            // 돈 차감하고
            Money -= FlashLightPrice;
            ItemsQueue.Enqueue(ItemCode.FlashLight);

            // 팩토리에서 손전등 생성
            //Factory.Instance.GetItem();

            Debug.Log($"{FlashLightPrice}원 이 사용되었다. 현재 남은 돈{Money}원");
        }
        else
        {
            Debug.Log($"돈이 부족합니다. 현재 남은 돈 {Money}원");
        }
    }




}
