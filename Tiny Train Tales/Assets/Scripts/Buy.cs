using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Buy : MonoBehaviour
{
    [SerializeField] float cost;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] float costIncrease;
    
    GameManager gameManager;
    float coins;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        coins = gameManager.GetCoins();
        costText.text = cost.ToString();
    }

    public void Buying()
    {
        if (coins < cost) { return; }
        gameManager.Buy(cost);
        cost *= costIncrease;
    }
}
