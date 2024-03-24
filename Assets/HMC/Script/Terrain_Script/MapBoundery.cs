using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBoundery : MonoBehaviour
{
    private Color originalColor;     //Collier를 가지고있는 object의 원래 색상 저장용 변수
    private void Start()
    {
        originalColor = GetComponent<Renderer>().material.color;  // obeject의 원래 색상 저장
    }
    private void OnTriggerEnter(Collider other)  //player의 이동범위를 제한 하는 콜라이더.
    {
        if(other.CompareTag("player"))           
        {
            CapsuleCollider playerCollider = other.GetComponent<CapsuleCollider>();
            if(playerCollider != null)
            { 
                float distance = Vector3.Distance(playerCollider.ClosestPoint(transform.position),transform.position);
                float radius = playerCollider.radius;   //플레이어와 오브젝트의 거리 계산.
                if(distance <= radius)
                {
                    ChangeTransparency(0.5f); //충돌 지점을 기준으로 투명도 변경.
                }
            }
            Debug.Log("player touchde collider ");
            
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("player"))
        {
            GetComponent<Renderer>().material.color = originalColor;  //player가 collier에서 떨어지면 원래 색상으로 돌아감.
            Debug.Log("Player fell off collider");
        }
    }
    private void ChangeTransparency(float transparency)
    {
        Color newColor = originalColor;
        newColor.a = transparency;
        GetComponent<Renderer>().material.color = newColor;

    }
    
}
