using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Jester : EnemyBase
{
    Player player;
    public float patrolRange = 10f; // 배회 범위
    public float patrolTime = 5f;   // 배회 시간
    public float changeModeTime = 5f;
    public float attackPower = 9999f;
    public float popingSpeed = 10.0f;

    private Vector3 walkPoint;      // 다음 이동 지점
    private NavMeshAgent agent;
    private float timer;
    private float changeTimer;
    float originSpeed;

    bool isPlayerDetected = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        SetNewRandomDestination();
        timer = patrolTime;
        changeTimer = changeModeTime;
        originSpeed = agent.speed;
    }
    protected override void Start()
    {
        player = GameManager.Instance.Player;
        Debug.Log(player);
        State = EnemyState.Patrol;
        base.Start();
    }

    protected override void Update()
    {
        
        if(player != null)
        {
            if (player.IsInDungeon && State != EnemyState.Attack)
            {
                State = EnemyState.Chase;
                if(isPlayerDetected && State == EnemyState.Chase)
                {
                    State = EnemyState.Stop;
                    Debug.Log("찾음");
                }
            }
            else if(!player.IsInDungeon)
            {
                agent.speed = originSpeed;
                isPlayerDetected = false;
                State = EnemyState.Patrol;
            }
            else
            {
                State = EnemyState.Attack;
            }

        }

        base.Update();
        
    }
    protected override void Update_Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                SetNewRandomDestination();
                timer = patrolTime;
            }
        }
    }
    protected override void Update_Chase()
    {
        agent.SetDestination(player.transform.position);
    }
    protected override void Update_Stop()
    {
        agent.velocity = Vector3.zero;
        changeTimer -= Time.deltaTime;
        if(changeTimer <= 0f)
        {
            agent.speed = popingSpeed;
            changeTimer = changeModeTime;
            State = EnemyState.Attack;
        }
    }

    protected override void Update_Attack()
    {
        agent.SetDestination(player.transform.position);
        transform.LookAt(player.transform.position);
    }
    void SetNewRandomDestination()
    {
        // 현재 위치를 기준으로 무작위한 지점을 설정
        Vector3 randomDirection = Random.insideUnitSphere * patrolRange;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, patrolRange, 1);
        walkPoint = hit.position;

        // 설정한 지점으로 이동
        agent.SetDestination(walkPoint);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerDetected = true;
            
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && State == EnemyState.Attack)
        {
            player.Defense(attackPower);
        }
    }
}
