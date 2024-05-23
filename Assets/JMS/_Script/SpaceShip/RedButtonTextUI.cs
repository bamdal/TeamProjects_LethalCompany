using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RedButtonTextUI : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshPro text;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            text.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            text.enabled = false;
        }
    }
}
