using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    Animator animator;

    /// <summary>
    /// 함선의 있는 버튼(우주로 다시 돌아갈때 누르는 버튼)
    /// </summary>
    RedButton redButton;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        redButton =GetComponentInChildren<RedButton>();
        redButton.request += ButtonClick;
    }

    private void ButtonClick()
    {
        
    }

    private void Start()
    {

            AsyncStartScene asyncStart = FindAnyObjectByType<AsyncStartScene>();
            if(asyncStart != null)
            {
                asyncStart.onGameStart += () => { animator.SetTrigger("Open"); };
            }



    }
}
