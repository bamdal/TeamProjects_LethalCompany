using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Factory : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetHardware(ItemCode.GasTank);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        
    }
}
