using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_StartScene : TestBase
{
#if UNITY_EDITOR

    public float money = 50.0f;

    private void Start()
    {
        
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.NextDay();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        GameManager.Instance.Money += money;
    }



#endif
}
