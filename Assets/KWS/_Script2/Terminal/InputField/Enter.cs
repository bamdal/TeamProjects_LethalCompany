using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Enter : MonoBehaviour
{
    public TMP_InputField inputField;

    /// <summary>
    /// 플레이어 인풋 액션
    /// </summary>
    private PlayerInputActions playerInput;

    /// <summary>
    /// 한글 맨 뒤 글자가 잘려서 나오는 문제 수정용 string
    /// 정확한 입력을 비교할 때 totalText를 사용해서 비교해야 함
    /// </summary>
    public string totalText;

    /// <summary>
    /// totalText을 델리게이트로 터미널에 보내기 위한 델리게이트
    /// </summary>
    public Action<string> TotalText;

    private void Awake()
    {
        playerInput = new PlayerInputActions();
        inputField = GetComponent<TMP_InputField>();
        inputField.onSubmit.AddListener((text) =>
        {
            TotalText?.Invoke(text);
            ClearText();
            inputField.ActivateInputField();        //InputField를 활성화하는 함수
        });
    }

    private void Start()
    {
        // InputField의 OnEndEdit 이벤트에 함수를 연결
        //inputField.onEndEdit.AddListener(EndEdit);
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Player.EnterInteract.performed += EnterClick;
    }

    private void OnDisable()
    {
        playerInput.Player.EnterInteract.performed -= EnterClick;
        playerInput.Disable();
    }

    private void EnterClick(InputAction.CallbackContext context)
    {
        // 메시지를 보내도 포커스 활성화
        //inputField.ActivateInputField();

        // 다른 곳을 클릭 했다가 엔터치면 포커스 활성화
        FocusOn();

        //// inputField가 공백이 아닐때
        //if (inputField.text != "") 
        //{
        //    // EnterInteract 이벤트가 발생했을 때 입력 필드의 텍스트를 가져와서 EndEdit 함수 호출
        //    totalText = inputField.text;        // inputField에서 입력된 마지막 문자를 제외한 문자들을 totaltext에 저장하고

        //    // 입력 필드의 텍스트를 초기화
        //    ClearText();                        // inputField 초기화 => 마지막 문자가 남음
        //    StartCoroutine(LastWordChecdk());   // 코루틴 실행
        //}

        //// 메시지를 보낸 후 비활성화
        ////inputField.Select();

        //FocusOut();
    }

    /// <summary>
    /// 한글 입력시 마지막 문자가 잘리는 문제 해결용 함수
    /// EnterClick 함수에서 inputField에서 입력된 마지막 문자를 제외한 문자들을 totaltext에 저장하고
    /// inputField 초기화 => 마지막 문자가 남음
    /// 코루틴에서 EndEdit 함수를 실행해 마지막 남은 문자를 totalText에 더하고 inputField 초기화
    /// </summary>
    /// <param name="text">마지막 남은 한글 문자</param>
    private void EndEdit(string text)
    {
        //totalText += inputField.text;       // 마지막 문자를 totalText에 더해줌
        //Debug.Log(totalText);               // 디버그로 전체 문자 출력
        //ClearText();                        // 마지막 문자 초기화
        //TotalText?.Invoke(totalText);
    }

    /// <summary>
    /// 한글 입력시 마지막 문자 잘리는 문제 해결용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator LastWordChecdk()
    {
        yield return new WaitForSeconds(0.01f);
        //EndEdit(totalText);
    }

    /// <summary>
    /// 인풋필드의 포커스를 유지하기 위한 함수
    /// </summary>
    public void FocusOn()
    {
        // 메시지를 보내도 포커스 활성화
        inputField.ActivateInputField();
    }

    /// <summary>
    /// 터미널에 접속하기 전에는 인풋필드의 포커스를 내리기 위한 함수
    /// </summary>
    public void FocusOut()
    {
        // 메시지를 보낸 후 비활성화
        //inputField.Select();        // 포커스가 되어있으면 비활성화, 포커스가 안되어 있으면 활성화
        inputField.DeactivateInputField();
    }

    /// <summary>
    /// 문자열을 비우기 위한 함수
    /// </summary>
    public void ClearText()
    {
        inputField.text = string.Empty;
    }
}
