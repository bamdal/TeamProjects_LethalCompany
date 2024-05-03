using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceShip : MonoBehaviour
{
    Animator animator;

    /// <summary>
    /// 함선의 있는 버튼(우주로 다시 돌아갈때 누르는 버튼)
    /// </summary>
    RedButton redButton;


    CinemachineImpulseSource cinemachineImpulse;

    Transform itemBox;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        animator = GetComponent<Animator>();
        redButton = GetComponentInChildren<RedButton>();
        redButton.onRequest += ButtonClick;
        cinemachineImpulse = GetComponent<CinemachineImpulseSource>();

        itemBox = transform.GetChild(7);
    }

    private void ButtonClick()
    {
        if (SceneManager.GetActiveScene().name != "MiddleScene" || SceneManager.GetActiveScene().name != "StartScene")
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(10, 5, 15) * 0.5f);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Item") || collider.CompareTag("Hardware"))
                {
                    Debug.Log(collider.name);
                    collider.gameObject.transform.parent = itemBox;

                }


            }

            StartCoroutine(LoadDungenonScene());

        }

    }

    private void Start()
    {

        AsyncStartScene asyncStart = FindAnyObjectByType<AsyncStartScene>();
        if (asyncStart != null)
        {
            asyncStart.onGameStart += () => { animator.SetTrigger("Open"); };
        }

        GameManager.Instance.onGameOver += OnGameOver;

    }

    public void SpaceShipDoorOpen()
    {
        animator.SetTrigger("Open"); 
    }

    private void OnGameOver()
    {
        animator.SetTrigger("Open");
        Light light = transform.GetChild(5).GetComponent<Light>();
        light.enabled = false;
        cinemachineImpulse.GenerateImpulseWithVelocity(Random.onUnitSphere);
        cinemachineImpulse.GenerateImpulse();

    }

    IEnumerator LoadDungenonScene()
    {
        GameManager.Instance.SpaceShip.transform.position = Vector3.zero;
        GameManager.Instance.SpaceShip.transform.rotation = Quaternion.identity;
        AsyncOperation async = SceneManager.LoadSceneAsync("MiddleScene", LoadSceneMode.Single);

        while (!async.isDone)
        {
            yield return null;
        }

    }
}
