using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_DropItem : TestBase
{
    GameObject obj;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    private void Awake()
    {
        
        obj = FindObjectOfType<ItemBase>().gameObject;
        meshFilter = obj.GetComponent<MeshFilter>();
        meshRenderer = obj.GetComponent<MeshRenderer>();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {

    }
}
