using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public DateTime startTime = DateTime.Parse("07:00");
    private TimeSpan totalTime = TimeSpan.FromMinutes(20);
    private DateTime currentTime;
    private DateTime lastEmittedTime;
    public Action<DateTime> OnTimeChanged;
    public Action OnHourChangeed;
    public DateTime CurrentTime
    {
        get => currentTime;
        private set
        {
            if (currentTime != value)
            {
                if(currentTime.Hour != value.Hour)
                {
                    OnHourChangeed?.Invoke();
                }
                currentTime = value;
//                OnTimeChanged?.Invoke(currentTime); // 시간 변경 시 이벤트 호출
            }
        }
    }

    

    void Awake()
    {
        // 시작 시간 설정
        CurrentTime = startTime;
    }

    void Update()
    {
        // 현재 시간 계산
        CurrentTime = startTime.Add(totalTime * Time.time / 60f);
        if (CurrentTime.Minute % 5 == 0 && CurrentTime != lastEmittedTime)
        {
            lastEmittedTime = CurrentTime;
            OnTimeChanged?.Invoke(CurrentTime);
            //Debug.Log("바뀜" + CurrentTime);
        }
    }
}
