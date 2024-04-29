using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRader : MonoBehaviour
{
    Queue<Transform> itemTransforms = new Queue<Transform>();
    public Action<Queue<Transform>> onItemView;
    Player player;
    Queue<Transform> ItemTransforms
    {
        get => itemTransforms;
        set
        {
            if (itemTransforms != null)
            {
                itemTransforms = value;
            }
        }
    }
    private void Start()
    {
        player = GameManager.Instance.Player;
        player.onRclickIsNotPressed += ItemQueueClear;
    }

    private void ItemQueueClear()
    {
        ItemTransforms.Clear();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Item") || collision.CompareTag("Hardware"))
        {
            Transform itemTransform = collision.transform;
            Vector3 itemPosition = itemTransform.position;

            // 아이템의 위치와 플레이어의 위치 사이의 방향 벡터를 구합니다.
            Vector3 directionToItem = itemPosition - transform.position;

            // 플레이어 위치에서 아이템까지의 레이를 생성합니다.
            Ray ray = new Ray(transform.position + transform.forward * 0.5f, directionToItem);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 레이가 충돌한 객체가 벽인지 확인합니다.
                if (hit.collider.CompareTag("Obstacle"))
                {
                    // 벽 뒤에 있는 아이템을 제거합니다.
                    Debug.Log("벽 뒤에 있는 아이템: " + itemTransform.gameObject.name);
                }
                else if (hit.collider.CompareTag("Item") || collision.CompareTag("Hardware"))
                {
                    // 벽 뒤에 없는 아이템을 목록에 추가합니다.
                    ItemTransforms.Enqueue(itemTransform);
                }
            }
            else
            {
                // 레이가 아이템에 닿지 않은 경우 아이템을 목록에 추가합니다.
                ItemTransforms.Enqueue(itemTransform);
            }
        }
        onItemView?.Invoke(ItemTransforms);


    }
}
