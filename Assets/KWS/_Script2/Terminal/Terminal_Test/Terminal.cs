using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using UnityEngine.SceneManagement;
using UnityEngine.ProBuilder.Shapes;

public class Terminal : MonoBehaviour,IInteraction
{
    /// <summary>
    /// PressF_text 텍스트 활성화 범위를 감지할 콜라이더
    /// </summary>
    SphereCollider sphere;

    /// <summary>
    /// PressF_text
    /// </summary>
    TextMeshProUGUI PressF_text;

    /// <summary>
    /// mainText
    /// </summary>
    TextMeshProUGUI mainText;

    /// <summary>
    /// storeText
    /// </summary>
    TextMeshProUGUI storeText;

    /// <summary>
    /// HelpText
    /// </summary>
    TextMeshProUGUI helpText;

    /// <summary>
    /// 인터페이스를 화면의 중앙에 정렬하기 위한 오프셋
    /// </summary>
    public Vector3 interfaceOffset = new Vector3(0.5f, 0.5f, 0);
    
    public CinemachineVirtualCamera farVcam;        // 멀리 있는 Vcam
    public CinemachineVirtualCamera nearVcam;       // 가까이 있는 Vcam

    /// <summary>
    /// 플레이어 인풋 액션
    /// </summary>
    private PlayerInputActions playerInputActions;

    /// <summary>
    /// 인풋필드에서 엔터가 입력되었을 때 실행될 스크립트
    /// </summary>
    Enter enter;

    /// <summary>
    /// 플레이어 인풋 스크립트
    /// </summary>
    PlayerInput playerInput;

    /// <summary>
    /// 스토어 화면에서 입력된 onFlashLight 아이템을 게임매니저로 알릴 델리게이트
    /// </summary>
    public Action onFlashLight;

    /// <summary>
    /// 터미널의 범위에 들어왔는지 확인하는 변수
    /// </summary>
    //bool TerminalRange = false;

    /// <summary>
    /// 터미널에서 씬을 불러올 때 입력된 행성을 받아올 string
    /// </summary>
    string sceneNameToLoad;

    /// <summary>
    /// 플레이어의 상태를 나타내는 캔버스
    /// </summary>
    public Canvas playerCanvas;

    // 델리게이트들 -----------------------------------------------------------------------------------------------------

    public Action ESC;
    public Action onRequest { get; set; }

    GameManager gameManager;

    AsyncOperation loadOperation;
    private void Awake()
    {
        sphere = GetComponent<SphereCollider>();                             // PressF_text의 감지범위 콜라이더
        
        Transform canvas = transform.GetChild(0);                            // 0번째 자식 canvas
        PressF_text = canvas.GetChild(0).GetComponent<TextMeshProUGUI>();    // canvas의 0번째 자식 Press_E

        mainText = canvas.GetChild(2).GetComponent<TextMeshProUGUI>();       // canvas의 2번째 자식 DefaultText

        storeText = canvas.GetChild(3).GetComponent<TextMeshProUGUI>();      // canvas의 3번째 자식 StoreText

        helpText = canvas.GetChild(4).GetComponent<TextMeshProUGUI>();       // canvas의 4번째 자식 HelpText


        // 게임 시작 시
        mainText.gameObject.SetActive(true);                                // 시작할 때 mainText 활성화
        PressF_text.gameObject.SetActive(false);                            // 시작할 때 PressF_text 비활성화
        storeText.gameObject.SetActive(false);                              // 시작할 때 storeText 비활성화
        helpText.gameObject.SetActive(false);                               // 시작할 때 helpText 비활성화

        playerInputActions = new PlayerInputActions();

        // Enter 스크립트 찾음
        enter = FindAnyObjectByType<Enter>();
    }

    private void Start()
    {
        enter.TotalText += ChangePanel;

        // 게임 시작 시 farVcam에 PlayerVC 자동 할당
        GameObject temp = GameObject.Find("PlayerVC");
        farVcam = temp.GetComponent<CinemachineVirtualCamera>();

        // 게임 시작 시 nearVcam 자동 할당
        Transform near = transform.GetChild(1);
        nearVcam = near.GetComponent<CinemachineVirtualCamera>();

        gameManager = GameManager.Instance;
    }



    private void OnEnable()
    {
        //playerInputActions.Player.terminal.performed += OnEClick;
        //playerInputActions.Player.ESCInteract.performed += OnESCClick;        
    }

    private void OnDisable()
    {
        //playerInputActions.Player.ESCInteract.performed -= OnESCClick;
        //playerInputActions.Player.terminal.performed -= OnEClick;
    }

