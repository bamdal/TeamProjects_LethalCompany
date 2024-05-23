using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    /// <summary>
    /// 문의 상태를 구별하는 부울값 true면 문이 열림 false면 문이 닫힘
    /// </summary>
    bool open = false;

    ModulConnector connector;

    /// <summary>
    /// true면 forward방향으로 열기
    /// </summary>
    bool forwardOpen = true;

    public float doorSpeed = 10.0f;

    /// <summary>
    /// 문이 회전할 축
    /// </summary>
    Transform hinge;

    /// <summary>
    /// 문 여닫는 쿨타임
    /// </summary>
    public float coolTime = 1.5f;

    /// <summary>
    /// 문 여닫는걸 판단하는 함수(true = 닫힘, false = 열수있음)
    /// </summary>
    bool doorLock = false;

    private void Awake()
    {
        connector = GetComponent<ModulConnector>();
        hinge = transform.GetChild(0);
    }

    /// <summary>
    /// target이 문을 열고 닫게 하는 함수
    /// </summary>
    /// <param name="target">문을 연 객체</param>
    public void Interaction(GameObject target)
    {
        OpenVecter(target);
        TogggleOpenClose();
 
    }

    /// <summary>
    /// 문을 여는 함수
    /// </summary>
    void Open()
    {
        open = true;
        StartCoroutine(OpenDoor(forwardOpen));

    }

    IEnumerator OpenDoor(bool forwardOpen)
    {

        if (forwardOpen)
        {
            Quaternion forwardOpenDoor = Quaternion.LookRotation(-transform.right);
            // 90 -> 0
            while (Quaternion.Angle(hinge.rotation, forwardOpenDoor) > 0.1f)
            {
                hinge.rotation = Quaternion.Slerp(hinge.rotation, forwardOpenDoor, Time.deltaTime * doorSpeed);
                yield return null;
            }
            hinge.rotation = forwardOpenDoor;
        }
        else
        {
            Quaternion backwardOpenDoor = Quaternion.LookRotation(transform.right);
            // 90 -> 180
            while (Quaternion.Angle(hinge.rotation, backwardOpenDoor) > 0.1f)
            {
                hinge.rotation = Quaternion.Slerp(hinge.rotation, backwardOpenDoor, Time.deltaTime * doorSpeed);

                yield return null;
            }
            hinge.rotation = backwardOpenDoor;
        }

        yield return null;
    }

    /// <summary>
    /// 문을 닫는 함수
    /// </summary>
    void Close()
    {
        open = false;
        // y를 90으로
        StartCoroutine(CloseDoor());
    }

    IEnumerator CloseDoor()
    {

        Quaternion CloseDoor = Quaternion.LookRotation(transform.forward);

        while (Quaternion.Angle(hinge.rotation, CloseDoor) > 0.1f)
        {
            hinge.rotation = Quaternion.Lerp(hinge.rotation, CloseDoor, Time.deltaTime * doorSpeed);
            yield return null;
        }
        hinge.rotation = CloseDoor;
    }

    /// <summary>
    /// 문의 상태의 따라 문을 여닫는 토글 함수
    /// </summary>
    void TogggleOpenClose()
    {
        if (!doorLock)
        {
            doorLock = true;
            if (hinge != null)
            {
                StopAllCoroutines();
                if (open)
                {
                    Close();
                }
                else
                {
                    Open();
                }
            }
            StartCoroutine(OpenCoolTime());
        }


    }

    IEnumerator OpenCoolTime()
    {
        yield return new WaitForSeconds(coolTime);
        doorLock = false;
    }

    /// <summary>
    /// 문을 연 객체의 위치에 따라 문이 열리는 방향을 결정하는 함수
    /// </summary>
    /// <param name="target">문을 연 객체</param>
    void OpenVecter(GameObject target)
    {
        Vector3 targetDir = (connector.transform.position - target.transform.position).normalized;
        if (Vector3.Dot(targetDir, connector.transform.forward) > 0)
        {
            forwardOpen = true;
        }
        else
        {
            forwardOpen = false;
        }
        
    }
    public void EnemyDoorOpen(GameObject target)
    {
        if (!doorLock && !open)
        {


            doorLock = true;
            OpenVecter(target);
            StopAllCoroutines();
            Open();
            StartCoroutine(OpenCoolTime());
        }
        

    }

#if UNITY_EDITOR
    public void Test_OpenVector(GameObject target)
    {
        OpenVecter(target);
    }
#endif
}
