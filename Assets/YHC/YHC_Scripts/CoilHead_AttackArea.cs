using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoilHead_AttackArea : MonoBehaviour
{
    public float attackRadius = 1.5f;

    /// <summary>
    /// 플레이어가 공격 범위 내에 들어왔을 때 실행될 델리게이트
    /// </summary>
    public Action<Transform> onPlayerApproach;

    public Action onPlayerOut;
    
    SphereCollider attackArea;

    private void Awake()
    {
    }

    private void Start()
    {
        attackArea = GetComponent<SphereCollider>();
        attackArea.radius = attackRadius;
        attackArea.enabled = false;
        attackArea.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            onPlayerApproach?.Invoke(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            onPlayerOut?.Invoke();
        }
    }

}
