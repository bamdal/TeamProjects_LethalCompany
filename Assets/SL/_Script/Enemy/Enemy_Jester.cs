using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Jester : EnemyBase
{
    Player player;
    private void Awake()
    {
        State = EnemyState.Patrol;
    }
    protected override void Start()
    {
        base.Start();
        player = GameManager.Instance.Player;
    }

    protected override void Update()
    {
        base.Update();
    }


}
