using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CoilHead : EnemyBase, IHealth
{
    int attackDamage = 0;
    public int AttackDamage => attackDamage;

    float attackCoolTime;

    float currentAttackCoolTime;

    float moveSpeed = 10;
    public float MoveSpeed
    {
        get => moveSpeed;
        set
        {
            moveSpeed = value;
        }
    }

    NavMeshAgent agent;

    EnemyState coilheadState;
    public EnemyState CoilHeadState
    {
        get => coilheadState;
        set
        {
            if(coilheadState != value)
            {
                coilheadState = value;
                switch(coilheadState)
                {
                    case EnemyState.Stop:
                        break;
                    case EnemyState.Patrol:
                        MoveSpeed = 7.0f;
                        break;
                    case EnemyState.Chase:
                        MoveSpeed = 3.0f;
                        break;
                    case EnemyState.Attack:
                        break;
                    case EnemyState.Die:
                        break;
                    default:
                        break;

                }
            }
        }
    }

    Vector3 randomPos;

    private void Awake()
    {
        attackCoolTime = 90;
        attackCoolTime = 0.2f;
        currentAttackCoolTime = attackCoolTime;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.speed = moveSpeed;
        agent.stoppingDistance = 0.1f;
        randomPos = Vector3.zero;
    }
    private void Update()
    {
        if(agent.remainingDistance < agent.stoppingDistance)
        {
            randomPos = new Vector3(UnityEngine.Random.Range(-30.0f, 30.0f), transform.position.y, UnityEngine.Random.Range(-30.0f, 30.0f));
            Debug.Log($"{randomPos}");
            agent.SetDestination(randomPos);
        }
    }

    public void TracePlayer(Vector3 pos)
    {
        agent.SetDestination(pos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            agent.SetDestination(other.transform.position);
        }
    }
}
