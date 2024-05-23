using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_H : MonoBehaviour
{
    public float attackRange = 1.5f; // 공격 범위
    public float attackCooldown = 3f; // 공격 쿨다운 시간
    public float idleDuration = 2f; // 대기 시간
    public float lookAroundDuration = 5f; // LookAround 애니메이션 지속 시간

    private State currentState = State.IDLE; // 몬스터의 현재 상태
    private Transform target; // 추적할 대상 (플레이어)
    private NavMeshAgent agent; // NavMeshAgent 컴포넌트
    private Animator anim;
    private float lastAttackTime; // 마지막 공격 시간 기록 변수
    private bool isWalking = false;
    private bool isLookingAround = false;
    private float idleTimer = 0f;

    public enum State
    {
        IDLE,
        CHASE,
        ATTACK,
        DIE
    }

    private void Start()
    {
        // 플레이어 태그를 가진 객체를 찾아 추적 대상으로 설정
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent 컴포넌트 가져오기
        anim = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
        anim.SetBool("Walking", false); // 초기 Walking 상태를 false로 설정
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

        // LookAround 애니메이션이 끝난 후 IDLE 상태로 돌아가기 위한 타이머
        if (isLookingAround)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= lookAroundDuration)
            {
                isLookingAround = false;
                idleTimer = 0f;
                SetRandomDestination();
            }
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
            if (!isWalking && !isLookingAround)
            {
                SetRandomDestination();
            }
            else if (!agent.pathPending && agent.remainingDistance <= 0.1f && isWalking)
            {
                isWalking = false;
                anim.SetBool("Walking", false);
                idleTimer += Time.deltaTime;
                if (idleTimer >= idleDuration)
                {
                    idleTimer = 0f;
                    StartCoroutine(LookAround());
                }
            }
        }
    }

    private IEnumerator LookAround()
    {
        isLookingAround = true;
        anim.SetBool("LookAround", true);
        anim.SetBool("Walking", false);
        yield return new WaitForSeconds(lookAroundDuration);
        isLookingAround = false;
        anim.SetBool("LookAround", false);
        currentState = State.IDLE;
    }

    private void SetRandomDestination()
    {
        Vector3 randomDirection = Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0) * Vector3.forward;
        Vector3 targetPosition = transform.position + randomDirection * 10f;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, 10f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            isWalking = true;
            anim.SetBool("Walking", true);
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
        anim.SetBool("Walking", true);
    }

    private void Attack()
    {
        // 일정 시간 간격으로 공격
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            // 공격 코드 추가
            anim.SetTrigger("Attack");
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
        anim.SetTrigger("Die");
        Debug.Log("I'm dead!");
        Destroy(gameObject, 2f); // 2초 후에 객체 삭제
    }
}
