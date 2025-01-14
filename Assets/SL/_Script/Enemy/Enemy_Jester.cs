using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy_Jester : EnemyBase
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
    public int maxSpawnCount = 1;
    public float spawnPercent = 0.3f;
    AudioSource jesterAudio;


    public override int MaxSpawnCount { get => maxSpawnCount; set { } }
    public override float SpawnPercent { get => spawnPercent; set { } }

    PlayerRader playerRader;

    private Vector3 walkPoint;      // 다음 이동 지점
    private NavMeshAgent agent;
    private float timer;
    private float changeTimer;
    float originSpeed;
    private Transform layStartPosition;
    bool isPlayerDetected = false;
    MeshRenderer[] meshRenderer;
    Animator animator;
    

    private void Awake()
    {
        jesterAudio = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        meshRenderer = new MeshRenderer[2];
        for(int i = 0; i < 2; i++)
        {
                meshRenderer[i] = transform.GetChild(1).transform.GetChild(i).GetComponent<MeshRenderer>();
            for(int j = 0; j < 2; j++)
            {
            }

        }
        
        timer = patrolTime;
        changeTimer = changeModeTime;
        originSpeed = agent.speed;
        onEnemyStateUpdate = Update_Patrol;
        layStartPosition = transform.GetChild(0);
        playerRader = transform.GetChild(3).GetComponent<PlayerRader>();
        playerRader.findPlayer += FindPlayer;
        animator = GetComponent<Animator>();
    }



    protected override void Start()
    {
        base.Start();
        player = GameManager.Instance.Player;
        Debug.Log(player);
        State = EnemyState.Patrol;

        SetNewRandomDestination();
    }

    protected override void Update()
    {

        if (player != null)
        {
            if (!player.IsInDungeon)
            {
                agent.speed = originSpeed;
                isPlayerDetected = false;
                
                transform.GetChild(2).gameObject.SetActive(false);
                changeTimer = changeModeTime;
                State = EnemyState.Patrol;
                playerRader.gameObject.GetComponent<Collider>().enabled = true;
                jesterAudio.Stop();
                animator.SetBool(nameof(Attack),false);
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
        agent.isStopped = false;
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
                    // 여기에 audio 시작 코드
                    jesterAudio.Play();
                }
            }
        }
    }
    protected override void Update_Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        changeTimer -= Time.deltaTime;
        float t = Mathf.Clamp01(1 - (changeTimer / changeModeTime));
        Color startColor = Color.white;
        Color endColor = Color.red;
        Color newColor = Color.Lerp(startColor, endColor, t);
        for (int i = 0; i < 2; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                meshRenderer[i].materials[j].color = newColor;
            }
        }
        playerRader.gameObject.GetComponent<Collider>().enabled = false;

        if (changeTimer <= 0f)
        {
            changeTimer = changeModeTime;
            animator.SetBool(nameof(Attack), true);
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    meshRenderer[i].materials[j].color = endColor;
                }
            }
            StartCoroutine(OpenChest());
        }
    }

    IEnumerator OpenChest()
    {

        yield return new WaitForSeconds(1.0f);
        transform.GetChild(2).gameObject.SetActive(true);
        State = EnemyState.Attack;
        jesterAudio.Stop();
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


    private void OnTriggerEnter(Collider collision)
    {
        /*if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerDetected = true;

        }*/
        if (collision.gameObject.CompareTag("Player") && State == EnemyState.Attack)
        {
            player.Defense(attackPower);
        }
    }
    private void FindPlayer(bool isPlayer)
    {
        isPlayerDetected = isPlayer;
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.CompareTag("Player") && State == EnemyState.Attack)
        {
            player.Defense(attackPower);
        }
    }*/
}
