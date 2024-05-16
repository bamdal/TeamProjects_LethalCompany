using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public DateTime startTime = DateTime.Parse("07:00");
    private DateTime lastEmittedTime;
    public Action<DateTime> OnTimeChanged;
    public Action<int> OnHourChanged;

    // 현실 시간과 게임 시간의 비율 (1초에 해당하는 현실 시간)
    private float gameSecondsPerRealSecond = 51.0f;

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
    }

    void Update()
    {
        // 현실 시간과의 비율을 곱해서 게임 시간을 업데이트
        CurrentTime = CurrentTime.AddSeconds(Time.deltaTime * gameSecondsPerRealSecond);

        // 게임 시간이 분 단위로 변경되고 이전에 발송된 시간이 아닌 경우에만 이벤트 발송
        if (CurrentTime.Minute % 5 == 0 && CurrentTime != lastEmittedTime)
        {
            lastEmittedTime = CurrentTime;
            OnTimeChanged?.Invoke(CurrentTime);
        }
    }
}
