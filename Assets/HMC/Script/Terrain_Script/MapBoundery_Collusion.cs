using UnityEngine;

public class MapBoundary_Collision : MonoBehaviour
{
    // 오브젝트의 원래 알파값
    private float originalAlpha;
    // 충돌 후 변경될 알파값
    public float collisionAlpha = 0.5f;

    // 플레이어와 충돌 시 호출되는 함수
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트의 Renderer를 가져옵니다.
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            // Material을 가져옵니다.
            Material material = renderer.material;

            // Material의 색상을 가져와 알파 값을 변경합니다.
            Color color = material.color;
            color.a = collisionAlpha;
            material.color = color;
        }
    }

    // 플레이어가 오브젝트에서 벗어날 때 호출되는 함수
    private void OnCollisionExit(Collision collision)
    {
        // 충돌한 오브젝트의 Renderer를 가져옵니다.
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            // Material을 가져옵니다.
            Material material = renderer.material;

            // Material의 색상을 가져와 알파 값을 원래 알파 값으로 변경합니다.
            Color color = material.color;
            color.a = originalAlpha;
            material.color = color;
        }
    }
}
