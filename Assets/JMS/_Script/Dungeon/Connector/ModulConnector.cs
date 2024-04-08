using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulConnector : MonoBehaviour
{
    // tranform.forward가 반드시 다음 연결장소를 향하게 만들어야 한다
    // 향하는 방향은 동서남북 순서로 0,1,2,3값을 가지게 한다

    Vector3 connection;

    /// <summary>
    /// 연결지점이 바라보고 있는 방향벡터
    /// </summary>
    public Vector3 Connection => connection;

    private void Start()
    {
        connection = transform.forward;
    }
}
