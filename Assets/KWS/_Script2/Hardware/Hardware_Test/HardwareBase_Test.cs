using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HardwareBase_Test : MonoBehaviour
{
    /// <summary>
    /// 폐철물의 무게
    /// </summary>
    [Range(10.0f, 50.0f)]
    public float hardwareWeight;

    /// <summary>
    /// 폐철물의 가격(min : 10, max : 50)
    /// </summary>
    [Range(100.0f, 500.0f)]
    public float hardwarePrice;

    protected virtual void HardwareWeight()
    {
        // 물건 종류마다 무게가 달라야 함
        // HardwareBase를 상속받는 곳에서 각각 가격 결정
        // 물건 크기에 따라 무게가 달라져야..?
    }

    protected virtual void HardwareSellPrice()
    {
        // 물건 종류마다 가격이 달라야 함
        // HardwareBase를 상속받는 곳에서 각각 가격 결정
    }
}
