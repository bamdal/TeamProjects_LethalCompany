using UnityEngine;
using UnityEngine.AI;

public class Enemy_H : MonoBehaviour
{
    // 몬스터의 상태를 나타내는 열거형
    public enum State
    {
        IDLE,
        CHASE,
        ATTACK,
        DIE
    }

    public float attackRange = 1.5f; // 공격 범위
    public float attackCooldown = 3f; // 공격 쿨다운 시간
    public float idleDuration = 2f;

    private State currentState = State.IDLE; // 몬스터의 현재 상태
    private Transform target; // 추적할 대상 (플레이어)
    private NavMeshAgent agent; // NavMeshAgent 컴포넌트
    private Animator anim;
    private float lastAttackTime; // 마지막 공격 시간 기록 변수
    private bool isWalking = false;
    private bool needToIdle = false;
    private float idleTimer = 0f;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어를 추적 대상으로 설정
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent 컴포넌트 가져오기
        anim = GetComponent<Animator>();
        anim.SetBool("Walking", false);
    }

    private void Update()
    {
        // 현재 상태에 따라 적절한 동작 수행
        switch (currentState)
        {
            case State.IDLE:
                Idle();
                break;
            case State.CHASE:
                Chase();
                break;
            case State.ATTACK:
                Attack();
                break;
            case State.DIE:
                Die();
                break;
        }

        // 다음 Update에서 Idle() 메서드를 호출
        if (needToIdle)
        {
            needToIdle = false;
            Idle();
        }
    }

private void Idle()
{
    // 플레이어가 일정 범위 내에 있으면 추적 상태로 변경
    if (Vector3.Distance(transform.position, target.position) <= attackRange)
    {
        currentState = State.CHASE;
        Debug.Log("Chase");
    }
    else
    {
        if (!isWalking)
        {
            // 새로운 랜덤 방향 선택 전에 적이 바라보는 방향을 고려하여 회전
            Vector3 randomDirection = Quaternion.Euler(0, Random.Range(0f, 360f), 0) * transform.forward;
            Vector3 targetPosition = transform.position + randomDirection * 10f;
            NavMeshHit hit;
            NavMesh.SamplePosition(targetPosition, out hit, 10f, NavMesh.AllAreas);
            agent.SetDestination(hit.position);

            isWalking = true;
            idleTimer = 1.0f;
            anim.SetBool("Walking", true);
            anim.SetBool("LookAround", false); // 이동 중에는 LookAround 애니메이션 비활성화
        }
        else if (!agent.pathPending && agent.remainingDistance <= 0.1f)
        {
            idleTimer += Time.deltaTime;
            if(idleTimer >= idleDuration)
            {
                isWalking = false;
                needToIdle = true;
                anim.SetBool("Walking", false);
            }
        }
    }

    // LookAround 애니메이션을 대기 상태에서 실행
    if (!isWalking && needToIdle)
    {
        anim.SetBool("LookAround", true);
        // LookAround 애니메이션이 끝난 후에 다음 동작을 수행할 수 있도록 설정해야 합니다.
        // 예를 들어, LookAround 애니메이션이 끝나고 다음 상태로 전환하는 트리거를 설정합니다.
        // 이는 애니메이터에서 상태 전환을 제어하거나, 코드에서 트리거를 설정하여 구현할 수 있습니다.
    }
}

    private void Chase()
    {
        // NavMeshAgent를 이용하여 플레이어를 추적
        agent.SetDestination(target.position);

        // 공격 범위에 도달하면 공격 상태로 변경
        if (Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            currentState = State.ATTACK;
        }

        // 추적 중에 속도 높이기
        agent.speed = 3.0f;
    }

    private void Attack()
    {
        // 일정 시간 간격으로 공격
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            // 공격 코드 추가
            Debug.Log("Attack!");
            lastAttackTime = Time.time;
        }

        // 공격 범위를 벗어나면 추적 상태로 변경
        if (Vector3.Distance(transform.position, target.position) > attackRange)
        {
            currentState = State.CHASE;
        }
    }

    private void Die()
    {
        // 사망 처리 코드 추가
        Debug.Log("I'm dead!");
        Destroy(gameObject);

        // 몬스터가 사망하면 속도를 원래대로 복구
        agent.speed = 1.0f;
    }
}
