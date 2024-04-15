using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    /// <summary>
    /// 항상 향하게 할 카메라의 트랜스폼
    /// </summary>
    public CinemachineVirtualCamera playerVC;

    void Update()
    {
        // 카메라와의 상대 위치를 구해서(UI에서 카메라를 향하는 방향 벡터)
        Vector3 relativePos = playerVC.transform.position - transform.position;

        // 카메라의 forward 방향으로 텍스트를 회전
        transform.rotation = Quaternion.LookRotation(-relativePos, Vector3.up);
    }
}
