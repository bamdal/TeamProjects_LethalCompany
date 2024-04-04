using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public float highDensity = 0.05f;
    public float LowDensity = 0.01f;
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
public class AnotherScript : MonoBehaviour
{
    private Flash flash;
    private void Start()
    {
        flash = FindObjectOfType<Flash>();
    }
    private void Update()
    {
        if(flash != enabled)
        {
            FindObjectOfType<Fog>().SetHighDensity();
        }
        else
        {
            FindObjectOfType<Fog>().SetLowDensity();
        }
    }
}