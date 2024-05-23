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
            if (other.gameObject.CompareTag("Player"))
            {
                Vector3 closestPoint = other.ClosestPoint(transform.position);
                float distance = Vector3.Distance(closestPoint, transform.position);
                BoxCollider playerCollider = other.GetComponent<BoxCollider>();
                if (playerCollider != null)
                {
                    float halfDiagonal = playerCollider.size.magnitude / 2;
                    if (distance <= halfDiagonal)
                    {
                    ChangeTransparency(0.8f); // 충돌 지점을 기준으로 투명도 변경 (더 불투명하게 설정)
                    }
                }
            Debug.Log("player touchde collider ");
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            GetComponent<Renderer>().material.color = originalColor;  //player가 collier에서 떨어지면 원래 색상으로 돌아감.
            Debug.Log("Player fell off collider");
        }
    }
    private void ChangeTransparency(float transparency)
    {
        Color newColor = originalColor;
        newColor.a = Mathf.Clamp(transparency, 0f,1f);
        GetComponent<Renderer>().material.color = newColor;

    }
    
}
