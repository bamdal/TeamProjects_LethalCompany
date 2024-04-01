using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_IntegrationScenes : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetItem();
    }
}
