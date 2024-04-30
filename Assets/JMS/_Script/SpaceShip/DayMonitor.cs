using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayMonitor : MonoBehaviour
{
    TextMeshPro DayText;

    private void Awake()
    {
        DayText = GetComponentInChildren<TextMeshPro>();
    }


    void Start()
    {
        GameManager.Instance.onDayChange += OnDayChange;
    }

    private void OnDayChange(int day)
    {
        DayText.text = $"D-{day}";
    }

 
}
