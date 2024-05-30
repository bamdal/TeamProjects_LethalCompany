using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public float highDensity = 0.8f;
    public float midDensity = 0.4f;
    public float highDensityProbability = 0.2f;
    private float originalDensity;

    void Start()
    {
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
                Debug.Log("높은 안개의 밀도 설정: " + highDensity);
            }
            else
            {
                RenderSettings.fogDensity = midDensity;
                Debug.Log("낮은 안개의 밀도 설정: " + midDensity);
            }
        }
        originalDensity = RenderSettings.fogDensity;
    }
}
