using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CoilHead : EnemyBase, IHealth
{
    /// <summary>
    /// CoilHead의 공격력
    /// </summary>
    int attackDamage = 0;
    public int AttackDamage => attackDamage;

    /// <summary>
    /// 공격 쿨타임
    /// </summary>
    float attackCoolTime;

    /// <summary>
    /// 현재 남아있는 쿨타임, 0이 되면 공격
    /// </summary>
    float currentAttackCoolTime;


    /// <summary>
    /// CoilHead의 이동속도
    /// </summary>
    float moveSpeed = 10;
    public float MoveSpeed
    {
        get => moveSpeed;
        set
        {
            moveSpeed = value;
        }
    }

    /// <summary>
    /// CoilHead의 패트롤 범위, 목적지에 도작하면 patrolRange 범위 안에 새로운 랜덤 목적지 생성
    /// </summary>
    float patrolRange = 100.0f;

    // 컴포넌트
    NavMeshAgent agent;

    private void Awake()
    {
        attackDamage = 90;
        attackCoolTime = 0.2f;
        currentAttackCoolTime = attackCoolTime;

        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.SetDestination(SetRandomDestination());
    }

    private void Update()
    {
        if(agent.remainingDistance < agent.stoppingDistance)
        {
            agent.SetDestination(SetRandomDestination());
        }

        currentAttackCoolTime -= Time.deltaTime;
    }

    /// <summary>
    /// 새로운 랜덤 목적지 생성
    /// </summary>
    /// <returns></returns>
    Vector3 SetRandomDestination()
    {
        Vector3 random = UnityEngine.Random.insideUnitSphere * patrolRange;
        random += transform.position;

        NavMeshHit hit;
        if(NavMesh.SamplePosition(random, out hit, patrolRange, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
