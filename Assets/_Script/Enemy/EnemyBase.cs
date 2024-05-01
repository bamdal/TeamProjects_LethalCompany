using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IHealth, IBattler
{
    public float stunnedTime = 5.0f;

    public enum EnemyState
    {
        Stop = 0,
        Patrol,
        Chase,
        Attack,
        Die,
    }

    EnemyState state = EnemyState.Patrol;

    protected EnemyState State
    {
        get => state;
        set
        {
            if(state != value)
            {
                state = value;
                switch (state)
                {
                    case EnemyState.Stop:
                        Debug.Log("정지 상태");
                        onEnemyStateUpdate = Update_Stop;
                        break;
                    case EnemyState.Patrol:
                        Debug.Log("정찰 상태");
                        onEnemyStateUpdate = Update_Patrol;
                        break;
                    case EnemyState.Chase:
                        Debug.Log("추적 상태");
                        onEnemyStateUpdate = Update_Chase;
                        break;
                    case EnemyState.Attack:
                        Debug.Log("공격 상태");
                        onEnemyStateUpdate = Update_Attack;
                        break;
                    case EnemyState.Die:
                        Debug.Log("사망 상태");
                        onDie?.Invoke();
                        onEnemyStateUpdate = Update_Die;

                        break;
                }
            }
        }
    }

    public Action onDie;
    public Action onDebuffAttack;

    public float Hp { get; set; }

    protected Action onEnemyStateUpdate;

    protected virtual void Start()
    {
        onDebuffAttack += OnDebuff;
    }

    protected virtual void Update()
    {
        // onEnemyStateUpdate();
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

    public void Defense(float attackPower)
    {
        
    }

    /// <summary>
    /// 적이 디버프 공격을 맞았을때 실행될 함수(잽건과 수류탄의 스턴용)
    /// </summary>
    public void OnDebuff()
    {
        StartCoroutine(StunnedEnemy());
    }

    IEnumerator StunnedEnemy()
    {
        State = EnemyState.Stop;
        yield return new WaitForSeconds(stunnedTime);
        State = EnemyState.Patrol;
    }


    /// IBattler와 IHealth에 각각 Hp, Die() || Attack(), Defense() 추가되어있어요. 적 만드실 때 추가해주세요.
}
