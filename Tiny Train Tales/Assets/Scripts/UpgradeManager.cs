using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI maxSpeedText;
    [SerializeField] float addToMaxSpeed;
    [Space]
    [SerializeField] TextMeshProUGUI maxPassangersText;
    [SerializeField] float addToMaxPassangers;

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
        gameManager.AddToMaxSpeed(addToMaxSpeed);
    }
}
