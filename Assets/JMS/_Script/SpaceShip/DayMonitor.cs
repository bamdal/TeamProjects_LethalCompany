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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
