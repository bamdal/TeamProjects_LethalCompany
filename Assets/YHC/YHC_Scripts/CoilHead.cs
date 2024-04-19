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
    float moveSpeed = 0.0f;
    float MoveSpeed
    {
        get => moveSpeed;
        set
        {
            if(moveSpeed != value)
            {
                moveSpeed = value;
                agent.speed = moveSpeed;
            }
        }
    }

    public float patrolMoveSpeed = 5.0f;
    public float chaseMoveSpeed = 7.0f;

    /// <summary>
    /// CoilHead의 패트롤 범위, 목적지에 도작하면 patrolRange 범위 안에 새로운 랜덤 목적지 생성
    /// </summary>
    float patrolRange = 100.0f;

    /// <summary>
    /// 추적상태 돌입 범위
    /// </summary>
    public float chasePatrolTransitionRange = 20.0f;

    /// <summary>
    /// 눈 마주칠때 범위
    /// </summary>
    public float cognitionRange = 10.0f;

    Action onStateTransition_Patrol_Chase;

    // 컴포넌트
    NavMeshAgent agent;
    SphereCollider chaseArea;

    Player player;
    
    private void Awake()
    {
        attackDamage = 90;
        attackCoolTime = 2.0f;
        currentAttackCoolTime = attackCoolTime;

        agent = GetComponent<NavMeshAgent>();
        chaseArea = GetComponent<SphereCollider>();
        chaseArea.radius = chasePatrolTransitionRange;
    }

    private void Start()
    {
        agent.SetDestination(SetRandomDestination());
        MoveSpeed = patrolMoveSpeed;
        agent.speed = MoveSpeed;
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
            State = EnemyState.Chase;
            MoveSpeed = chaseMoveSpeed;
            player = GameManager.Instance.Player;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        State = EnemyState.Patrol;
        MoveSpeed = patrolMoveSpeed;
        player = null;
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
        if(PlayerEncounter())
        {
            agent.speed = 0.0f;
        }
        else
        {
            agent.speed = chaseMoveSpeed;
        }
    }

    protected override void Update_Attack()
    {
        currentAttackCoolTime -= Time.deltaTime;
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

    /// <summary>
    /// 적과 플레이어가 눈을 마주쳤는지 확인하는 함수
    /// </summary>
    bool PlayerEncounter()
    {
        bool result = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, cognitionRange, LayerMask.GetMask("Player")))
        {
            if((player.transform.forward - transform.position).sqrMagnitude < 30.0f)
            {
                result = true;
            }
        }
        
        return result;
    }
}
