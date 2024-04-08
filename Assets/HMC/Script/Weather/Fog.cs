using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public float highDensity = 0.05f;
    public float lowDensity = 0.001f;
    private float originalDensity;
    void Start()
    {
        int randomValue = Random.Range(0,2);

        if(randomValue == 0)
        {
            RenderSettings.fog = false;
        }
        else
        {
            RenderSettings.fog = true;
            Debug.Log("Fog on");
        }
        originalDensity = RenderSettings.fogDensity;
    }
    public void SetHighDensity()
    {
        RenderSettings.fogDensity = highDensity;
    }
    public void SetLowDensity()
    {
        RenderSettings.fogDensity = originalDensity;
    }
}
