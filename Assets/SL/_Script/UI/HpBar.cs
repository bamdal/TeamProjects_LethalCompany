using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    Player player;
    Slider hpBar;

    private void Awake()
    {
        hpBar = GetComponent<Slider>();
        
    }
    private void Start()
    {
        player = GameManager.Instance.Player;
        player.onHealthChange += RefrashHp;
    }

    private void RefrashHp(float hp)
    {
        hpBar.value = hp / player.maxHp;
    }


    private void Update()
    {
        
    }
}
