using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    Timer timer;

    private void Awake()
    {
        timeText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        timer = FindAnyObjectByType<Timer>();
        timer.OnTimeChanged += TimerChange;
        timeText.text = timer.CurrentTime.ToString("HH:mm");
    }

    private void TimerChange(DateTime time)
    {
        timeText.text = timer.CurrentTime.ToString("HH:mm");
    }
}
