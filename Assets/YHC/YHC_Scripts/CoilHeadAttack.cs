using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoilHeadAttack : MonoBehaviour
{

    public Action<IBattler> onAttackPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            IBattler bttler = other.GetComponent<IBattler>();
            onAttackPlayer?.Invoke(bttler);
        }
    }
}
