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
        GameManager.Instance.onMoneyChange += OnMoneyChange;
        GameManager.Instance.onTargetAmountMoneyChange += OnTargetAmountMoneyChange;

    }

    private void OnTargetAmountMoneyChange(float Target)
    {
        int money = (int)GameManager.Instance.TotalMoney;
        MoneyText.text = $"${(int)Target}\n/ ${money}";
    }

    private void OnMoneyChange(float _)
    {
        int TargetMoney = (int)GameManager.Instance.TargetAmountMoney;
        int money = (int)GameManager.Instance.TotalMoney;
        MoneyText.text = $"${TargetMoney}\n/ ${(int)money}";
    }


}
