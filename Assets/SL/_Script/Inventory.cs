using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{   
    Player player;
    Transform[] invenSlots = new Transform[4];
    public Transform[] InvenSlots
    {
        get => invenSlots;
        set
        {
            if(invenSlots != value)
            {
                invenSlots = value;
            }
        }
    }

    private void Awake()
    {
        player = GameManager.Instance.Player;
        for (int i = 0; i < 4; i++)
        {
            InvenSlots[i] = transform.GetChild(i);
        }
    }

    private void Update()
    {

    }

    public Transform FindActiveObject(Transform[] objects)
    {
        Transform result = null;
        foreach (Transform obj in objects)
        {
            if (obj.childCount > 0 && obj.GetChild(0).gameObject.activeSelf)
            {
                result = obj.GetChild(0);
                break; // 활성화된 오브젝트를 찾았으므로 반복문을 종료합니다.
            }
        }
        return result;
    }

    public int FindIvenIndex(Transform[] objects)
    {
        int result = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].childCount > 0 && objects[i].GetChild(0).gameObject.activeSelf)
            {
                // 활성화된 아이템이 발견되었으므로 해당 인덱스를 반환합니다.
                result = i;
            }
            else
            {
                result = 5;
            }
        }
        return result;
    }
}
