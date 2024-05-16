using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public DateTime startTime = DateTime.Parse("07:00");
    private DateTime lastEmittedTime;
    public Action<DateTime> OnTimeChanged;
    public Action<int> OnHourChanged;
    [Header("인게임 시간을 분으로")]
    public int totalGameTimeMinute = 1020;
    [Header("현실시간을 분으로")]
    public int realTimeMinute = 20;

    // 현실 시간과 게임 시간의 비율
    private float gameSecondsPerRealSecond = 0;

    private DateTime currentTime;
    public DateTime CurrentTime
    {
        get => currentTime;
        private set
        {
            if (currentTime != value)
            {
                if (currentTime.Hour != value.Hour)
                {
                    OnHourChanged?.Invoke(value.Hour);
                }
                currentTime = value;
            }
        }
    }

    void Awake()
    {
        CurrentTime = startTime;
        gameSecondsPerRealSecond = totalGameTimeMinute / realTimeMinute;
    }

    void Update()
    {

        CurrentTime = CurrentTime.AddSeconds(Time.deltaTime * gameSecondsPerRealSecond);

        if (CurrentTime.Minute % 5 == 0 && CurrentTime != lastEmittedTime)
        {
            lastEmittedTime = CurrentTime;
            OnTimeChanged?.Invoke(CurrentTime);
        }
    }
}
