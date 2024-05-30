using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycle : MonoBehaviour
{
    public Action onDisable;

    Transform objectPool;

    private void Start()
    {
        GameManager.Instance.onGameReady += OnDisableObject;
        objectPool = transform.parent.GetComponent<Transform>();
    }

    private void OnDisableObject()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(false);
        }
    }

    protected virtual void OnDisable()
    {
        onDisable?.Invoke();    // 비활성화될때 큐에 되돌려야 하기때문에 델리게이트 실행해서 알리기
    }

    public Transform getParent()
    {
        return objectPool;
    }
}
