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

    protected Action onEnemyStateUpdate;

    private void Update()
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
}
