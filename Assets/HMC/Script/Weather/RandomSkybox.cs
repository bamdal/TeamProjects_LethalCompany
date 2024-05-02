using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomSkybox : MonoBehaviour
{
    /**public Material Sunrise;
    public Material Sunset;
    public Material Night_Moonless;
    public Material Night;
    public Material Day;
    public Material Day_Sunless;

    public delegate float TimeDelegate();
    public TimeDelegate getTime;

    void Start()
    {
        //getTime = ExternalTimeProvider.GetTime;   //시간을 받아 올 클래스.
        UpdateSkybox();
    }

    void UpdateSkybox()
    {
        float currentTime = getTime(); //현재 시간 얻기

        //시간에 따른 스카이박스 설정
        if(Issunrise(currentTime))
        {
            RenderSettings.skybox = Sunrise; //03~06
        }
        else if(Issunset(currentTime))    
        {
            RenderSettings.skybox = Sunset;  //15~18
        }
        else if(IsDay(currentTime))
        {
            if(Random.value < 0.5f)          //18~03
            {
                RenderSettings.skybox = Day;
            }
            else
            {
                RenderSettings.skybox = Day_Sunless;
            }
        }
        else
        {
            if(Random.value < 0.5f)           //03~06
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
        //시간대 여부 결정 코드
        //return;
    }
    bool Issunrise(float currentTime)
    {
        //시간대 여부 결정 코드
        return;
    }
    bool Issunset(float currentTime)
    {
        //시간대 여부 결정 코드 
        return;
    }**/
}
