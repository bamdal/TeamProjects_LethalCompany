using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hygrodere_ChaseArea : MonoBehaviour
{
    public Action<Collider> onChaseIn;
    public Action<Collider> onChaseOut;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onChaseIn?.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onChaseOut?.Invoke(other);
        }

    }
}
