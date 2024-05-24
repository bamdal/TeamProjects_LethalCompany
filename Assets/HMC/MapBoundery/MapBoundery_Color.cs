using UnityEngine;

public class MakeTransparent : MonoBehaviour
{
    void Start()
    {
        // Renderer를 가져옵니다.
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            // 기존의 Material을 복사하여 새로운 인스턴스를 생성합니다.
            Material material = new Material(renderer.material);

            // Material의 Render Mode를 Transparent로 설정합니다.
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            // Material의 색상을 가져와 알파 값을 0으로 설정합니다.
            Color color = material.color;
            color.a = 0f;
            material.color = color;

            // Renderer의 Material을 새로 만든 Material로 교체합니다.
            renderer.material = material;
        }
        else
        {
            Debug.LogError("Renderer를 찾을 수 없습니다. 이 스크립트는 Renderer가 있는 GameObject에 붙여야 합니다.");
        }
    }
}
