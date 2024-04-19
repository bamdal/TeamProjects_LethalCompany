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

    /// <summary>
    /// 
    /// </summary>
    public float chasePatrolTransitionRange = 20.0f;

    Action onStateTransition_Patrol_Chase;

    // 컴포넌트
    NavMeshAgent agent;
    SphereCollider chaseArea;
    
    private void Awake()
    {
        attackDamage = 90;
        attackCoolTime = 0.2f;
        currentAttackCoolTime = attackCoolTime;

        agent = GetComponent<NavMeshAgent>();
        chaseArea = GetComponent<SphereCollider>();
        chaseArea.radius = chasePatrolTransitionRange;
    }

    private void Start()
    {
        agent.SetDestination(SetRandomDestination());
    }

    protected override void Update()
    {
        base.Update();
        attackCoolTime -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            onStateTransition_Patrol_Chase?.Invoke();
        }
    }

    // 업데이트 함수들 ----------------------------------------------------------------------------------------------------------------
    protected override void Update_Stop()
    {

    }

    protected override void Update_Patrol()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            agent.SetDestination(SetRandomDestination());
        }
    }

    protected override void Update_Chase()
    {

    }

    protected override void Update_Attack()
    {

    }

    protected override void Update_Die()
    {

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

    /// <summary>
    /// Patrol 상태와 Chase 상태 전환용 함수
    /// </summary>
    void PatrolChaseMutualTranstion()
    {

    }   
}
