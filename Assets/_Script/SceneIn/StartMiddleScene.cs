using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMiddleScene : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.NextDay();
    }
}
