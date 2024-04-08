using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogControler : MonoBehaviour
    {
    private Flash flash;
    private Fog fog;
    private void Start()
    {
        flash = FindObjectOfType<Flash>();
        fog = FindObjectOfType<Fog>();
    }
    //private void Update()
    //{
    //    if (flash != null && fog != null)
    //   {
    //        if(flash.enabled)
    //       {
    //            fog.SetLowDensity();
    //            Debug.Log("Fog density set to low");
    //        }
    //        else
    //        {
    //           fog.SetHighDensity();
    //            Debug.Log("Fog density set to high");
    //        }
    //    }
    //}
}
