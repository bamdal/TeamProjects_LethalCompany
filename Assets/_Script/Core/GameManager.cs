using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임상태
/// </summary>
public enum GameState
{
    GameReady,
    GameStart,
    GameOver
}

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;
    public Player Player => player;

    SpaceShip spaceShip;

    public SpaceShip SpaceShip => spaceShip;

    Difficulty difficulty = Difficulty.D;

    public Difficulty Difficulty => difficulty;

    public bool TEST_DIFFICYLTY = false;
    public Difficulty TEST_DIFFICULYY = Difficulty.D;

    /// <summary>
    /// 아이템 데이터 매니저
    /// </summary>
    ItemDataManager itemDataManager;
    public ItemDataManager ItemData => itemDataManager;

    /// <summary>
    /// 택배시스템 매니저
    /// </summary>
    DropBoxManager dropBoxManager;
    
    public DropBoxManager DropBoxManager => dropBoxManager;

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

    float totalMoney = -50;

    public float TotalMoney => totalMoney;

    /// <summary>
    /// 초기자금
    /// </summary>
    float initialFunds = 50.0f;

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
                if (value > 0)
                {
                    totalMoney += value - money;
                }
                money = value;
                onMoneyChange?.Invoke(TotalMoney);       // MoneyCountMonitor에서 사용
                onMoney?.Invoke(money);
            }
        }
    }


    /// <summary>
    /// 게임매니저가 현재 가지고 있는 돈이 변화하면 실행할 델리게이트
    /// </summary>
    public Action<float> onMoneyChange;


    public Action<float> onMoney;


    /// <summary>
    /// 현재 게임상태
    /// </summary>
    GameState gameState = GameState.GameReady;

    /// <summary>
    /// 현재 게임상태 변경시 알리는 프로퍼티
    /// </summary>
    public GameState GameState
    {
        get => gameState;
        set
        {
            if (gameState != value)
            {
                gameState = value;
                switch (gameState) 
                {
                    case GameState.GameReady:
                        Debug.Log("게임레디");
                        onGameReady?.Invoke();
                        break;
                    case GameState.GameStart:
                        Debug.Log("게임스타트");
                        var enumValues = Enum.GetValues(typeof(Difficulty)).Cast<ItemCode>().Where(itemCode => (int)itemCode < 10).ToArray();
                        difficulty = (Difficulty)enumValues.GetValue(UnityEngine.Random.Range(0, enumValues.Length));
                        if (TEST_DIFFICYLTY)
                        {
                            difficulty = TEST_DIFFICULYY;
                        }
                        onBuy?.Invoke();
                        onGameStart?.Invoke();
                        break;
                    case GameState.GameOver:
                        Debug.Log("게임오버");
                        onGameOver?.Invoke();   
                        break;
                }
            }
        }
    }


    // 게임상태 델리게이트
    public Action onGameReady;
    public Action onGameStart;
    public Action onGameOver;

    /// <summary>
    /// 현재 날짜
    /// </summary>
    int dday = 3;

    /// <summary>
    /// 최대 날짜
    /// </summary>
    int maxDay = 3;

    /// <summary>
    /// 현재 날짜를 알리는 프로퍼티
    /// </summary>
    int Dday
    {
        get => dday;
        set
        {
            if(dday != value)
            {


                dday = Mathf.Clamp(value,0,maxDay+1);
                
                onDayChange?.Invoke(dday);

                if (dday == 0)
                {
                    Quest();
                }
            }
        }
    }

    public Action<int> onDayChange;

    /// <summary>
    /// 시작 목표 금액
    /// </summary>
    float startTargetAmountMoney = 256;

    /// <summary>
    /// 목표 금액
    /// </summary>
    float targetAmountMoney;

    /// <summary>
    /// 돈 증감 배율
    /// </summary>
    float MoneyMagnification = 1.2f;


    public float TargetAmountMoney
    {
        get => targetAmountMoney;
        private set
        {
            if(targetAmountMoney != value)
            {
                targetAmountMoney = value;
                onTargetAmountMoneyChange?.Invoke(targetAmountMoney);
            }
        }
    }

    public Action<float> onTargetAmountMoneyChange;

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
            if (items != value)
            {
                items = value;
            }
        }
    }

    /// <summary>
    /// EnemyAI용 배회할 포지션 좌표들
    /// </summary>
    public List<Modul> enemyTargetPositions => dungeonGenerator?.Moduls;

    protected override void OnAddtiveInitialize()
    {
        dungeonGenerator = FindAnyObjectByType<DungeonGenerator>();

    }

    private void Awake()
    {
        items = new Queue<ItemCode>();
    }

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();
        spaceShip = FindAnyObjectByType<SpaceShip>();
        player = FindAnyObjectByType<Player>();
        player.onDie = OnDie;
        Money = initialFunds;
    }

    protected override void OnInitialize()
    {
        itemDataManager = GetComponent<ItemDataManager>();
        dropBoxManager = GetComponent<DropBoxManager>();    
        Player.cam = Camera.main;
        Player.invenUI = FindAnyObjectByType<InventoryUI>();
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        store = FindAnyObjectByType<Store>();
        TargetAmountMoney = startTargetAmountMoney;
        ItemDB itemDB = FindAnyObjectByType<ItemDB>();
        if (store != null)
        {
            store.onMoneyEarned += OnMoneyAdd;       // Store 클래스의 델리게이트 연결
        }

        terminal = FindAnyObjectByType<Terminal>();
        if (terminal != null)
        {
            terminal.onUseMoney += OnUseMoney;    // Terminal 클래스의 델리게이트 연결
        }
    }

    /// <summary>
    /// 딜레이용 부울
    /// </summary>
    bool delay = true;

    /// <summary>
    /// 플레이어 사망시 씬 이동
    /// </summary>
    void OnDie()
    {
        if (delay)
        {
            delay = false;
            if (GameState == GameState.GameStart)
            {

                StartCoroutine(LoadSpaceScene());

            }
            else
            {
                StartCoroutine(GameoverScene());
            }
        }

    }

    IEnumerator GameoverScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(6, LoadSceneMode.Single);
        async.allowSceneActivation = false;
        SpaceShip.SpaceShipDoorClose();
        yield return new WaitForSeconds(3.0f);
        async.allowSceneActivation = true;
        Player.PlayerRefresh();
        SpaceShip.Refresh();
        while (!async.isDone)
        {
            yield return null;
        }
        SpaceShip.transform.position = Vector3.zero;
        SpaceShip.transform.rotation = Quaternion.identity;
        
        Player.ControllerTPPosition(Vector3.zero);
        Player.PlayerRefresh();
        Terminal.IsSpace();

        yield return new WaitForSeconds(10.0f);
        delay = true;
    }

    IEnumerator LoadSpaceScene()
    {

        SpaceShip.SpaceShipDoorClose();
        AsyncOperation async = SceneManager.LoadSceneAsync(5, LoadSceneMode.Single);
        async.allowSceneActivation = false;
        yield return new WaitForSeconds(3.0f);
        async.allowSceneActivation = true;

        while (!async.isDone)
        {
            yield return null;
        }

        SpaceShip.transform.position = Vector3.zero;
        SpaceShip.transform.rotation = Quaternion.identity;
        Player.ControllerTPPosition(Vector3.zero);
        Player.PlayerRefresh();
        yield return new WaitForSeconds(10.0f);
        delay = true;
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
    private void OnUseMoney(float price)
    {
        if (Money > price)
        {
            Money -= price;
        }
        else
        {
            Debug.Log("돈이 부족합니다.");
        }

        /*if (Money >= FlashLightPrice)
        {
            // 돈 차감하고
            Money -= FlashLightPrice;
            ItemsQueue.Enqueue(ItemCode.FlashLight);

            // 팩토리에서 손전등 생성
            //Factory.Instance.GetItem();

            Debug.Log($"{FlashLightPrice}원 이 사용되었다. 현재 남은 돈{Money}원");

            // 아이템을 샀다고 델리게이트로 알림
            onBuy?.Invoke();
        }
        else
        {
            Debug.Log($"돈이 부족합니다. 현재 남은 돈 {Money}원");
        }*/
    }

    /// <summary>
    /// 아이템을 샀다고 알릴 델리게이트
    /// </summary>
    public Action onBuy;

    /// <summary>
    /// 하루 감소 시키는 함수
    /// </summary>
    public void NextDay()
    {
        Dday--;
    }

    /// <summary>
    /// 목표금액 체크
    /// </summary>
    void Quest()
    {
        if (TargetAmountMoney > totalMoney) // 목표금액 도달 실패시 게임오버
        {
            GameState = GameState.GameOver;   
        }
        else
        {   // 퀘스트 달성시 목표금액 증가, 기간 초기화
            Dday = maxDay;
            totalMoney = 0;
            TargetAmountMoney *= MoneyMagnification;
        }
    }

    /// <summary>
    /// 게임 초기화 함수
    /// </summary>
    public void ResetGame()
    {
        Dday = maxDay;
        Money = initialFunds;
        targetAmountMoney = startTargetAmountMoney;
    }


#if UNITY_EDITOR
    /// <summary>
    /// 테스트용
    /// </summary>
    public void OnUse()
    {
        onBuy.Invoke();
    }

#endif
}
