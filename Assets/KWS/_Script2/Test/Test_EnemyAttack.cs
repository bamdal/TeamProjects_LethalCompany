using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_EnemyAttack : TestBase
{
    public Enemy_Spider enemy;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        enemy.Hp -= 5;
        Debug.Log(enemy.Hp);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Instantiate(enemy);
    }
}
