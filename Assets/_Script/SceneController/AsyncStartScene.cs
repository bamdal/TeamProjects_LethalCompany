using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncStartScene : MonoBehaviour
{
    // Start is called before the first frame update

    /// <summary>
    /// 함선이 내릴 좌표용 트랜스폼
    /// </summary>
    Transform landPosition;

    /// <summary>
    /// 택배 상자 도착용 좌표 트랜스폼
    /// </summary>
    Transform dropBoxPosition;

    EnemySpawner spawner;

    public List<EnemySpawnPoint> enemySpawnPoints;

    public LoadingPanel loadingPanel;

    private void Awake()
    {
        landPosition = transform.GetChild(0);
        dropBoxPosition = transform.GetChild(1);
        spawner = GetComponent<EnemySpawner>();

    }

    void Start()
    {
        if (loadingPanel == null)
        {
            loadingPanel = FindAnyObjectByType<LoadingPanel>();
        }

        GameManager.Instance.GameState = GameState.GameStart;
        GameManager.Instance.DropBoxManager.dropPosition = dropBoxPosition;
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

        AsyncOperation async = SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);

        while (!async.isDone)
        {
            yield return null;
        }

        GameManager.Instance.SpaceShip.transform.position = landPosition.position;
        GameManager.Instance.SpaceShip.transform.rotation = landPosition.rotation;
        GameManager.Instance.Player.ControllerTPPosition(landPosition.position);

        // DungenonScene이 로드된 후에 StartGame 메서드를 호출
        GameObject dungeonScene = SceneManager.GetSceneByBuildIndex(3).GetRootGameObjects()[0];
        if (dungeonScene != null)
        {
            dungeonScene.GetComponent<DungeonGenerator>().StartGame();
        }

        // StartGame이 끝난 후에 async.isDone을 확인
        while (!dungeonScene.GetComponent<DungeonGenerator>().IsStartGameDone())
        {
            yield return null;
        }

        spawner.OnSpawnEnemy(enemySpawnPoints);

        onSceneLoadComplite?.Invoke();

        loadingPanel.CanvasGroupAlphaChange();
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
