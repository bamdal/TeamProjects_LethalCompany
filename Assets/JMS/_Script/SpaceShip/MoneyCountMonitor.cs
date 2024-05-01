using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyCountMonitor : MonoBehaviour
{

    TextMeshPro MoneyText;

    private void Awake()
    {
        MoneyText = GetComponentInChildren<TextMeshPro>();
    }

    void Start()
    {
        GameManager.Instance.OnMoneyChange += OnMoneyChange;


    }

    private void OnMoneyChange(float money)
    {
        MoneyText.text = $"${money}";
    }


}
