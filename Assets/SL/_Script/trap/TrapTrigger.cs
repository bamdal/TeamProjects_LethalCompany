using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrapTrigger : MonoBehaviour
{
    public float loweringSpeed = 1f; // 트랩이 내려가는 속도
    public float raisingSpeed = 1f; // 트랩이 올라가는 속도
    public float trapLowerPosition = -5f; // 트랩이 내려갈 위치 (y값)
    public float gravityAcceleration = 9.8f; // 중력 가속도
    float currentY;
    private bool isLowering = false;
    Transform trap;


    private void Awake()
    {
        trap = transform.GetChild(0);
        currentY = trap.transform.position.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isLowering)
                StartCoroutine(LowerTrap());
        }
    }

    IEnumerator LowerTrap()
    {
        isLowering = true;
        float currentSpeed = loweringSpeed;

        while (trap.position.y > trapLowerPosition)
        {
            currentSpeed += gravityAcceleration * Time.deltaTime;
            float newY = trap.position.y - currentSpeed * Time.deltaTime;
            trap.position = new Vector3(trap.position.x, Mathf.Max(newY, trapLowerPosition), trap.position.z);
            yield return null;
        }

        StartCoroutine(RaiseTrap());
    }

    IEnumerator RaiseTrap()
    {
        while (trap.position.y < currentY)
        {
            float newY = trap.position.y + raisingSpeed * Time.deltaTime;
            trap.position = new Vector3(trap.position.x, Mathf.Min(newY, currentY), trap.position.z);
            yield return null;
        }

        isLowering = false;
    }
}

