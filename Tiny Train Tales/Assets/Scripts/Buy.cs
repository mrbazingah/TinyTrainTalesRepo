using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Buy : MonoBehaviour
{
    [SerializeField] float cost;
    [SerializeField] float costIncrease;
    
    GameManager gameManager;
    float coins;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (PlayerPrefs.HasKey("MaxSpeedCost"))
        {
            cost = PlayerPrefs.GetFloat("MaxSpeedCost");
        }
    }

    void Update()
    {
        coins = gameManager.GetCoins();
    }

    public void Buying(TextMeshProUGUI costText)
    {
        if (coins < cost) { return; }
        gameManager.Buy(cost);
        cost *= costIncrease;
        cost = Mathf.Floor(cost);
        costText.text = cost.ToString();

        PlayerPrefs.SetFloat("MaxSpeedCost", cost);
    }
}
