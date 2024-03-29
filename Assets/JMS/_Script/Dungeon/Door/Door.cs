using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteraction
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

    public float doorSpeed = 180.0f;

    private void Awake()
    {
        connector = GetComponentInParent<ModulConnector>();
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
        if (forwardOpen)
        StartCoroutine(OpenDoor(forwardOpen));

    }

    IEnumerator OpenDoor(bool forwardOpen)
    {
        Debug.Log(forwardOpen);
        if (forwardOpen)
        {
            // 90 -> 0
            while (transform.rotation.y > 0)
            {
                transform.Rotate(Time.deltaTime * -doorSpeed * Vector3.forward);
                yield return null;
            }
        }
        else
        {
            // 90 -> 180
            while (transform.rotation.y < 180)
            {
                transform.Rotate(Time.deltaTime * doorSpeed * Vector3.forward);
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
        yield return null;
    }

    /// <summary>
    /// 문의 상태의 따라 문을 여닫는 토글 함수
    /// </summary>
    void TogggleOpenClose()
    {
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
