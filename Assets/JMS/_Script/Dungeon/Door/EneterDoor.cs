using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EneterDoor : MonoBehaviour, IInteraction
{
    public Action onRequest { get ; set ; }

    /// <summary>
    /// 텔레포트시 이동할 장소
    /// </summary>
    Transform spawnPoint;

    private void Awake()
    {
        spawnPoint = transform.GetChild(0);
    }

    public void Interaction(GameObject target)
    {
        Debug.Log("누름");
        DungeonInside tp = FindAnyObjectByType<DungeonInside>();
        Player player = target.GetComponent<Player>();
        player.IsInDungeon = true;
        CharacterController c = player.GetComponent<CharacterController>();
        c.enabled = false;
        target.transform.position = tp.TPPosition().position;
        c.enabled = true;
    }

    /// <summary>
    /// 이 문에 순간이동 시켜줄 좌표
    /// </summary>
    /// <returns></returns>
    public Transform TPPosition()
    {
        return spawnPoint;
    }


}
