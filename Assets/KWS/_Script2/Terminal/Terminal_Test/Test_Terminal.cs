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

public class Test_Terminal : MonoBehaviour
{
    /// <summary>
    /// SphereCollider를 찾기 위한 변수 sphere
    /// </summary>
    SphereCollider sphere;

    /// <summary>
    /// PressE_text
    /// </summary>
    TextMeshProUGUI PressE_text;

    /// <summary>
    /// mainText
    /// </summary>
    TextMeshProUGUI mainText;

    TextMeshProUGUI storeText;

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

    private void Awake()
    {
        // SphereCollider를 찾아서 변수에 할당합니다.
        sphere = GetComponent<SphereCollider>();

        // 0번째 자식 canvas
        Transform canvas = transform.GetChild(0);

        // "Press_E"를 이름으로 가진 자식
        /*PressE_text = canvas.Find("Press_E")?.GetComponent<TextMeshProUGUI>();

        // 만약 "Press_E"를 찾지 못했다면, 경고를 출력합니다.
        if (PressE_text == null)
        {
            Debug.LogWarning("TextMeshProUGUI를 찾을 수 없습니다.");
        }*/

        Transform child = transform.GetChild(0);                            // 0번째 자식 canvas
        PressE_text = canvas.GetChild(0).GetComponent<TextMeshProUGUI>();    // canvas의 0번째 자식 Press_E

        mainText = canvas.GetChild(2).GetComponent<TextMeshProUGUI>();    // canvas의 2번째 자식 DefaultText

        storeText = canvas.GetChild(3).GetComponent<TextMeshProUGUI>();      // canvas의 3번째 자식 StoreText


        // 게임 시작 시
        PressE_text.gameObject.SetActive(false);                            // 시작할 때 PressE_text 비활성화
        mainText.gameObject.SetActive(true);                             // 시작할 때 mainText 활성화
        storeText.gameObject.SetActive(false);                              // 시작할 때 storeText 비활성화

        playerInputActions = new PlayerInputActions();

        // Enter 스크립트 찾음
        enter = FindAnyObjectByType<Enter>();

        playerInput = FindAnyObjectByType<PlayerInput>();

    }

    private void Start()
    {
        enter.TotalText += ChangePanel;
        playerInput.onTerminal += OnEClick;


    }

    private void OnEClick()
    {
        Debug.Log($"E 키가 눌렸습니다.");
        if (PressE_text.gameObject.activeSelf)
        {
            enter.ClearText();
            // 포커스 온
            enter.FocusOn();
            Debug.Log("PressE 활성화 & E 키가 눌렸습니다.");      // E 키가 눌렸을 때 디버그 출력
            PressE_text.gameObject.SetActive(false);
            SwitchCamera();

            // Move 액션 처리 비활성화
            playerInputActions.Player.Move.Disable();
        }
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
        //playerInputActions.Player.terminal.performed += OnEClick;
        playerInputActions.Player.ESCInteract.performed += OnESCClick;        
    }

    private void OnDisable()
    {
        playerInputActions.Player.ESCInteract.performed -= OnESCClick;
        //playerInputActions.Player.terminal.performed -= OnEClick;
        playerInputActions.Disable();
    }

    /// <summary>
    /// 터미널에 진입하기 위한 함수
    /// </summary>
    /// <param name="context"></param>
    /*private void OnEClick(InputAction.CallbackContext context)
    {
        Debug.Log($"E 키가 눌렸습니다.");
        if (PressE_text.gameObject.activeSelf && context.action.triggered)
        {
            enter.ClearText();
            // 포커스 온
            enter.FocusOn();
            Debug.Log("PressE 활성화 & E 키가 눌렸습니다.");      // E 키가 눌렸을 때 디버그 출력
            PressE_text.gameObject.SetActive(false);
            SwitchCamera();

            // Move 액션 처리 비활성화
            playerInputActions.Player.Move.Disable();
        }

    }*/

    /// <summary>
    /// 터미널에서 빠져나오기 위한 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnESCClick(InputAction.CallbackContext context)
    {
        Debug.Log($"ESC 키가 눌렸습니다");
        if (!PressE_text.gameObject.activeSelf && context.action.triggered)
        {
            // 포커스 아웃
            enter.FocusOut();

            Debug.Log($"PressE 비활성화 & ESC 키가 눌렸습니다.");      // ESC 키가 눌렸을 때 디버그 출력
            PressE_text.gameObject.SetActive(true);
            SwitchCamera();

            // Move 액션 처리 활성화
            playerInputActions.Player.Move.Enable();
        }

    }

    /// <summary>
    /// 플레이어가 터미널의 범위 안에 들어왔는지 확인하는 함수
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")                   // 충돌한 상대 오브젝트의 태그가 Player이면
        {
            Debug.Log($"[Player] 가 범위 안에 들어왔다.");
            PressE_text.gameObject.SetActive(true);             // TextMeshProUGUI를 활성화
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
            Debug.Log($"[Player] 가 범위 밖으로 나갔다.");
            PressE_text.gameObject.SetActive(false);            // TextMeshProUGUI를 비활성화
        }
    }

    /// <summary>
    /// nearVcam과 farVcam의 Priority를 서로 바꾸는 함수
    /// </summary>
    void SwitchCamera()
    {
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

    /// <summary>
    /// inputField에서 문자가 입력되었을 때 지정한 문자인지 확인하고 처리하는 함수
    /// </summary>
    /// <param name="obj">inputField에서 입력된 문자</param>
    void ChangePanel(string obj)
    {
        /*//Debug.Log($"ChangePanel이 {obj}가 입력된 것을 확인했다.");
        if (obj == "Store" || obj == "store" || obj == "스토어")               // 입력된 문자열이 store 라면
        {
            mainText.gameObject.SetActive(false);        // mainText 비활성화
            storeText.gameObject.SetActive(true);           // storeText 활성화
        }
        if (obj == "Main" || obj == "main" || obj == "메인")           // 입력된 문자열이 default 라면
        {
            storeText.gameObject.SetActive(false);          // storeText 비활성화
            mainText.gameObject.SetActive(true);         // mainText 활성화
        }*/

        switch (obj.ToLower())
        {
            case "store":
            case "스토어":
                mainText.gameObject.SetActive(false);           // mainText 비활성화
                storeText.gameObject.SetActive(true);           // storeText 활성화
                break;
            case "main":
            case "메인":
                storeText.gameObject.SetActive(false);          // storeText 비활성화
                mainText.gameObject.SetActive(true);            // mainText 활성화
                break;




            case "flashlight":
            case "손전등":
                if (!mainText.gameObject.activeSelf && storeText.gameObject.activeSelf)
                {
                    Debug.Log("스토어 입력 중 손전등 입력 확인");
                    onFlashLight?.Invoke();
                }
                break;
            default:
                Debug.Log("정확히 입력해주세요.");
                break;
        }
    }


}

/// 0. 플레이어가 터미널의 일정 범위에 들어오면 "Access terminal : [E]" 가 활성화     // 1
/// 1. 플레이어 인풋액션으로 E키를 받아서 터미널 활성화                               // 2       받으면 디버그 출력으로 테스트하기
/// 1-1. 터미널이 활성화되면 터미널의 모니터로 VCam 위치 조정                         // 3
/// 2. 터미널의 처음 화면 텍스트 출력                                                 // 1번-UI

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
/// 3-1 확인(Confirm), 취소(Deny), 상점(Store)...                                                    // 2번-UI
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
