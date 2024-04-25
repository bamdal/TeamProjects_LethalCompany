using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestCode : TestBase
{
#if UNITY_EDITOR

    public float count;
    

    private void Start()
    {
        
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.Money = count;
    }

#endif
}
