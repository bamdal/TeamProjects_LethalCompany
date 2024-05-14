using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMiddleScene : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(NextDayCorurine());
        GameManager.Instance.GameState = GameState.GameReady;
    }

    IEnumerator NextDayCorurine()
    {
        yield return new WaitForEndOfFrame();
        GameManager.Instance.NextDay();

    }
}
