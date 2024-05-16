using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDie : MonoBehaviour
{
    Image image;
    Player player;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
        player.onDie += OnPlayerDie;
    }
    public void OnPlayerDie()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeAlpha());
    }

    private void OnDestroy()
    {
        player.onDie -= OnPlayerDie;
    }

    IEnumerator ChangeAlpha()
    {
        float timer = 0;
        Color startColor = image.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);
        while (timer < 3.0f)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0.0f, 1.0f, timer / 3.0f);
            image.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
        image.color = targetColor;
    }
}
