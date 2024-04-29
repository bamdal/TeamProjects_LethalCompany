using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class ItemPanelController : MonoBehaviour
{
    ItemRader itemRader;
    List<GameObject> itemPanels = new List<GameObject>(); // 생성된 모든 아이템 패널을 추적하기 위한 리스트
    public GameObject itemPanelPrefab;
    Player player;
    Camera mainCamera;
    RectTransform canvasRectTransform;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Start()
    {
        Transform child = GameManager.Instance.Player.transform.GetChild(2);
        itemRader = child.GetComponent<ItemRader>();
        itemRader.onItemView += OnItemViewPanel;
        player = GameManager.Instance.Player;
        player.onRclickIsNotPressed += OnItemViewPanelDelete;

        // 캔버스를 찾아서 변수에 저장
        canvasRectTransform = GetComponent<RectTransform>();
       
    }


    private void OnItemViewPanelDelete()
    {
        Debug.Log("onItemViewPanelDelete");
        // 생성된 모든 아이템 패널을 제거
        foreach (GameObject panel in itemPanels)
        {
            Destroy(panel);
        }
        // 리스트 초기화
        itemPanels.Clear();
    }

    private void OnItemViewPanel(Queue<Transform> itemQueue)
    {
        Debug.Log("onItemViewPanel");
        // 이전에 생성된 아이템 패널들을 제거
        //OnItemViewPanelDelete();

        // 아이템 큐를 순회하면서 아이템 패널을 생성하고 정보를 설정
        while (itemQueue.Count > 0)
        {
            Transform temp = itemQueue.Dequeue();

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, mainCamera.WorldToScreenPoint(temp.position), null, out localPoint);

            // 캔버스 좌표로 변환된 localPoint를 사용하여 새로운 아이템 패널의 anchoredPosition 설정
            GameObject newItemPanel = Instantiate(itemPanelPrefab, Vector3.zero, Quaternion.identity, transform); // 캔버스를 부모로 설정
            RectTransform newItemPanelRectTransform = newItemPanel.GetComponent<RectTransform>();
            newItemPanelRectTransform.anchoredPosition = localPoint;

            IItemDataBase tempItemDataBase = temp.GetComponent<IItemDataBase>();
            //GameObject newItemPanel = Instantiate(itemPanelPrefab, localPoint, Quaternion.identity, transform); // 캔버스를 부모로 설정
            Debug.Log(newItemPanel + "생성됨");
            TextMeshProUGUI nameText = newItemPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = newItemPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            nameText.text = tempItemDataBase.GetItemDB().itemName;
            priceText.text = tempItemDataBase.GetItemDB().price.ToString();

            // 생성된 아이템 패널을 리스트에 추가
            itemPanels.Add(newItemPanel);
        }
    }
}
