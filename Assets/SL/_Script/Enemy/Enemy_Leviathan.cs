using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Leviathan : EnemyBase
{
    Player player;
    public float attackInterval = 0.3f;
    public float patrolRange = 10f; // 배회 범위
    public float patrolTime = 5f;   // 배회 시간
    public float attackPower = 10.0f;
    public float popingSpeed = 10.0f;
    public int maxSpawnCount = 1;
    public float spawnPercent = 0.3f;

    public override int MaxSpawnCount { get => maxSpawnCount; set { } }
    public override float SpawnPercent { get => spawnPercent; set { } }


    private Vector3 walkPoint;      // 다음 이동 지점
    private NavMeshAgent agent;
    private float timer;
    bool isPlayerDetected = false;
    bool isAttacked = false;
    PlayerRader playerRader;
    AttackRange attackRange;
    Coroutine onDamage;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = patrolTime;
        onEnemyStateUpdate = Update_Patrol;
        playerRader = transform.GetComponentInChildren<PlayerRader>();
        playerRader.findPlayer += IsPlaeyrDetected;
        attackRange = transform.GetComponentInChildren<AttackRange>();
        attackRange.isAttack += IsAttack;

    }

    private void IsAttack(bool value)
    {
        isAttacked = value;
    }

    private void IsPlaeyrDetected(bool value)
    {
        isPlayerDetected = value;
    }

    protected override void Start()
    {
        player = GameManager.Instance.Player;
        SetNewRandomDestination();
        base.Start();
    }

    protected override void Update()
    {
        if (isPlayerDetected)
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
        Vector3 randomDirection = Random.insideUnitSphere * patrolRange;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, patrolRange, 1);
        walkPoint = hit.position;

        agent.SetDestination(walkPoint);
    }
    private void OnTriggerEnter(Collider other)
    {
        //몇초이상 있으면 레비아탄 튀어나오고 데미지
    }
}
