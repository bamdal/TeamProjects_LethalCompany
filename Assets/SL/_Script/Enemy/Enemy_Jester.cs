using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class 
    Enemy_Jester : EnemyBase
{
    Player player;
    public float patrolRange = 10f; // 배회 범위
    public float patrolTime = 5f;   // 배회 시간
    public float changeModeTime = 5f;
    public float attackPower = 9999f;
    public float popingSpeed = 10.0f;
    public float modeChangeProbability = 0.3f;
    public LayerMask playerLayerMask;
    public float maxSpeed = 8.0f;

    private Vector3 walkPoint;      // 다음 이동 지점
    private NavMeshAgent agent;
    private float timer;
    private float changeTimer;
    float originSpeed;
    private Transform layStartPosition;
    bool isPlayerDetected = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        SetNewRandomDestination();
        timer = patrolTime;
        changeTimer = changeModeTime;
        originSpeed = agent.speed;
        onEnemyStateUpdate = Update_Patrol;
        layStartPosition = transform.GetChild(0);
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

        if (player != null)
        {
            if (!player.IsInDungeon)
            {
                agent.speed = originSpeed;
                isPlayerDetected = false;
                State = EnemyState.Patrol;
            }
            else
            {
                if (State != EnemyState.Attack && State != EnemyState.Stop)
                {
                    if (isPlayerDetected)
                    {
                        State = EnemyState.Chase;
                    }
                    else
                    {
                        State = EnemyState.Patrol;
                    }
                }
            }

            base.Update();
        }
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
        RaycastHit hit;
        if (Physics.Raycast(layStartPosition.transform.position, (player.transform.position - transform.position).normalized, out hit, 5.0f, playerLayerMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                float randomValue = Random.value;
                if (randomValue <= modeChangeProbability)
                {
                    State = EnemyState.Stop;
                }
            }
        }
    }
    protected override void Update_Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        changeTimer -= Time.deltaTime;
        if (changeTimer <= 0f)
        {
            changeTimer = changeModeTime;
            State = EnemyState.Attack;
        }
    }

    protected override void Update_Attack()
    {
        agent.isStopped = false;
        agent.speed += popingSpeed * Time.deltaTime;
        if (agent.speed > maxSpeed)
        {
            agent.speed = maxSpeed;
        }
        agent.SetDestination(player.transform.position);
        transform.LookAt(player.transform.position);
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
