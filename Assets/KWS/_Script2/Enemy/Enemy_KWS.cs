using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_KWS : EnemyBase
{
    protected GameObject player; // 플레이어 오브젝트를 저장할 변수
    private UnityEngine.AI.NavMeshAgent navMeshAgent; // 네비게이션 에이전트 변수

    public float moveSpeed = 1.0f;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player");
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
        // 플레이어가 없으면 리턴
        if (player == null)
        {
            return;
        }

        // 플레이어를 향해 회전
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0; // y축 회전 방지
        transform.rotation = Quaternion.LookRotation(direction);

        // 플레이어를 향해 이동
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    protected override void Update_Attack()
    {
        
    }

    protected override void Update_Die()
    {
        base.Update_Die();
    }

}
