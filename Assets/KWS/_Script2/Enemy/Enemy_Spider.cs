using System;
using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Spider : EnemyBase
{
    /// <summary>
    /// 트랩이 내려가는 속도
    /// </summary>
    public float loweringSpeed = 1f;

    /// <summary>
    /// 트랩이 올라가는 속도
    /// </summary>
    public float raisingSpeed = 1f;

    /// <summary>
    /// 트랩이 내려갈 위치(y값)
    /// </summary>
    public float trapLowerPosition = 1f;

    /// <summary>
    /// 중력 가속도
    /// </summary>
    public float gravityAcceleration = 9.8f;

    /// <summary>
    /// 최대 HP
    /// </summary>
    public float MaxHp = 20.0f;

    /// <summary>
    /// 최대 HP
    /// </summary>
    public override float Hp
    {
        get => MaxHp;
        set
        {
            if(value < 1)
            {
                State = EnemyState.Die;
                NoPath();
            }
        }
        //set => base.Hp = value;
    }

    /// <summary>
    /// 최대 스폰 가능한 마릿수
    /// </summary>
    public override int MaxSpawnCount
    {
        get => UnityEngine.Random.Range(3,6);       // 최대 3 ~ 5마리
        //set => base.MaxSpawnCount = value;
    }

    /// <summary>
    /// 게임내에 1개의 개체가 스폰될 확률(0~1)
    /// </summary>
    public override float SpawnPercent
    {
        get => 0.5f;        // 50%의 확률로 스폰
        //set => base.SpawnPercent = value;
    }

    private bool isLowering = false;
    Coroutine lowerTrap = null;
    public bool IsLowering
    {
        get => isLowering;
        set
        {
            if (isLowering != value)
            {
                isLowering = value;
            }
        }
    }



    /// <summary>
    /// 자식 오브젝트의 트랜스폼
    /// </summary>
    Transform childEnemy;

    /// <summary>
    /// 자식으로 붙어있는 오브젝트
    /// </summary>
    Enemy_Child_KWS enemy_Child;
    
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    /// <summary>
    /// 네브메시 에이전트
    /// </summary>
    private NavMeshAgent agent;

    [Range(1f, 5f)]
    public float moveSpeed = 1.0f;

    /*[Range(0.01f, 1f)]
    public float jumpHeight = 0.01f;*/

    [Range(1f, 10f)]
    public float stopDistance = 1.0f;

    [Range(1f, 10f)]
    public float rotationSpeed = 10.0f;

    /// <summary>
    /// rigid의 gravity를 끄기 위한 델리게이트
    /// </summary>
    public Action onRaise;

    /// <summary>
    /// rigid의 gravity를 켜기 위한 델리게이트
    /// </summary>
    public Action onLower;


    private void Awake()
    {
        childEnemy = transform.GetChild(0);       // 0번째 자식 Enemy_Spider
        enemy_Child = childEnemy.GetComponent<Enemy_Child_KWS>();
        Collider collider = transform.GetComponent<Collider>();  // 플레이어를 탐지할 콜라이더
        onEnemyStateUpdate = Update_Stop;
    }

    protected override void Start()
    {        
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed; // 이동 속도 설정
        //player = GameObject.FindWithTag("Player");
        player = GameManager.Instance.Player;
        agent.stoppingDistance = stopDistance;
        //State = EnemyState.Stop;
    }

    protected override void Update()
    {
        base.Update();
        //bool isGrounded = enemy_Child.IsGrounded();
        //onEnemyStateUpdate();
    }

    private void FixedUpdate()
    {
        //onEnemyStateUpdate();
    }
    
    protected override void Update_Stop()
    {
        StopCoroutine(enemy_Child.RaiseTrap());
        if(enemy_Child.IsGrounded())
        {
            Debug.Log("중력 조작");
            onRaise?.Invoke();
            
            StartCoroutine(enemy_Child.RaiseTrap());
        }
    }

    protected override void Update_Patrol()
    {

    }

    public Action OnChase;

    protected override void Update_Chase()
    {
        //Debug.Log("Update_Chase 상태 실행");
        onLower?.Invoke();
        OnChase?.Invoke();
        if (player != null)
        {
            // 플레이어를 향해 회전
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0; // y축 회전 방지
            //transform.rotation = Quaternion.LookRotation(direction);

            // 목표 회전 각도를 계산
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // 회전 속도에 따라 부드럽게 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 플레이어와의 거리 계산
            float distance = Vector3.Distance(transform.position, player.transform.position);

            // 플레이어와의 거리가 일정 범위 이상이면 이동
            if (distance > agent.stoppingDistance)
            {
                // 플레이어를 향해 이동
                agent.SetDestination(player.transform.position);
                enemy_Child.Jump();
            }
            else
            {
                NoPath();
                /*// 적이 플레이어 근처에 있을 때 가해지던 힘 제거
                agent.velocity = Vector3.zero;

                // 플레이어가 가까이 있을 때는 멈춤
                agent.ResetPath();*/
            }
        }
    }

    public void NoPath()
    {
        // 적이 플레이어 근처에 있을 때 가해지던 힘 제거
        agent.velocity = Vector3.zero;

        // 플레이어가 가까이 있을 때는 멈춤
        agent.ResetPath();
    }

    protected override void Update_Attack()
    {

    }

    protected override void Update_Die()
    {
        base.Update_Die();
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("바닥과 충돌");
        if (collision.gameObject.CompareTag("Ground"))
        {
            Jump();
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))     // 플레이어가 트리거 영역으로 들어오면
        {
            StopCoroutine(enemy_Child.Timer());      // Timer 코루틴 정지
            if (!IsLowering)
            {
                if (lowerTrap != null)
                {
                    StopCoroutine(lowerTrap);
                }
                if (!enemy_Child.IsGrounded())                  // 땅이 아니면
                {
                    lowerTrap = StartCoroutine(LowerTrap());    // 땅으로 떨어지기
                }
                else
                {
                    StopCoroutine(lowerTrap);
                }
            }
            State = EnemyState.Chase;

            StartCoroutine(enemy_Child.Timer());
        }
    }

    IEnumerator LowerTrap()
    {
        IsLowering = true;
        float currentSpeed = loweringSpeed;

        while (childEnemy.position.y > trapLowerPosition)
        {
            currentSpeed += gravityAcceleration * Time.deltaTime;
            float newY = childEnemy.position.y - currentSpeed * Time.deltaTime;
            childEnemy.position = new Vector3(childEnemy.position.x, Mathf.Max(newY, trapLowerPosition), childEnemy.position.z);
            yield return null;
        }
    }

    /*/// <summary>
    /// 플레이어를 일정 시간동안 잡지 못했을 경우 실행될 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator NotCatch()
    {
        yield return new WaitForSeconds(notCatchTime);
        NoPath();
        State = EnemyState.Stop;
    }*/

    public void StateStop()
    {
        State = EnemyState.Stop;
    }
}
