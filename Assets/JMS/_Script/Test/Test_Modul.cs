using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Modul : TestBase
{
    public GameObject target;
    public Door Door;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Door.Test_OpenVector(target);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {


    }
}
