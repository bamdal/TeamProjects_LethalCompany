using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_MiddeScene : TestBase
{
#if UNITY_EDITOR

    public DayMonitor dayMonitor;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.NextDay();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        GameManager.Instance.Money += 100;
    }



#endif

}
