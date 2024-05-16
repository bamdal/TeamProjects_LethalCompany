using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadSceneController : MonoBehaviour
{
    float elapsedTime =0;
    private void Start()
    {
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
       
        while (true)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
            if (elapsedTime > 1.0f && Input.anyKeyDown)
            {
                AsyncOperation async = SceneManager.LoadSceneAsync("MiddleScene", LoadSceneMode.Single);
                while (!async.isDone)
                {
                    yield return null;
                }
            }
        }

    }
}
