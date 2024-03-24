using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store_Fin : MonoBehaviour
{
    void Start()
    {
        // 경로 설정
        //string folderPath = "Assets/KWS/Resources/ItemDB";
        string folderPath = "ItemDB";


        // 해당 폴더 내 모든 스크립터블 오브젝트 찾기
        ItemDB[] scriptableObjects = Resources.LoadAll<ItemDB>(folderPath);                
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 상대방 오브젝트가 있는지 확인
        if (collision.gameObject != null)
        {
            // 충돌한 상대방 오브젝트가 Hardware인지 확인 
            if (collision.gameObject.CompareTag("Hardware"))
            {
                // 해당 Hardware에 연결된 ItemDB 스크립터블 오브젝트 찾기
                string itemName = collision.gameObject.name;
                ItemDB itemDB = Resources.Load<ItemDB>($"ItemDB/{itemName}");

                if (itemDB != null)
                {
                    // ItemDB 정보 출력
                    Debug.Log($"충돌한 {itemDB.itemName} 무게: {itemDB.weight}");
                    Debug.Log($"충돌한 {itemDB.itemName} 가격: {itemDB.price}");
                }
                else
                {
                    Debug.LogWarning("Hardware 오브젝트에 ItemDB 스크립트가 연결되어 있지 않습니다.");
                }
            }
        }
    }
}
