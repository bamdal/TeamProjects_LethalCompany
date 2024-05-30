using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    TrapTrigger trapTrigger;
    float currentY;
    Coroutine raiseTrap = null;
    public LayerMask groundLayer;
    

    private void Awake()
    {
        trapTrigger = GetComponentInParent<TrapTrigger>();
        currentY = transform.position.y;
    }
    void Update()
    {
        if(IsGrounded())
        {
            trapTrigger.StopLowerTrap();
            if (raiseTrap != null)
            {
                StopCoroutine(raiseTrap);
            }
            raiseTrap = StartCoroutine(RaiseTrap());
        }
        //Debug.Log(IsGrounded());
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
            player.Defense(100);
        }
        else
        {
            StartCoroutine(RaiseTrap());
        }
    }
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1f, layerMask: groundLayer);
    }
}
