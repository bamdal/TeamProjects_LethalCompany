using UnityEngine;

public class RandomSkybox : MonoBehaviour
{
    public Material Sunrise;
    public Material Sunset;
    public Material Night_Moonless;
    public Material Night;
    public Material Day;
    public Material Day_Sunless;

    public Timer timer; // Timer 객체에 직접 접근

    void Start()
    {
        if (timer == null)
        {
            Debug.LogError("Timer is not initialized");
            return;
        }

        // 초기에 한 번 스카이박스 설정
        UpdateSkybox();
    }

    void Awake()
    {
        timer = FindObjectOfType<Timer>();

        if (timer == null)
        {
            Debug.LogError("Timer script not found");
            return;
        }

        UpdateSkybox();
    }

    void UpdateSkybox()
    {
        if (timer == null)
        {
            Debug.LogError("Timer is not initialized");
            return;
        }

        // 현재 시간 얻기
        float currentTime = timer.CurrentTime.Hour;

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
        else if (IsNight(currentTime))
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
