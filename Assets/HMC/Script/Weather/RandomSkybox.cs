using System;
using UnityEngine;

public class RandomSkybox : MonoBehaviour
{
    //public Material Sunrise;
    public Material Sunset;
    public Material Night_Moonless;
    public Material Night;
    public Material Day;
    public Material Day_Sunless;

    private Timer timer;
    private Material selectedDaySkybox;
    private Material sealectedNightSkybox;
    private Material currentSkybox;

    void Start()
    {
        timer = FindObjectOfType<Timer>();

        if (timer == null)
        {
            Debug.LogError("Timer script not found");
            return;
        }
    
        selectedDaySkybox = UnityEngine.Random.value < 0.5f ? Day : Day_Sunless;
        sealectedNightSkybox = UnityEngine.Random.value < 0.5f ? Night : Night_Moonless;
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
        Material newSkybox = null;
        
        if (IsSunset(currentTimeHour))
        {
            newSkybox = Sunset; // 15~18
        }
        else if (IsDay(currentTimeHour))
        {
            newSkybox = selectedDaySkybox;
        }
        else if (IsNight(currentTimeHour))
        {
            newSkybox = sealectedNightSkybox;
        }
        if(newSkybox != null && newSkybox != currentSkybox)
        {
            RenderSettings.skybox = newSkybox;
            currentSkybox = newSkybox;
            Debug.Log("Skybox updated to : " + currentSkybox.name);
        }
    }

    bool IsDay(int currentTimeHour)
    {
        return currentTimeHour >= 7 && currentTimeHour < 13;
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
