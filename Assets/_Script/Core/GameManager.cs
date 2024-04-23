using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player => player;

    ItemDataManager itemDataManager;
    public ItemDataManager ItemData => itemDataManager;

    Store store;
    public Store Store => store;

    Terminal terminal;
    public Terminal Terminal => terminal;

    DungeonGenerator dungeonGenerator;

    /// <summary>
    /// EnemyAI용 배회할 포지션 좌표들
    /// </summary>
    public List<Modul> moduls => dungeonGenerator?.Moduls;

    protected override void OnAddtiveInitialize()
    {
        dungeonGenerator = FindAnyObjectByType<DungeonGenerator>();

    }

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        itemDataManager = GetComponent<ItemDataManager>();
        
        store = FindAnyObjectByType<Store>();        
        if(store != null)
        {
            store.onMoneyEarned += Money;       // Store 클래스의 델리게이트를 구독
        }

        terminal = FindAnyObjectByType<Terminal>();
        if (terminal != null)
        {
            terminal.onFlashLight += UseMoney;
        }
    }

    /// <summary>
    /// Store 클래스의 델리게이트가 호출될 때 실행되는 함수
    /// </summary>
    /// <param name="totalPrice"></param>
    /// <param name="totalMoney"></param>
    void Money(float totalPrice, float totalMoney)
    {
        Debug.Log($"GameManager에서 판매 정보를 받았다. 총 가격: {totalPrice}, 누적 금액: {totalMoney}");
    }

    /// <summary>
    /// 터미널 상점에서 물건을 구매했을 때 실행될 함수
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void UseMoney()
    {
        throw new NotImplementedException();
    }




}
