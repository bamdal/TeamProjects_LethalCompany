using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBase : ItemBase
{
    public ToolData toolData;

    private void Awake()
    {
        gameObject.AddComponent<Light>();
    }
}
