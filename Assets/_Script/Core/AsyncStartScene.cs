using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncStartScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadDungenonScene());
    }

    public Action onGameStart;

    IEnumerator LoadDungenonScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("DungenonScene", LoadSceneMode.Additive);

        while (!async.isDone)
        {
            yield return null;
        }

        // DungenonScene이 로드된 후에 StartGame 메서드를 호출
        GameObject dungeonScene = SceneManager.GetSceneByName("DungenonScene").GetRootGameObjects()[0];
        if (dungeonScene != null)
        {
            dungeonScene.GetComponent<DungeonGenerator>().StartGame();
        }

        // StartGame이 끝난 후에 async.isDone을 확인
        while (!dungeonScene.GetComponent<DungeonGenerator>().IsStartGameDone())
        {
            yield return null;
        }

        onGameStart?.Invoke();
    }
}
