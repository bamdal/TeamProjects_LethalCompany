using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreButton : MonoBehaviour, IInteraction
{
    Animator animator;

    readonly int Hash_Click = Animator.StringToHash("Click");
    public Action onRequest { get; set; }

    Store store;

    public void Interaction(GameObject target)
    {
        Debug.Log("실행");
        animator.SetTrigger(Hash_Click);
        onRequest?.Invoke();
        store.StoreInteraction();

    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        store = GetComponentInParent<Store>();
    }
}
