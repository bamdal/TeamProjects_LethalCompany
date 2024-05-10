using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour,IInteraction
{
    Animator animator;

    readonly int Hash_Click = Animator.StringToHash("Click");
    public Action onRequest { get; set; }

    public void Interaction(GameObject target)
    {
        animator.SetTrigger(Hash_Click);
        if (GameManager.Instance.OnGameState != GameState.GameOver)
        {
            GameManager.Instance.SpaceShip.SpaceShipDoorClose();


            StartCoroutine(ButtonClick());
        }

    }

    IEnumerator ButtonClick()
    {
        yield return new WaitForSeconds(1.5f);
        onRequest?.Invoke();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
}
