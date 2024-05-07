using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trap : MonoBehaviour
{
    TrapTrigger trapTrigger;
    float currentY;
    Coroutine raiseTrap = null;
    private void Awake()
    {
        trapTrigger = GetComponentInParent<TrapTrigger>();
        currentY = transform.position.y;
    }

    IEnumerator RaiseTrap()
    {
        while (transform.position.y < currentY)
        {
            float newY = transform.position.y + 1.0f * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, Mathf.Min(newY, currentY), transform.position.z);
            yield return null;
        }
        trapTrigger.IsLowering = false;
    }
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
