using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    float highDensity = 0.5f;
    float midDensity = 0.4f;
    float lowDensity = 0.2f;
    public float highDensityProbability = 0.2f;
    public float midDensityProbability = 0.5f;
/*    private float lowDensityPRobability;
    private float originalDensity;*/

    void Start()
    {
        if(highDensityProbability + midDensityProbability > 1f)
        {
            Debug.Log("확률의 합이 1이 되지 않습니다. 자동으로 조정합니다");
            float totalProbability = highDensityProbability + midDensityProbability;
            highDensityProbability /= totalProbability;
            midDensityProbability /= totalProbability;

            //lowDensityPRobability = 1f - (highDensityProbability + midDensityProbability);
        }
        int randomValue = Random.Range(0, 2);
        if (randomValue == 0)
        {
            RenderSettings.fog = false;
            Debug.Log("Fog off");
        }
        else
        {
            RenderSettings.fog = true;
            Debug.Log("Fog on");

            float randomProbability = Random.Range(0f, 1f);
            if (randomProbability < highDensityProbability)
            {
                RenderSettings.fogDensity = highDensity;
                Debug.Log("high fog Density: " + highDensity);
            }
            else if(randomProbability < highDensityProbability + midDensityProbability)
            {
                RenderSettings.fogDensity = midDensity;
                Debug.Log("mid fog Density:" + midDensity);
            }
            else
            {
                RenderSettings.fogDensity = lowDensity;
                Debug.Log("low fog Density: " + lowDensity);
            }
        }
        //originalDensity = RenderSettings.fogDensity;
        Debug.Log("현재 안개의 밀도:" + RenderSettings.fogDensity);

    }
    void Update() 
    {
        //Debug.Log("현재 안개의 밀도:" + RenderSettings.fogDensity);
    }
}
