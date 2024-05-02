using UnityEngine;

public class RandomSkybox : MonoBehaviour
{
    public Material Sunrise;
    public Material Sunset;
    public Material Night_Moonless;
    public Material Night;
    public Material Day;
    public Material Day_Sunless;

    public delegate float TimeDelegate();
    public TimeDelegate getTime; // 현재 시간을 가져오는 델리게이트

    void Start()
    {
        // 초기에 한 번 스카이박스 설정
        UpdateSkybox();
    }

    void UpdateSkybox()
    {
        // 현재 시간 얻기
        float currentTime = getTime();

        // 시간에 따른 스카이박스 설정
        if (IsSunrise(currentTime))
        {
            RenderSettings.skybox = Sunrise; // 03~06
        }
        else if (IsSunset(currentTime))
        {
            RenderSettings.skybox = Sunset; // 15~18
        }
        else if (IsDay(currentTime))
        {
            if (Random.value < 0.5f) // 18~03
            {
                RenderSettings.skybox = Day;
            }
            else
            {
                RenderSettings.skybox = Day_Sunless;
            }
        }
        else if(IsNight(currentTime))
        {
            if (Random.value < 0.5f) // 03~06
            {
                RenderSettings.skybox = Night;
            }
            else
            {
                RenderSettings.skybox = Night_Moonless;
            }
        }
    }

    bool IsDay(float currentTime)
    {
        return currentTime >= 10 && currentTime < 13;
    }

    bool IsSunrise(float currentTime)
    {
        return currentTime >= 7 && currentTime < 10;
    }

    bool IsSunset(float currentTime)
    {
        return currentTime >= 13 && currentTime < 16;
    }
    bool IsNight(float currentTime)
    {
        return currentTime >= 16 && currentTime < 19;
    }
}
