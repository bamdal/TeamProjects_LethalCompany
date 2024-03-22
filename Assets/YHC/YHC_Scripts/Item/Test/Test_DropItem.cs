using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_DropItem : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        int i = 0;
        Factory.Instance.GetItem();
    }
}
