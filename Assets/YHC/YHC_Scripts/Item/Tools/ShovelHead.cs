using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelHead : MonoBehaviour
{
    public Action<Collider> onShovelTiggerOn;
    public Action<Collider> onShovelTiggerOff;

    private void OnTriggerEnter(Collider other)
    {
        onShovelTiggerOn?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        onShovelTiggerOff?.Invoke(other);
    }
}
