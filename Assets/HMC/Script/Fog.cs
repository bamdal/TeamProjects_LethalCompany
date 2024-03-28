using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public float fogDensityWhenFlashloghtOn = 0.03f;    //손전등이 켜진 상태에서의 안개 밀도
    public float fogDensityWhenFlashloghtOff = 0.1f;    //손전등이 꺼진 상태에서의 안개 밀도

    private GameObject flashlight;       //손전등 오브젝트찾기
    private bool lastFlashlightState;   //이전 프레임의 손전등 상태 저장용 변수

    void Start()
    {
        flashlight = GameObject.Find("flashlight");  //손전등 오브젝트 찾기
        UpdateFogDensity();                          //초기 안개 설정
        lastFlashlightState = flashlight != null && flashlight.activeSelf;  //초기 손전등 상태 저장
    }
    void Update() 
    {
        if(flashlight != null && flashlight.activeSelf !=lastFlashlightState)  //손전등 상태에 따라 안개 밀도 업데이트
        {
        UpdateFogDensity();
        }
        lastFlashlightState = flashlight != null && flashlight.activeSelf;// 현재 손전등 상태를 저장
    }
    void UpdateFogDensity()
    {
        if(flashlight != null)
        {
        bool flashlightOn = flashlight.activeSelf;  //손전등이 켜져있는지 확인

        RenderSettings.fogDensity = flashlightOn ? fogDensityWhenFlashloghtOn : fogDensityWhenFlashloghtOff;// 손전등 상태에 따라 안개 밀도 설정
        }
    }

    void OnDestrot()
    {
        flashlight = null;
        //손전등의 사용시간이 끝나거나 사라지면 손전등 정보 삭제를 위한 예시 코드.
    }
}
