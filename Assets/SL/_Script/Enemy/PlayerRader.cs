using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRader : MonoBehaviour
{
    public Action<bool> findPlayer;
    public float raderRange = 10.0f;
    public float chaseRange = 15.0f;
    SphereCollider playerRaderCollider;

    
    private void Awake()
    {
        playerRaderCollider = GetComponent<SphereCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            findPlayer?.Invoke(true);
            playerRaderCollider.radius = chaseRange;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            findPlayer?.Invoke(false);
            playerRaderCollider.radius = raderRange;
        }
    }
}
