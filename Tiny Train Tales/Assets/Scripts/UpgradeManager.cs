using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI maxSpeedText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] float cost;
    [SerializeField] float addToMaxSpeed;
    [Space]
    [SerializeField] TextMeshProUGUI maxPassangersText;
    [SerializeField] float addToMaxPassangers;
    [Space]
    [SerializeField] float costIncrease;

    float maxSpeed;
    float maxPassangers;

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        maxSpeed = gameManager.GetMaxSpeed();
        maxSpeedText.text = "Current: " + maxSpeed.ToString() + " km/h";

        maxPassangers = gameManager.GetMaxPassangers();
        maxPassangersText.text = "Current: " + maxPassangers.ToString();
    }

    public void UpgradeMaxSpeed()
    {
        float coins = gameManager.GetCoins();
        if (coins < cost) { return; }

        gameManager.Buy(cost);
        cost *= costIncrease;
        cost = Mathf.Floor(cost);
        costText.text = cost.ToString();

        gameManager.AddToMaxSpeed(addToMaxSpeed);
    }
}
