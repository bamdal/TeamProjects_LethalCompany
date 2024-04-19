using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenPoint : MonoBehaviour, IInteraction
{
    public Door door;

    public Action request { get; set; }

    private void Awake()
    {
        
    }
    public void Interaction(GameObject target)
    {
        door.Interaction(target);
    }


}
