using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CoilHead : EnemyBase, IBattler, IHealth
{
    // 이동 관련 -----------------------------------------------------------

    const float waitTime = 3.0f;
    float wait = 0.0f;

    /// <summary>
    /// CoilHead의 이동속도
    /// </summary>
    float moveSpeed = 0.0f;
    float MoveSpeed
    {
        get => moveSpeed;
        set
        {
            if (moveSpeed != value)
            {
                moveSpeed = value;
                agent.speed = moveSpeed;
            }
        }
    }

    public float patrolMoveSpeed = 2.0f;
    public float chaseMoveSpeed = 5.0f;

    /// <summary>
    /// CoilHead의 패트롤 범위, 목적지에 도작하면 patrolRange 범위 안에 새로운 랜덤 목적지 생성
    /// </summary>
    float patrolRange = 100.0f;

    /// <summary>
    /// 추적상태 돌입 범위
    /// </summary>
    public float chasePatrolTransitionRange = 10.0f;

    // 공격 관련 -----------------------------------------------------------

    // float hp = 100.0f;


    /// <summary>
    /// CoilHead의 공격력
    /// </summary>
    int attackDamage = 9999;

    public int AttackDamage => attackDamage;

    /// <summary>
    /// 공격 쿨타임
    /// </summary>
    public float attackCoolTime = 5.0f;

    /// <summary>
    /// 현재 남아있는 쿨타임, 0이 되면 공격가능
    /// </summary>
    float currentAttackCoolTime;

    /// <summary>
    /// 공격이 가능한 상태(남아있는 쿨타임이 0 미만이다)
    /// </summary>
    bool IsCoolTime => currentAttackCoolTime < 0;

    /// <summary>
    /// 눈이 마주쳤다고 할 수 있는 시야 각도(25도 미만이면 눈이 마주쳤다.
    /// </summary>
    public float cognitionAngle = 25.0f;

    /// <summary>
    /// 현재 플레이어와 적 사이의 시야 각도
    /// </summary>
    float currentAngle = -1.0f;

    /// <summary>
    /// 플레이어의 트랜스폼
    /// </summary>
    Transform playerTransform;

    /// <summary>
    /// 공격 대상(필요시 playerTransform에서 GetCompnent실행)
    /// </summary>
    IBattler attackTarget;

    /// <summary>
    /// 적이 죽어있는지 살아있는지 확인하는 변수
    /// </summary>
    bool isAlive = true;

    /// <summary>
    /// 적이 죽었는지 살았는지 확인하고 설정하는 프로퍼티
    /// </summary>
    public bool IsAlive
    {
        get => isAlive;
        set
        {
            if (isAlive != value)
            {
                isAlive = value;
            }
        }
    }

    /// <summary>
    /// 플레이어가 죽었는지 살았는지 확인하는 변수
    /// </summary>
    bool isPlayerDie = false;

    // 컴포넌트
    NavMeshAgent agent;
    SphereCollider chaseArea;
    CoilHead_AttackArea attackArea;

    private void Awake()
    {
        attackDamage = 90;
        currentAttackCoolTime = attackCoolTime;

        agent = GetComponent<NavMeshAgent>();
        chaseArea = GetComponent<SphereCollider>();
        attackArea = GetComponentInChildren<CoilHead_AttackArea>();
    }

    protected override void Start()
    {
        base.Start();

        State = EnemyState.Stop;
        State = EnemyState.Patrol;

        agent.SetDestination(GetRandomDestination());
        MoveSpeed = patrolMoveSpeed;
        agent.speed = MoveSpeed;
        wait = waitTime;

        chaseArea.radius = chasePatrolTransitionRange;
        attackArea.onPlayerApproach += AttackAreaApproach;
        attackArea.onPlayerOut += (() =>
        {
            State = EnemyState.Chase;
        });

        StartCoroutine(SpawnTimeTriggerActivate());
    }

    protected override void Update()
    {
        base.Update();
        if (playerTransform != null)
        {

            currentAngle = GetSightAngle(playerTransform);              // 플레이어와 적 사이의 각도 측정

            transform.rotation = Quaternion.Slerp(transform.rotation
                , Quaternion.LookRotation(playerTransform.transform.position - transform.position), 0.1f);
            
            if(!IsSightCheck(playerTransform))
            {
                if (currentAngle < cognitionAngle)
                {
                    agent.speed = 0.0f;
                    agent.velocity = Vector3.zero;
                }
                else
                {
                    agent.speed = chaseMoveSpeed;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 발견");
            playerTransform = other.transform;
            MoveSpeed = chaseMoveSpeed;
            agent.speed = MoveSpeed;
            State = EnemyState.Chase;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 도망");
            MoveSpeed = patrolMoveSpeed;
            State = EnemyState.Patrol;
            agent.speed = patrolMoveSpeed;
            attackTarget = null;
            playerTransform = null;
        }
    }

    IEnumerator SpawnTimeTriggerActivate()
    {
        chaseArea.enabled = false;
        yield return null;
        chaseArea.enabled = true;
    }

    // 업데이트 함수들 ----------------------------------------------------------------------------------------------------------------
    protected override void Update_Stop()
    {
        wait -= Time.deltaTime;
        if (!isPlayerDie)
        {
            if (wait < 0.0f)
            {
                State = EnemyState.Patrol;
                agent.SetDestination(GetRandomDestination());
                wait = waitTime;
            }
        }
        else
        {
            agent.speed = 0.0f;
        }
    }

    protected override void Update_Patrol()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            agent.SetDestination(GetRandomDestination());
        }
    }

    protected override void Update_Chase()
    {
        if (playerTransform != null)
        {
            agent.SetDestination(playerTransform.position);
        }
        else
        {
            State = EnemyState.Patrol;
        }
    }

    protected override void Update_Attack()
    {
        currentAttackCoolTime -= Time.deltaTime;

        agent.SetDestination(playerTransform.position + new Vector3 (1, 0, 1));

        if (IsCoolTime)
        {
            Attack(attackTarget);
            currentAttackCoolTime = attackCoolTime;
        }
    }

    protected override void Update_Die()
    {

    }

    // 이동 관련 ----------------------------------------------------------------------------------------------------

    /// <summary>
    /// 새로운 랜덤 목적지 생성
    /// </summary>
    /// <returns></returns>
    Vector3 GetRandomDestination()
    {
        Vector3 random = UnityEngine.Random.insideUnitSphere * patrolRange;
        random += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(random, out hit, patrolRange, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return Vector3.zero;
        }
    }

    // 공격 관련 --------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 플레이어가 공격 범위 내에 들어왔을 때 상태 변경용 함수
    /// </summary>
    /// <param name="player">범위 내에 들어온 플레이어</param>
    private void AttackAreaApproach(Transform player)
    {
        // 플레이어일때만 실행되기때문에 추가 확인 필요 X
        playerTransform = player;
        attackTarget = player.GetComponent<IBattler>();
        State = EnemyState.Attack;
    }

    // 적과 플레이어 시야 관련 ------------------------------------------------------------------------------------------

    /// <summary>
    /// 플레이어의 시야 범위 내에 적이 있는지 확인하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <returns>플레이어와 적 사이의 각도</returns>
    float GetSightAngle(Transform player)
    {
        Vector3 dir = transform.position - player.transform.position;
        float angle = Vector3.Angle(player.transform.forward, dir);

        return angle;
    }

    /// <summary>
    /// 적과 플레이어 사이에 물체가 있는지 확인하는 함수
    /// </summary>
    /// <param name="player"></param>
    /// <returns>true면 사이에 물체가 없다, false면 사이에 물체가 있다.</returns>
    public bool IsSightCheck(Transform player)
    {
        bool result = false;
        Vector3 dir = player.transform.position - transform.position;
        Ray ray = new(transform.position, dir);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                result = true;
            }
        }

        return result;
    }

    public void CoilHeadDie()
    {
        State = EnemyState.Die;
        IsAlive = false;
    }

    public void PlayerDie()
    {
        State = EnemyState.Stop;
    }
}
