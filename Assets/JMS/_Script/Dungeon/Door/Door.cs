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

    public float doorSpeed = 520.0f;

    /// <summary>
    /// 문이 회전할 축
    /// </summary>
    Transform hinge;

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
        Debug.Log(forwardOpen);
        if (forwardOpen)
        {
            // 90 -> 0
            while (hinge.eulerAngles.y < -90.0f)
            {
                hinge.Rotate(Time.deltaTime * -doorSpeed * Vector3.up);
                yield return null;
            }
        }
        else
        {
            // 90 -> 180
            while (hinge.eulerAngles.y < 90.0f)
            {
                hinge.Rotate(Time.deltaTime * doorSpeed * Vector3.up);

                yield return null;
            }
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
        Quaternion targetRotation = Quaternion.Euler(hinge.eulerAngles.x, 0f, hinge.eulerAngles.z);

        while (Quaternion.Angle(hinge.rotation, targetRotation) > 0.1f)
        {
            hinge.rotation = Quaternion.Lerp(hinge.rotation, targetRotation, Time.deltaTime * 5f);
            yield return null;
        }
        hinge.rotation = targetRotation;
    }

    /// <summary>
    /// 문의 상태의 따라 문을 여닫는 토글 함수
    /// </summary>
    void TogggleOpenClose()
    {
        StopAllCoroutines();
        if(open)
        {
            Close();
        }
        else
        {
            Open();
        }
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
            Debug.Log("같은방향"); // 90 -> 0
            forwardOpen = true;
        }
        else
        {
            Debug.Log("다른방향");  // 90 -> 180
            forwardOpen = false;
        }
        
    }

#if UNITY_EDITOR
    public void Test_OpenVector(GameObject target)
    {
        OpenVecter(target);
    }
#endif
}
