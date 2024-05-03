using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnareFlea : EnemyBase
{
    TrapTrigger trapTrigger;
    float currentY;
    Coroutine raiseTrap = null;
    private void Awake()
    {
        trapTrigger = GetComponentInParent<TrapTrigger>();
        currentY = transform.position.y;
    }
    protected override void Start()
    {
        base.Start();
    }

    IEnumerator RaiseTrap()
    {
        while (transform.position.y < currentY)
        {
            float newY = transform.position.y + 1.0f * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, Mathf.Min(newY, currentY), transform.position.z);
            yield return null;
        }
        trapTrigger.IsLowering = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("Player"))
        {
            if (raiseTrap != null)
            {
                StopCoroutine(raiseTrap);
            }
            raiseTrap = StartCoroutine(RaiseTrap());
        }
    }
}
