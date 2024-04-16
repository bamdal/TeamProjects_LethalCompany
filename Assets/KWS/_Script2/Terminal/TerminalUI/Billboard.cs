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

    //public Camera mainCamera;

    void Update()
    {
        //transform.LookAt(transform.position + mainCamera.transform.forward);
        transform.forward = playerVC.transform.forward;
    }
    /// 빌보드로 만들면 UI의 글자가 찢어지는 문제 수정 필요
}
