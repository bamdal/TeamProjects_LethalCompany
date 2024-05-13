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

        GameManager.Instance.OnGameState = GameState.GameStart;
        if (SceneManager.GetActiveScene().name != "Company")
        {
            StartCoroutine(LoadDungenonScene());

        }
        else
        {
            StartCoroutine(Delay());
          
        }
    }

    public Action onSceneLoadComplite;

    IEnumerator LoadDungenonScene()
    {

        AsyncOperation async = SceneManager.LoadSceneAsync("DungenonScene", LoadSceneMode.Additive);

        while (!async.isDone)
        {
            yield return null;
        }
        GameManager.Instance.SpaceShip.transform.position = landPosition.position;
        GameManager.Instance.SpaceShip.transform.rotation = landPosition.rotation;
        GameManager.Instance.Player.ControllerTPPosition(landPosition.position);

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

        onSceneLoadComplite?.Invoke();
        GameManager.Instance.SpaceShip.SpaceShipDoorOpen();
    }

    IEnumerator Delay()
    {
        yield return null;
        GameManager.Instance.SpaceShip.transform.position = landPosition.position;
        GameManager.Instance.SpaceShip.transform.rotation = landPosition.rotation;
        GameManager.Instance.Player.ControllerTPPosition(landPosition.position);
        onSceneLoadComplite?.Invoke();
        GameManager.Instance.SpaceShip.SpaceShipDoorOpen();

    }
}
