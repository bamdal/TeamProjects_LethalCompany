using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_IntegrationScenes : TestBase
{
#if UNITY_EDITOR
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetItem();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        GameManager.Instance.Player.Defense(50.0f);
    }
#endif
}
