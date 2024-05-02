using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMiddleScene : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(NextDayCorurine());
    }

    IEnumerator NextDayCorurine()
    {
        yield return new WaitForEndOfFrame();
        GameManager.Instance.NextDay();

    }
}
