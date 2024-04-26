using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour,IInteraction
{
    Animator animator;

    readonly int Hash_Click = Animator.StringToHash("Click");
    public Action request { get; set; }

    public void Interaction(GameObject target)
    {
        
        animator.SetTrigger(Hash_Click);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
}
