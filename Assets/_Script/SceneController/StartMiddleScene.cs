using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMiddleScene : MonoBehaviour
{
    private void Start()
    {
        if(GameManager.Instance.GameState != GameState.GameOver)
            StartCoroutine(NextDayCorurine());
        GameManager.Instance.GameState = GameState.GameReady;
    }

    IEnumerator NextDayCorurine()
    {
        yield return new WaitForEndOfFrame();
        GameManager.Instance.NextDay();

    }
}
