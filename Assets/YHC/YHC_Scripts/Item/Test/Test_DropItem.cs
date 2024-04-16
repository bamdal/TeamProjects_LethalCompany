using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class Test_DropItem : TestBase
{
    public Grenade grenade;
    public CoilHead coilHead;
    public Vector3 randPos;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        coilHead.TracePlayer(randPos);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        randPos = new Vector3(Random.Range(-30.0f, 30.0f), coilHead.transform.position.y, Random.Range(-30.0f, 30.0f));
        Debug.Log($"{randPos}");
    }
}
