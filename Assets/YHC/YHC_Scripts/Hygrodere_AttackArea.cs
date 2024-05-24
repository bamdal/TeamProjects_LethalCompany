using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hygrodere_AttackArea : MonoBehaviour
{
    public float attackRadius = 1.0f;

    /// <summary>
    /// 플레이어가 공격 범위 내에 들어왔을 때 실행될 델리게이트
    /// </summary>
    public Action<Collider> onAttackIn;
    public Action<Collider> onAttackStay;

    public Action<Collider> onAttackOut;

    SphereCollider attackArea;

    private void Start()
    {
        attackArea = GetComponent<SphereCollider>();
        attackArea.radius = attackRadius;
        attackArea.enabled = false;
        attackArea.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onAttackIn?.Invoke(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            onAttackStay?.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onAttackOut?.Invoke(other);
        }
    }
}
