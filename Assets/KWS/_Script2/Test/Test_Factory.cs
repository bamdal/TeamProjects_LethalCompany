using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Factory : TestBase
{
    //Vector3 position = new Vector3(5.0f, 0.0f, 5.0f);
    
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        //Factory.Instance.GetHardware(ItemCode.Barrel, new Vector3(0f,2f,0f));
        Factory.Instance.GetHardware(ItemCode.Barrel);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Factory.Instance.GetHardware(ItemCode.CableDrum);        
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Factory.Instance.GetHardware(ItemCode.GarbageCart);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        Factory.Instance.GetHardware(ItemCode.GasTank);
    }
    protected override void OnTest5(InputAction.CallbackContext context)
    {
        Factory.Instance.GetHardware(ItemCode.PalletJack);
    }
}
