using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IHealth, IBattler
{
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
                        onEnemyStateUpdate = Update_Stop;
                        break;
                    case EnemyState.Patrol:
                        onEnemyStateUpdate = Update_Patrol;
                        break;
                    case EnemyState.Chase:
                        onEnemyStateUpdate = Update_Chase;
                        break;
                    case EnemyState.Attack:
                        onEnemyStateUpdate = Update_Attack;
                        break;
                    case EnemyState.Die:
                        onEnemyStateUpdate = Update_Die;
                        break;
                }
            }
        }
    }

    public float Hp { get; set; }

    protected Action onEnemyStateUpdate;

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
    }

    public void Attack(IBattler target)
    {
    }

    public void Defense(float attackPower)
    {
    }


    /// IBattler와 IHealth에 각각 Hp, Die() || Attack(), Defense() 추가되어있어요. 적 만드실 때 추가해주세요.
}
