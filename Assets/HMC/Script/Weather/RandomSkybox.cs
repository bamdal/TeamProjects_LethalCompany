using System;
using UnityEngine;

public class RandomSkybox : MonoBehaviour
{
    public Material Sunrise;
    public Material Sunset;
    public Material Night_Moonless;
    public Material Night;
    public Material Day;
    public Material Day_Sunless;

    private Timer timer;

    void Start()
    {
        timer = FindObjectOfType<Timer>();

        if (timer == null)
        {
            Debug.LogError("Timer script not found");
            return;
        }

        // Timer 클래스의 이벤트에 대한 구독
        timer.OnTimeChanged += UpdateSkyboxWithDateTime;
        timer.OnHourChanged += UpdateSkyboxWithInt;

        // 초기에 한 번 스카이박스 설정
        UpdateSkyboxWithDateTime(timer.CurrentTime); // 현재 시간 값 전달
    }
    void UpdateSkyboxWithDateTime(DateTime currentTime)
    {
        // 시간에 따른 스카이박스 설정
        UpdateSkybox(currentTime.Hour);
    }

    void UpdateSkyboxWithInt(int currentTimeHour)
    {
        // 시간에 따른 스카이박스 설정
        UpdateSkybox(currentTimeHour);
    }

    void UpdateSkybox(int currentTimeHour)
    {
        if (IsSunrise(currentTimeHour))
        {
            RenderSettings.skybox = Sunrise; // 03~06
        }
        else if (IsSunset(currentTimeHour))
        {
            RenderSettings.skybox = Sunset; // 15~18
        }
        else if (IsDay(currentTimeHour))
        {
            if (UnityEngine.Random.value < 0.5f) // 18~03
            {
                RenderSettings.skybox = Day;
            }
            else
            {
                RenderSettings.skybox = Day_Sunless;
            }
        }
        else if (IsNight(currentTimeHour))
        {
            if (UnityEngine.Random.value < 0.5f) // 03~06
            {
                RenderSettings.skybox = Night;
            }
            else
            {
                RenderSettings.skybox = Night_Moonless;
            }
        }
    }

    bool IsDay(int currentTimeHour)
    {
        return currentTimeHour >= 10 && currentTimeHour < 13;
    }

    bool IsSunrise(int currentTimeHour)
    {
        return currentTimeHour >= 7 && currentTimeHour < 10;
    }

    bool IsSunset(int currentTimeHour)
    {
        return currentTimeHour >= 13 && currentTimeHour < 16;
    }

    bool IsNight(int currentTimeHour)
    {
        return currentTimeHour >= 16 && currentTimeHour < 19;
    }
}
