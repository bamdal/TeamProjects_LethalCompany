using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoadingScene : MonoBehaviour
{
    /// <summary>
    /// 다음에 로딩씬이 끝나고 나서 불려질 씬의 이름
    /// </summary>
    public string nextSceneName = "LoadSampleScene";

    AsyncStartScene asyncStartScene;


    /// <summary>
    /// slider의 value가 증가하는 속도(초당)
    /// </summary>
    public float loadingBarSpeed = 1.0f;

    /// <summary>
    /// 글자 변경용 코루틴
    /// </summary>
    IEnumerator loadingTextCoroutine;



    // UI
    Slider loadingSlider;
    TextMeshProUGUI loadingText;


    private void Start()
    {
        loadingSlider = FindAnyObjectByType<Slider>();
        loadingText = FindAnyObjectByType<TextMeshProUGUI>();

        loadingTextCoroutine = LoadingTextProgress();

        StartCoroutine(loadingTextCoroutine);
    }

    private void Update()
    {
        if(asyncStartScene != null) 
        {

            asyncStartScene = FindAnyObjectByType<AsyncStartScene>();
            asyncStartScene.onSceneLoadComplite += AsyncLoadScene;
        }
    }


    /// <summary>
    /// 글자의 모양을 계속 변경하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadingTextProgress()
    {        
        // 0.2초 간격으로 .이 찍힌다.
        // .은 최대 5개까지만 찍인다.
        // "Loading" ~ "Loading . . . . ."

        WaitForSeconds wait = new WaitForSeconds(0.2f);
        string[] texts =
        {
            "Loading",
            "Loading .",
            "Loading . .",
            "Loading . . .",
            "Loading . . . .",
            "Loading . . . . .",
        };

        int index = 0;
        while(true)
        {
            loadingText.text = texts[index];
            index++;
            index %= texts.Length;
            yield return wait;
        }
    }

    /// <summary>
    /// 비동기로 씬을 로딩하는 코루틴
    /// </summary>
    /// <returns></returns>
    void AsyncLoadScene()
    {





        StopCoroutine(loadingTextCoroutine);        // 글자 변경 안되게 만들기
        loadingText.text = "Loading\nComplete!";    // 완료되었다고 글자 출력
    }
}