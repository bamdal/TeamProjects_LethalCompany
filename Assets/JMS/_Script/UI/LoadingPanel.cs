using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;
    TextMeshProUGUI difficultyText;


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        difficultyText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        difficultyText.text = $"난이도 : {GameManager.Instance.Difficulty}";
    }

    public void CanvasGroupAlphaChange()
    {
        canvasGroup.alpha = 0f;
    }
}