    /// <summary>
    /// 터미널에 진입하기 위한 함수
    /// </summary>
    /// <param name="context"></param>
    /*private void OnEClick(InputAction.CallbackContext context)
    {
        Debug.Log($"E 키가 눌렸습니다.");
        if (PressF_text.gameObject.activeSelf && context.action.triggered)
        {
            enter.ClearText();
            // 포커스 온
            enter.FocusOn();
            Debug.Log("PressE 활성화 & E 키가 눌렸습니다.");      // E 키가 눌렸을 때 디버그 출력
            PressF_text.gameObject.SetActive(false);
            SwitchCamera();

            // Move 액션 처리 비활성화
            playerInputActions.Player.Move.Disable();
        }

    }*/

    /// <summary>
    /// 터미널에서 빠져나오기 위한 함수
    /// </summary>
    /// <param name="context"></param>
    /*private void OnESCClick(InputAction.CallbackContext context)
    {
        Debug.Log($"ESC 키가 눌렸습니다");
        if (!PressF_text.gameObject.activeSelf && context.action.triggered)
        {
            // 포커스 아웃
            enter.FocusOut();

            Debug.Log($"PressE 비활성화 & ESC 키가 눌렸습니다.");      // ESC 키가 눌렸을 때 디버그 출력
            PressF_text.gameObject.SetActive(true);
            SwitchCamera();

            // Move 액션 처리 활성화
            playerInputActions.Player.Move.Enable();
        }

    }*/

    

    // 터미널 진입 관련 -----------------------------------------------------------------------------------------------------
/*
    /// <summary>
    /// 터미널에 진입하기 위한 함수
    /// </summary>
    private void OnEClick()
    {
        Debug.Log($"E 키가 눌렸습니다.");
        if (PressF_text.gameObject.activeSelf && TerminalRange == true)
        {

        }
    }*/



