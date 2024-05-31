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

    public Transform ItemBox => itemBox;

    Terminal terminal;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        animator = GetComponent<Animator>();
        redButton = GetComponentInChildren<RedButton>();
        redButton.onRequest += ButtonClick;
        cinemachineImpulse = GetComponent<CinemachineImpulseSource>();
        terminal = GetComponentInChildren<Terminal>();

        itemBox = transform.GetChild(7);
    }

    private void ButtonClick()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (SceneManager.GetActiveScene().buildIndex != 4)
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

                StartCoroutine(LoadSpaceScene());

            }
            else
            {
                terminal.GoNextScene();
            }

        }
        else
        {
            terminal.GoNextScene();
        }

    }

    private void Start()
    {


        GameManager.Instance.onGameOver += OnGameOver;

    }

    public void SpaceShipDoorOpen()
    {
        animator.ResetTrigger("Open");
        animator.ResetTrigger("Close");
        animator.SetTrigger("Open"); 
    }


    public void SpaceShipDoorClose()
    {
        animator.ResetTrigger("Open");
        animator.ResetTrigger("Close");
        animator.SetTrigger("Close");
    }

    /// <summary>
    /// 버튼 눌렸을 때 시작되는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadSpaceScene()
    {


        AsyncOperation async = SceneManager.LoadSceneAsync(4, LoadSceneMode.Single);

        while (!async.isDone)
        {
            yield return new WaitForSeconds(1.0f);
        }

        GameManager.Instance.SpaceShip.transform.position = Vector3.zero;
        GameManager.Instance.SpaceShip.transform.rotation = Quaternion.identity;
        GameManager.Instance.Player.ControllerTPPosition(Vector3.zero);
        for (int i = 0; i < itemBox.childCount; i++)
        {
            itemBox.GetChild(i).gameObject.SetActive(true);
        }
    }


    /// <summary>
    /// 게임 오버시 이펙트들
    /// </summary>
    private void OnGameOver()
    {
        animator.SetTrigger("Open");
        Light light = transform.GetChild(5).GetComponent<Light>();
        light.enabled = false;
        StartCoroutine(ExplosionShip());
        Transform shipBase = transform.GetChild(1);
        Collider[] shipCollider = shipBase.GetComponentsInChildren<Collider>();

        foreach (Collider collider in shipCollider)
        {
            collider.enabled = false;
        }

        GameManager.Instance.Player.Die();

        StartCoroutine(ResetGameCorutine());



    }

    IEnumerator ResetGameCorutine()
    {
        yield return new WaitForSeconds (2.0f);
        GameManager.Instance.ResetGame();

    }

    public void Refresh()
    {
        animator.SetTrigger("Close");
        Light light = transform.GetChild(5).GetComponent<Light>();
        light.enabled = true;
        StopAllCoroutines();
        Transform shipBase = transform.GetChild(1);
        Collider[] shipCollider = shipBase.GetComponentsInChildren<Collider>();

        foreach (Collider collider in shipCollider)
        {
            collider.enabled = true;
        }

    }

    IEnumerator ExplosionShip()
    {
        int i = 0;
        while (i <10)
        {
            i++;
            cinemachineImpulse.GenerateImpulseWithVelocity(Random.onUnitSphere);
            cinemachineImpulse.GenerateImpulse();
            yield return new WaitForSeconds(Random.value);
        }
    }
}
