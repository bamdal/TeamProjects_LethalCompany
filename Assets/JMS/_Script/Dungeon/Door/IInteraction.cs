using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteraction
{

    /// <summary>
    /// 플레이어가 상호작용 가능한 물체에 상호작용키를 눌렀을때 일어날 반응
    /// </summary>
    /// <param name="target">상호작용한 플레이어의 게임오브젝트</param>
    void Interaction(GameObject target);

    Action onRequest { get; set; }
}
