using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorOpenPoint : MonoBehaviour, IInteraction
{
    public Door door;

    public Action onRequest { get; set; }

    public void Interaction(GameObject target)
    {
        door.Interaction(target);
    }



    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            door.Interaction(collision.gameObject);
            Debug.Log(collision.gameObject);
        }
    }


}
