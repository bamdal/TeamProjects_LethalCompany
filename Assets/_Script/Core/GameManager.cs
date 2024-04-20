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

        // Store 클래스의 델리게이트를 구독
        store.onMoneyEarned += Money;
    }

    // Store 클래스의 델리게이트가 호출될 때 실행되는 함수
    void Money(float totalPrice, float totalMoney)
    {
        Debug.Log($"GameManager에서 판매 정보를 받았다. 총 가격: {totalPrice}, 누적 금액: {totalMoney}");
    }
}
