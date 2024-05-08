using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Jester : EnemyBase
{
    Player player;
    public float patrolRange = 10f; // 배회 범위
    public float patrolTime = 5f;   // 배회 시간

    private Vector3 walkPoint;      // 다음 이동 지점
    private NavMeshAgent agent;
    private float timer;

    bool isPlayerDetected = false;

    private void Awake()
    {
        State = EnemyState.Patrol;
        agent = GetComponent<NavMeshAgent>();
        
    }
    protected override void Start()
    {
        base.Start();
        player = GameManager.Instance.Player;
    }

    protected override void Update()
    {
        if(isPlayerDetected)
        {
            State = EnemyState.Chase;
        }
        else
        {
            State = EnemyState.Patrol;
        }
        base.Update();
        
    }
    protected override void Update_Patrol()
    {
        // 이동 지점에 도달하거나 타이머가 종료된 경우 새로운 이동 지점을 설정
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isPlayerDetected = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isPlayerDetected = false;
        }
    }

}
