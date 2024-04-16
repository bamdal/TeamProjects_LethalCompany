using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonInside : MonoBehaviour, IInteraction
{
    

    public void Interaction(GameObject target)
    {
        Debug.Log("누름");
        Transform tp = FindAnyObjectByType<GenerationPointNav>().transform;
        Player player = target.GetComponent<Player>();
        CharacterController c = player.GetComponent<CharacterController>();
        c.enabled = false;
        target.transform.position = tp.transform.position;
        c.enabled = true;
    }

    
}
