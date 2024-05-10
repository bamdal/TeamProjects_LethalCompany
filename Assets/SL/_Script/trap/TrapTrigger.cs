using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrapTrigger : MonoBehaviour, IDuengenSpawn
{
    public float loweringSpeed = 1f; // 트랩이 내려가는 속도
    public float raisingSpeed = 1f; // 트랩이 올라가는 속도
    public float trapLowerPosition = -5f; // 트랩이 내려갈 위치 (y값)
    public float gravityAcceleration = 9.8f; // 중력 가속도
    private bool isLowering = false;
    public Coroutine lowerTrap = null;
    public int maxSpawnCount = 5;
    public float spawnPercent = 0.5f;

    public bool IsLowering
    {
        get => isLowering;
        set
        {
            if(isLowering != value)
            {
                isLowering = value;
            }
        }
    }


    /// <summary>
    /// 최대 스폰 가능한 마릿수
    /// </summary>
    public int MaxSpawnCount { get => maxSpawnCount; set { } }

    /// <summary>
    /// 게임내에 1개의 개체가 스폰될 확률(0~1)
    /// </summary>
    public float SpawnPercent { get => spawnPercent; set { } }

    Transform trap;



    private void Awake()
    {
        trap = transform.GetChild(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!IsLowering)
            {
                if(lowerTrap != null)
                {
                    StopCoroutine(lowerTrap);
                }
                lowerTrap = StartCoroutine(LowerTrap());
            }
        }
    }

    IEnumerator LowerTrap()
    {
        IsLowering = true;
        float currentSpeed = loweringSpeed;

        while (trap.position.y > trapLowerPosition)
        {
            currentSpeed += gravityAcceleration * Time.deltaTime;
            float newY = trap.position.y - currentSpeed * Time.deltaTime;
            trap.position = new Vector3(trap.position.x, Mathf.Max(newY, trapLowerPosition), trap.position.z);
            yield return null;
        }
    }
    public void StopLowerTrap()
    {
        if (lowerTrap != null)
        {
            StopCoroutine(lowerTrap);
        }
    }
}

