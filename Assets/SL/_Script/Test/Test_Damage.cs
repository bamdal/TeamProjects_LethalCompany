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



}
