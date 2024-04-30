using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name);
            IBattler player = other.gameObject.GetComponent<IBattler>();
            player.Defense(99999999);
        }
    }
}
