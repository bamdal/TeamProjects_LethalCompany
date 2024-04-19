using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    Player player;
    Slider staminaBar;

    private void Awake()
    {

        staminaBar = GetComponent<Slider>();
    }
    private void Start()
    {
        player = GameManager.Instance.Player;
        player.onStaminaChange += RefrashStamina;

    }

    private void RefrashStamina(float stamina)
    {
        staminaBar.value = stamina / player.maxStamina;
    }


    private void Update()
    {

    }
}
