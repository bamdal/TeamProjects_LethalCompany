using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour,IInteraction
{
    Animator animator;

    readonly int Hash_Click = Animator.StringToHash("Click");
    public Action onRequest { get; set; }

    bool coolTime = true;
    public void Interaction(GameObject target)
    {
        animator.SetTrigger(Hash_Click);
        if (GameManager.Instance.GameState != GameState.GameOver)
        {
            GameManager.Instance.SpaceShip.SpaceShipDoorIdle();

            if(coolTime)
            {

                StartCoroutine(ButtonClick());
            }
        }

    }

    IEnumerator ButtonClick()
    {
        coolTime = false;
        yield return new WaitForSeconds(1.5f);
        onRequest?.Invoke();
        coolTime = true;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        coolTime = true;
    }
}