    /// <summary>
    /// 플레이어가 터미널의 범위 안에 들어왔는지 확인하는 함수
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")                   // 충돌한 상대 오브젝트의 태그가 Player이면
        {
            //TerminalRange = true;
            Debug.Log($"[Player] 가 범위 안에 들어왔다.");
            PressF_text.gameObject.SetActive(true);             // TextMeshProUGUI를 활성화
        }
    }

    /// <summary>
    /// 플레이어가 터미널의 범위 밖으로 나갔는지 확인하는 함수
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")                   // 충돌한 상대 오브젝트의 태그가 Player이면
        {
            //TerminalRange = false;
            Debug.Log($"[Player] 가 범위 밖으로 나갔다.");
            PressF_text.gameObject.SetActive(false);            // TextMeshProUGUI를 비활성화
        }
    }

    /// <summary>
    /// nearVcam과 farVcam의 Priority를 서로 바꾸는 함수
    /// </summary>
    void SwitchCamera()
    {
        if (farVcam == null)    // 씬이동시 놓친 카메라 다시 맞춰주기
        {
            GameObject temp = GameObject.Find("PlayerVC");
            farVcam = temp.GetComponent<CinemachineVirtualCamera>();
        }
        if (nearVcam != null && farVcam != null)
        {
            // 초기에 nearVcam.Priority = 11, farVcam.Priority = 10
            int nearPriority = nearVcam.Priority;
            nearVcam.Priority = farVcam.Priority;
            farVcam.Priority = nearPriority;
                        
        }
        else
        {
            Debug.LogError("Priority를 바꿀 수 없다.");
        }
    }

    // 터미널의 InputField 관련 ------------------------------------------------------------------------------------------------

    /// <summary>
    /// inputField에서 문자가 입력되었을 때 지정한 문자인지 확인하고 처리하는 함수
    /// </summary>
    /// <param name="obj">inputField에서 입력된 문자</param>
    void ChangePanel(string obj)
    {
        switch (obj.ToLower())
        {
            case "store":
            case "스토어":
                mainText.gameObject.SetActive(false);           // mainText 비활성화
                storeText.gameObject.SetActive(true);           // storeText 활성화
                helpText.gameObject.SetActive(false);           // helpText 비활성화
                break;
            case "main":
            case "메인":
                storeText.gameObject.SetActive(false);          // storeText 비활성화
                mainText.gameObject.SetActive(true);            // mainText 활성화
                helpText.gameObject.SetActive(false);           // helpText 비활성화
                break;
            case "help":
            case "도움":
                if(mainText.gameObject.activeSelf || storeText.gameObject.activeSelf)   // mainText 또는 storeText 가 활성화 상태이면
                {
                    mainText.gameObject.SetActive(false);           // mainText 비활성화
                    storeText.gameObject.SetActive(false);          // storeText 비활성화
                    helpText.gameObject.SetActive(true);            // helpText 활성화
                }
                break;

            // 물건 구매하는 부분 -----------------------------------------------------------------------------------------
            case "flashlight":
            case "손전등":
                if (!mainText.gameObject.activeSelf && storeText.gameObject.activeSelf) // mainText 비활성화 storeText 활성화 상태이면
                {
                    Debug.Log("스토어 입력 중 손전등 입력 확인");
                    gameManager.ItemsQueue.Enqueue(ItemCode.FlashLight);
                }
                break;
            case "proflashlight":
            case "프로손전등":
                if (!mainText.gameObject.activeSelf && storeText.gameObject.activeSelf) // mainText 비활성화 storeText 활성화 상태이면
                {
                    Debug.Log("스토어 입력 중 프로손전등 입력 확인");
                    gameManager.ItemsQueue.Enqueue(ItemCode.FlashLightUp);
                }
                break;
            case "shovel":
            case "삽":
                if (!mainText.gameObject.activeSelf && storeText.gameObject.activeSelf) // mainText 비활성화 storeText 활성화 상태이면
                {
                    Debug.Log("스토어 입력 중 삽 입력 확인");
                    gameManager.ItemsQueue.Enqueue(ItemCode.Shovel);
                }
                break;
            case "zapGun":
            case "공기권총":
                if (!mainText.gameObject.activeSelf && storeText.gameObject.activeSelf) // mainText 비활성화 storeText 활성화 상태이면
                {
                    Debug.Log("스토어 입력 중 ZapGun 입력 확인");
                    gameManager.ItemsQueue.Enqueue(ItemCode.ZapGun);
                }
                break;
            case "grenade":
            case "섬광수류탄":
                if (!mainText.gameObject.activeSelf && storeText.gameObject.activeSelf) // mainText 비활성화 storeText 활성화 상태이면
                {
                    Debug.Log("스토어 입력 중 섬광수류탄 입력 확인");
                    gameManager.ItemsQueue.Enqueue(ItemCode.Grenade);
                }
                break;
            case "labber":
            case "사다리":
                if (!mainText.gameObject.activeSelf && storeText.gameObject.activeSelf) // mainText 비활성화 storeText 활성화 상태이면
                {
                    Debug.Log("스토어 입력 중 사다리 입력 확인");
                    gameManager.ItemsQueue.Enqueue(ItemCode.Labber);
                }
                break;

            // 행성 이동하는 부분 -----------------------------------------------------------------------------------------
            case "타이탄":
            case "titan":
                if (mainText.gameObject.activeSelf)     // mainText가 활성화 된 상태에서
                {
                    Debug.Log("mainText가 활성화된 상태에서 행성의 입력을 확인");
                    sceneNameToLoad = "IntegrationScenes";              // 씬의 이름이 IntegrationScenes 것 불러옴
                    ChangeSceen();
                }
                break; 

            case "원래행성":
                if (mainText.gameObject.activeSelf)     // mainText가 활성화 된 상태에서
                {
                    Debug.Log("mainText가 활성화된 상태에서 원래 행성의 입력을 확인");
                    sceneNameToLoad = "10_Test_Money";              // 씬의 이름이 10_Test_Money 것 불러옴
                    ChangeSceen();
                }
                break;
            case "회사":
            case "company":
                if (mainText.gameObject.activeSelf)     // mainText가 활성화 된 상태에서
                {
                    Debug.Log("mainText가 활성화된 상태에서 원래 행성의 입력을 확인");
                    sceneNameToLoad = "Company";              // 씬의 이름이 10_Test_Money 것 불러옴
                    ChangeSceen();
                }
                break;
            default:
                Debug.Log("정확히 입력해주세요.");
                break;
        }

        gameManager.onBuy?.Invoke();
    }

    /// <summary>
    /// 씬을 변경하기 위한 함수
    /// </summary>
    void ChangeSceen()
    {
        pressESC();
        StartCoroutine(LoadSceneAsync());
        enter.FocusOut();
    }


    /// <summary>
    /// 씬을 비동기로 로드하기 위한 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadSceneAsync()
    {
        // 다음 씬 비동기 로드 시작
        //SceneManager.LoadScene("AsyncLoadScene", LoadSceneMode.Additive);

        loadOperation = SceneManager.LoadSceneAsync(sceneNameToLoad, LoadSceneMode.Single);
        loadOperation.allowSceneActivation = false;
        nearVcam.Priority = 9;
        // 씬 로드 완료를 기다림
        while (!loadOperation.isDone)
        {
            Debug.Log("delay");
            yield return null;
        }
        int count = GameManager.Instance.SpaceShip.ItemBox.childCount;
        for (int i = 0; i < count; i++)
        {
            GameManager.Instance.SpaceShip.ItemBox.GetChild(i).gameObject.SetActive(true);
        }

/*        // 현재 씬의 비동기 로드 시작
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        // 씬 언로드 완료를 기다림
        while (!unloadOperation.isDone)
        {
            yield return null;
        }*/

 

    }

    public void GoNextScene()
    {
        if (loadOperation != null)
        {

        loadOperation.allowSceneActivation = true;
        }
    }

    /// <summary>
    /// 상호작용 인터페이스
    /// </summary>
    /// <param name="target"></param>
    public void Interaction(GameObject target)
    {
        if (PressF_text.gameObject.activeSelf)
        {
            PressF_text.gameObject.SetActive(false);        // PressF_text 비활성화

            // OnESCClick 연결
            playerInputActions.Enable();
            playerInputActions.Option.ESC.performed += OnESCClick;

            // 텍스트 지우기
            enter.ClearText();

            // 포커스 온
            enter.FocusOn();

            // 카메라 스위칭
            SwitchCamera();

            // 여기다 플레이어의 canvas 끄는 것 추가
            playerCanvas.gameObject.SetActive(false);

        }
    }

    private void OnESCClick(InputAction.CallbackContext context)
    {
        Debug.Log($"ESC 키가 눌렸습니다");
        if (!PressF_text.gameObject.activeSelf)
        {
            pressESC();

            // 여기다 플레이어의 canvas 켜는 것 추가
            playerCanvas.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// ESC가 눌려지면 터미널에서 빠져나오는 함수
    /// </summary>
    private void pressESC()
    {
        PressF_text.gameObject.SetActive(true);        // PressF_text 활성화

        enter.ClearText();        // 혹시 텍스트 남아있으면 비우기

        // 포커스 아웃
        enter.FocusOut();

        // 터미널 화면 원래대로 돌리기
        storeText.gameObject.SetActive(false);          // storeText 비활성화
        mainText.gameObject.SetActive(true);            // mainText 활성화
        helpText.gameObject.SetActive(false);           // helpText 비활성화

        // 카메라 스위칭
        SwitchCamera();

        // OnESCClick 연결해제
        playerInputActions.Option.ESC.performed -= OnESCClick;
        playerInputActions.Disable();

        // IInteraction 인터페이스에 알림
        onRequest?.Invoke();
    }
}

/// 0. 플레이어가 터미널의 일정 범위에 들어오면 "Access terminal : [E]" 가 활성화     // 1       v
/// 1. 플레이어 인풋액션으로 E키를 받아서 터미널 활성화                               // 2       v
/// 1-1. 터미널이 활성화되면 터미널의 모니터로 VCam 위치 조정                         // 3       v
/// 2. 터미널의 처음 화면 텍스트 출력                                                 // 1번-UI  v

/*
위성 카탈로그에 오신 것을 환영합니다.
자동 조종 장치의 경로를 지정하려면 ROUTE를 입력하세요.
달에 대해 알아보려면 INFO를 입력하세요.
----------------------------		// (28개)

* 회사 건물	//	30%에 매매 중.

* 익스페러멘테이션 (날씨)	Experimentation
* 어슈어런스 (날씨)		    Assurance
* 보우 (날씨)		        Vow

* 오펜스 (날씨)		        Offense
* 머치 (날씨)		        March

* 렌드 (날씨)		        Rend
* 다인 (날씨)		        Dine
* 타이탄 (날씨)		        Titan

(입력하는 곳)    여기에 커서가 깜빡여야 함?
*/

/// 3. 터미널에서 Help(대소문자X) 를 입력하고 엔터를 누르면 사용가능한 모든 명령어가 터미널에 출력   // 2번-UI
/// 3-1 메인에서 할 수 있는 명령어
///     - Store
///     - 갈수 있는 행성들
///     - Help
/// 3-2 스토어에서 할 수 있는 명령어
///     - Main
///     - 살 수 있는 아이템들
///     - 현재 있는 총 금액?(Money)
/// 4. Store 목록
/*
Store를 입력하고 엔터를 누르면 상점이 열림
아아템 구매도 터미널에서 함 / 구매 가능한 아이템 목록(치고 엔터 눌러야 함)
/ 무전기				        12원
/ 손전등				        15원
/ 삽				            30원
/ 마스터 키			            20원
/ 프로 손전등(전문가용 손전등)	25원
/ 섬광 수류탄			        36원
/ 붐박스				        60원
/ Tzp 흡입기			        72원
/ 공기 권총(Zap Gun)		    400원
/ 제트팩				        700원
/ 확장 사다리(개폐식 사다리)	60원
*/
