using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Enter : MonoBehaviour
{
    public TMP_InputField inputField;

    /// <summary>
    /// 플레이어 인풋 액션
    /// </summary>
    private PlayerInputActions playerInput;

    private void Awake()
    {
        playerInput = new PlayerInputActions();
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
        // EnterInteract 이벤트가 발생했을 때 입력 필드의 텍스트를 가져와서 EndEdit 함수 호출
        EndEdit(inputField.text);
    }

    private void EndEdit(string text)
    {
        // Enter가 활성화되면 입력된 text를 디버그로 출력
        Debug.Log($"작성된 문자 : {text}");

        // 입력 필드의 텍스트를 초기화
        inputField.text = "";
        // 한글을 입력했을 때 맨 마지막 글자는 남아있는 문제 수정필요
    }
}
