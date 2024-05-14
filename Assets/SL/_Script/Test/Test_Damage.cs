using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Damage : TestBase
{
    Player player;

    private void Start()
    {
        player = GameManager.Instance.Player;    
    }
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        player.OnTestDamage();
    }
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        player.Defense(100);
    }
    protected override void OnTest3(InputAction.CallbackContext context)
    {
        player.PlayerRefresh();
    }


}
