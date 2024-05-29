using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour, IHealth, IBattler, IDuengenSpawn
{
    public enum EnemyState
    {
        Stop = 0,
        Patrol,
        Chase,
        Attack,
        Die,
    }

    EnemyState state = EnemyState.Stop;

    public EnemyState State
    {
        get => state;
        protected set
        {
            if(state != value)
            {
                state = value;
                switch (state)
                {
                    case EnemyState.Stop:
                        Debug.Log("정지 상태");
                        onEnemyStateChange?.Invoke(state);
                        onEnemyStateUpdate = Update_Stop;
                        break;
                    case EnemyState.Patrol:
                        Debug.Log("정찰 상태");
                        onEnemyStateChange?.Invoke(state);
                        onEnemyStateUpdate = Update_Patrol;
                        break;
                    case EnemyState.Chase:
                        Debug.Log("추적 상태");
                        onEnemyStateChange?.Invoke(state);
                        onEnemyStateUpdate = Update_Chase;
                        break;
                    case EnemyState.Attack:
                        Debug.Log("공격 상태");
                        onEnemyStateChange?.Invoke(state);
                        onEnemyStateUpdate = Update_Attack;
                        break;
                    case EnemyState.Die:
                        Debug.Log("사망 상태");
                        onDie?.Invoke();
                        onEnemyStateChange?.Invoke(state);
                        onEnemyStateUpdate = Update_Die;

                        break;
                }
            }
        }
    }

    public Action onDie;
    public Action<NavMeshAgent,int> onDebuffAttack;
    public Action onEnemyStateUpdate;
    public Action<EnemyState> onEnemyStateChange;

    public virtual float Hp { get; set; }

    /// <summary>
    /// 최대 스폰 가능한 마릿수
    /// </summary>
    public virtual int MaxSpawnCount { get; set; }

    /// <summary>
    /// 게임내에 1개의 개체가 스폰될 확률(0~1)
    /// </summary>
    public virtual float SpawnPercent { get; set; }

    protected virtual void Start()
    {
        onDebuffAttack += OnDebuff;
        onEnemyStateUpdate = Update_Stop;
    }

    protected virtual void Update()
    {
        onEnemyStateUpdate();
    }

    protected virtual void Update_Stop()
    {

    }
    
    protected virtual void Update_Patrol()
    {

    }

    protected virtual void Update_Chase()
    {
        
    }

    protected virtual void Update_Attack()
    {

    }
    
    protected virtual void Update_Die()
    {

    }


    public void Die()
    {
        State = EnemyState.Die;
    }

    public void Attack(IBattler target)
    {
        
    }

    public virtual void Defense(float attackPower)
    {
        
    }

    /// <summary>
    /// 적이 디버프 공격을 맞았을때 실행될 함수(잽건과 수류탄의 스턴용)
    /// </summary>
    public void OnDebuff(NavMeshAgent agent, int debufftime)
    {
        StartCoroutine(StunnedEnemy(agent, debufftime));
    }

    IEnumerator StunnedEnemy(NavMeshAgent agent, int debuffTime)
    {
        float temp = agent.speed;
        agent.speed = 0.0f;
        yield return new WaitForSeconds(debuffTime);
        agent.speed = temp;
    }


    /// IBattler와 IHealth에 각각 Hp, Die() || Attack(), Defense() 추가되어있어요. 적 만드실 때 추가해주세요.
}
