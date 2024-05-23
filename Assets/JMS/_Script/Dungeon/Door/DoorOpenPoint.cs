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




    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Enemy") && Vector3.Magnitude(transform.position - other.gameObject.transform.position) < 2)
        {

            door.EnemyDoorOpen(other.gameObject);
            Debug.Log(other.gameObject);
        }
    }


}
