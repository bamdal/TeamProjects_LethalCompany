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
        if (SceneManager.GetActiveScene().name != "StartScene")
        {
            if (SceneManager.GetActiveScene().name != "MiddleScene")
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

        }

    }

    private void Start()
    {


        GameManager.Instance.onGameOver += OnGameOver;

    }

    public void SpaceShipDoorOpen()
    {
        animator.SetTrigger("Open"); 
    }

    public void SpaceShipDoorClose()
    {
        animator.SetTrigger("Close");
    }


    private void OnGameOver()
    {
        animator.SetTrigger("Open");
        Light light = transform.GetChild(5).GetComponent<Light>();
        light.enabled = false;
        cinemachineImpulse.GenerateImpulseWithVelocity(Random.onUnitSphere);
        cinemachineImpulse.GenerateImpulse();

    }

    IEnumerator LoadSpaceScene()
    {


        AsyncOperation async = SceneManager.LoadSceneAsync("MiddleScene", LoadSceneMode.Single);

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
}
