using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private DateTime startTime;
    private TimeSpan duration;

    private void Awake()
    {
        timerText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        startTime = DateTime.Today.AddHours(7);
        duration = TimeSpan.FromMinutes(1); // 약 20분
    }

    void Update()
    {
        TimeSpan elapsed = DateTime.Now - startTime;
        TimeSpan remaining = duration - elapsed;

        // 시간 형식을 tt:mm으로 변환하여 TextMeshPro 텍스트에 표시
        timerText.text = string.Format("{0:D2}:{1:D2}", remaining.Hours, remaining.Minutes);
    }
}
