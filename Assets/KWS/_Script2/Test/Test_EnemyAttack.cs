using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_EnemyAttack : TestBase
{
    public Enemy_Spider enemyPrefab;    // 적 프리팹
    private Enemy_Spider enemy;         // 생성된 적의 참조를 저장하는 변수

    protected override void OnTest4(InputAction.CallbackContext context)
    {        
        if (enemy != null && enemy.Hp > 0)
        {
            enemy.Hp -= 5;
            Debug.Log($"적의 HP : {enemy.Hp}");
        }
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        // 생성된 적이 없을 때만 새로운 적 생성
        if (enemy == null)
        {
            // Enemy_Spider를 Instantiate하여 생성된 적의 참조를 저장
            enemy = Instantiate(enemyPrefab);
        }
    }
}
