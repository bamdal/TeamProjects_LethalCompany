using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_KWS : EnemyBase
{
    protected GameObject player; // 플레이어 오브젝트를 저장할 변수    
    private NavMeshAgent agent;
    Rigidbody rigid;

    [Range(1f, 5f)]
    public float moveSpeed = 1.0f;

    [Range(1f, 5f)]
    public float jumpHeight = 1.0f;

    [Range(1f, 10f)]
    public float stopDistance = 1.0f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed; // 이동 속도 설정
        player = GameObject.FindWithTag("Player");
        agent.stoppingDistance = stopDistance;
    }

    protected override void Update()
    {
        base.Update();
        //Update_Chase();
    }

    private void FixedUpdate()
    {
        Update_Chase();
    }

    protected override void Update_Stop()
    {
        
    }

    protected override void Update_Patrol()
    {
        
    }

    protected override void Update_Chase()
    {
        if (player != null)
        {
            // 플레이어를 향해 회전
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0; // y축 회전 방지
            transform.rotation = Quaternion.LookRotation(direction);

            // 플레이어와의 거리 계산
            float distance = Vector3.Distance(transform.position, player.transform.position);

            //Jump();

            // 플레이어와의 거리가 일정 범위 이상이면 이동
            if (distance > agent.stoppingDistance)
            {
                // 플레이어를 향해 이동
                agent.SetDestination(player.transform.position);
            }
            else
            {
                // 적이 플레이어 근처에 있을 때 가해지던 힘 제거
                agent.velocity = Vector3.zero;

                // 플레이어가 가까이 있을 때는 멈춤
                agent.ResetPath();

                // 적이 플레이어 근처에 있을 때 점프                
            }
        }
    }

    private void Jump()
    {
        // 점프할 방향과 힘 설정
        Vector3 jumpDirection = Vector3.up * jumpHeight;

        // Rigidbody에 힘을 가해서 점프시킴
        rigid.AddForce(jumpDirection, ForceMode.Impulse);
    }

    protected override void Update_Attack()
    {
        
    }

    protected override void Update_Die()
    {
        base.Update_Die();
    }

}
