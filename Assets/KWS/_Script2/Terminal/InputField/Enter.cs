using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enter : MonoBehaviour
{
    public TMP_InputField inputField;


    private void Start()
    {
        // InputField의 OnEndEdit 이벤트에 함수를 연결합니다.
        inputField.onEndEdit.AddListener(EndEdit);
    }

    private void EndEdit(string text)
    {
        // 입력이 종료되면 실행될 코드를 작성합니다.
        Debug.Log("Input ended. Text: " + text);
    }
}
