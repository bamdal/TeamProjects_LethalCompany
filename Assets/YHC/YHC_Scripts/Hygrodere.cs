using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Hygrodere : EnemyBase
{
    // 이동 관련 -----------------------------------------------------------

    const float waitTime = 2.0f;
    float wait = 0.0f;

    /// <summary>
    /// Hygro의 이동속도
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

    public float patrolMoveSpeed = 1.0f;
    public float chaseMoveSpeed = 2.0f;

    /// <summary>
    /// Hygro의 패트롤 범위, 목적지에 도작하면 patrolRange 범위 안에 새로운 랜덤 목적지 생성
    /// </summary>
    float patrolRange = 10.0f;

    /// <summary>
    /// 추적상태 돌입 범위
    /// </summary>
    public float chasePatrolTransitionRange = 5.0f;

    // HP 관련 -----------------------------------------------------------
    float hygroHp = 100.0f;

    public override float Hp
    {
        get => hygroHp;
        set
        {
            if (hygroHp != value)
            {
                hygroHp = value;
                hygroHp = Mathf.Clamp(hygroHp, 0, MaxHP);
                if (hygroHp <= 0)
                {
                    State = EnemyState.Die;
                    HygroDie();
                }
            }
        }
    }

    const float MaxHP = 30.0f;

    // 공격 관련 -----------------------------------------------------------

    /// <summary>
    /// Hygro의 공격력
    /// </summary>
    public int attackDamage = 9999;

    public int AttackDamage => attackDamage;

    /// <summary>
    /// 공격 쿨타임
    /// </summary>
    public float attackCoolTime = 0.5f;

    /// <summary>
    /// 현재 남아있는 쿨타임, 0이 되면 공격가능
    /// </summary>
    float currentAttackCoolTime;

    /// <summary>
    /// 공격이 가능한 상태(남아있는 쿨타임이 0 미만이다)
    /// </summary>
    bool IsCoolTime => currentAttackCoolTime < 0;

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

    public Hygrodere devidedHygro;

    /// <summary>
    /// 플레이어가 죽었는지 살았는지 확인하는 변수
    /// </summary>
    bool isPlayerDie = false;

    // 스폰 관련 -----------------------------------------------------------------------------

    /// <summary>
    /// 분열된 상태인지 아닌지 확인하는 변수, true면 분열된 상태, false면 분열 전
    /// </summary>
    public bool isDevided = false;

    public bool IsDevided
    {
        get => isDevided;
        set
        {
            if (isDevided != value)
            {
                isDevided = value;
            }
        }
    }

    /// <summary>
    /// 하이그로디어의 최대 스폰 수
    /// </summary>
    public int HygroMaxSpawnCount = 2;
    public override int MaxSpawnCount
    {
        get => HygroMaxSpawnCount;
        set => HygroMaxSpawnCount = value;
    }

    /// <summary>
    /// 하이그로디어의 스폰 확률
    /// </summary>
    public float HygroSpawnRate = 0.06f;

    public override float SpawnPercent
    {
        get => HygroSpawnRate;
        set => HygroSpawnRate = value;
    }

    // 컴포넌트
    NavMeshAgent agent;
    SphereCollider chaseRadius;
    Hygrodere_ChaseArea chaseArea;
    Hygrodere_AttackArea attackArea;

    protected override void Start()
    {
        base.Start();

        attackDamage = 35;
        currentAttackCoolTime = attackCoolTime;
        hygroHp = MaxHP;

        agent = GetComponent<NavMeshAgent>();
        chaseArea = GetComponentInChildren<Hygrodere_ChaseArea>();
        chaseRadius = chaseArea.GetComponent<SphereCollider>();
        attackArea = GetComponentInChildren<Hygrodere_AttackArea>();

        State = EnemyState.Stop;
        State = EnemyState.Patrol;

        agent.SetDestination(GetRandomDestination());
        MoveSpeed = patrolMoveSpeed;
        agent.speed = MoveSpeed;
        wait = waitTime;

        chaseRadius.radius = chasePatrolTransitionRange;
        attackArea.onAttackIn += AttackAreaPlayerIn;
        attackArea.onAttackOut += AttackAreaPlayerOut;
        chaseArea.onChaseIn += ChaseAreaPlayerIn;
        chaseArea.onChaseOut += ChaseAreaPlayerOut;

        IsAlive = true;

        StartCoroutine(SpawnTimeTriggerActivate());
    }


    protected override void Update()
    {
        base.Update();
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
                agent.SetDestination(GetRandomDestination());
                State = EnemyState.Patrol;
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
            // transform.Rotate(agent.destination);
        }
    }

    protected override void Update_Chase()
    {
        if (playerTransform != null)
        {
            agent.SetDestination(playerTransform.position);
            transform.Rotate(agent.destination);
        }
        else
        {
            State = EnemyState.Patrol;
        }
    }

    protected override void Update_Attack()
    {
        currentAttackCoolTime -= Time.deltaTime;

        if (playerTransform != null)
        {
            transform.Rotate(playerTransform.position);
        }

        if (IsCoolTime)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.forward, 0.5f);
            foreach (Collider collider in colliders)
            {
                IBattler battler = collider.GetComponent<IBattler>();
                if (battler != null)
                {
                    Attack(battler);
                }
            }
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

    private void ChaseAreaPlayerIn(Collider collider)
    {
        State = EnemyState.Chase;
    }

    private void ChaseAreaPlayerOut(Collider collider)
    {
        State = EnemyState.Patrol;
    }

    // 공격 관련 --------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 플레이어가 공격 범위 내에 들어왔을 때 상태 변경용 함수
    /// </summary>
    /// <param name="player">범위 내에 들어온 플레이어</param>
    private void AttackAreaPlayerIn(Collider collider)
    {
        // 플레이어일때만 실행되기때문에 추가 확인 필요 X
        State = EnemyState.Attack;
        if (IsCoolTime)
        {
            attackTarget = collider.GetComponent<IBattler>();
            Attack(attackTarget);
        }
    }

    private void AttackAreaPlayerOut(Collider collider)
    {
        playerTransform = null;
        attackTarget = null;
    }


    public void HygroDie()
    {
        if (!IsDevided)
        {
            gameObject.SetActive(false);

            StopAllCoroutines();
            StartCoroutine(DieCoroutine());

            IsAlive = false;
        }
        else
        {
            Destroy(this.gameObject, 3.0f);
        }
    }

    public void PlayerDie()
    {
        State = EnemyState.Stop;
        agent.speed = 0.0f;
    }


    public new void Attack(IBattler target)
    {
        target.Defense(AttackDamage);
    }

    public override void Defense(float attackPower)
    {
        Hp -= attackPower;
    }

    IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(3);

        Hygrodere newSlime1 = Instantiate(devidedHygro, transform.position, transform.rotation);
        newSlime1.IsDevided = true;
        Hygrodere newSlime2 = Instantiate(devidedHygro, transform.position, transform.rotation);
        newSlime2.IsDevided = true;
    }


    // 상태 관련 --------------------------------------------------------------------
    private void StateChange(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Die:
            case EnemyState.Stop:
            case EnemyState.Patrol:
            case EnemyState.Chase:
            case EnemyState.Attack:
                break;
        }
    }
}
