using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Buy : MonoBehaviour
{
    [SerializeField] int cost;
    [SerializeField] TextMeshProUGUI costText;
    [Space]
    [SerializeField] int amount;
    [SerializeField] TextMeshProUGUI amountText;
    
    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        costText.text = cost.ToString();
        amountText.text = amount.ToString();
    }

    public void Buying()
    {
        gameManager.Buy(cost);
    }
}
