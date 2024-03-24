using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Test_Terminal : MonoBehaviour
{
    /// <summary>
    /// SphereCollider를 찾기 위한 변수 sphere
    /// </summary>
    SphereCollider sphere;

    /// <summary>
    /// TextMeshProUGUI를 사용하기 위한 변수 PressE_text
    /// </summary>
    TextMeshProUGUI PressE_text;

    private void Awake()
    {
        // SphereCollider를 찾아서 변수에 할당합니다.
        sphere = GetComponent<SphereCollider>();

        // Canvas의 첫 번째 자식을 가져옵니다.
        Transform canvas = transform.GetChild(0);

        // "Press_E"를 이름으로 가진 자식을 찾습니다.
        PressE_text = canvas.Find("Press_E")?.GetComponent<TextMeshProUGUI>();

        // 만약 "Press_E"를 찾지 못했다면, 경고를 출력합니다.
        if (PressE_text == null)
        {
            Debug.LogWarning("TextMeshProUGUI를 찾을 수 없습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")           // 충돌한 상대 오브젝트의 태그가 Player이면
        {
            Debug.Log($"[Player] 가 범위 안에 들어왔다.");
            PressE_text.gameObject.SetActive(true);            // TextMeshProUGUI를 활성화
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")           // 충돌한 상대 오브젝트의 태그가 Player이면
        {
            Debug.Log($"[Player] 가 범위 밖으로 나갔다.");
            PressE_text.gameObject.SetActive(false);            // TextMeshProUGUI를 비활성화
        }
    }
}

/// 0. 플레이어가 터미널의 일정 범위에 들어오면 "Access terminal : [E]" 가 활성화     // 1
/// 1. 플레이어 인풋액션으로 E키를 받아서 터미널 활성화                               // 2
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
