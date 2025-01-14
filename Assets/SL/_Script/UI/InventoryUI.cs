using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Player player;
    Inventory inventory;

    Image[] itemImages = new Image[4];
    Image[] itemEdgeImages = new Image[4];


    public Color edgeRed = new Color(255, 0, 0, 1f);
    public Color edgeRedInvisible = new Color(255, 0, 0, 0.0f);
    public Image[] ItemImages
    {
        get => itemImages;
        set
        {
            if(itemImages != value)
            {
                itemImages = value;
            }
        }
    }

    public Image[] ItemEdgeImages
    {
        get => itemEdgeImages;
        set
        {
            if (itemEdgeImages != value)
            {
                itemEdgeImages = value;
            }
        }
    }


    private void Awake()
    {
        for (int i = 0; i < itemImages.Length; i++)
        {
            itemImages[i] = transform.GetChild(i).GetChild(0).GetComponent<Image>(); // 이미지 컴포넌트에 접근하는 코드 수정
        }
        for (int i = 0; i < itemImages.Length; i++)
        {
            itemEdgeImages[i] = transform.GetChild(i).GetComponent<Image>(); // 이미지 컴포넌트에 접근하는 코드 수정
            itemEdgeImages[i].color = edgeRedInvisible;
        }
        
        
    }
    void Start()
    {
        player = GameManager.Instance.Player;
        inventory = player.transform.GetChild(1).GetComponent<Inventory>();
        
        // inventory.ItemDBs가 null이 아닌 경우에만 itemImages의 스프라이트 설정
        if (inventory != null && inventory.ItemDBs != null)
        {
            for (int i = 0; i < itemImages.Length; i++)
            {
                if (inventory.ItemDBs[i] != null) // null 체크 추가
                {
                    itemImages[i].sprite = inventory.ItemDBs[i].itemIcon;
                }
            }
        }
        ItemEdgeImages[0].color = edgeRed;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
