using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkybox : MonoBehaviour
{
    void Start()
    {
        LoadRandomSkybox();
    }
    void LoadRandomSkybox()
    {
        Material[] skybox = Resources.LoadAll<Material>("SkyboxRandom");

        if(skybox != null && skybox.Length > 0)
        {
        int randomIndex = Random.Range(0, skybox.Length);
        RenderSettings.skybox = skybox[randomIndex];
        }
        else
        {
            Debug.Log("Mising Material!");
        }
    }
}
