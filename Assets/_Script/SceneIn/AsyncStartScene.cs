using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncStartScene : MonoBehaviour
{
    // Start is called before the first frame update
    Transform landPosition;

    private void Awake()
    {
        landPosition = transform.GetChild(0);
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Company")
        {
            StartCoroutine(LoadDungenonScene());

        }
        else
        {
            StartCoroutine(Delay());
          
        }
    }

    public Action onGameStart;

    IEnumerator LoadDungenonScene()
    {
        GameManager.Instance.SpaceShip.transform.position = landPosition.position;
        GameManager.Instance.SpaceShip.transform.rotation = landPosition.rotation;
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
        GameManager.Instance.SpaceShip.SpaceShipDoorOpen();
    }

    IEnumerator Delay()
    {
        yield return null;
        onGameStart?.Invoke();
        GameManager.Instance.SpaceShip.SpaceShipDoorOpen();

    }
}
