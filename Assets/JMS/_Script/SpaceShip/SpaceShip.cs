using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();    
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
