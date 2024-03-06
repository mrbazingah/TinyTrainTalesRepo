using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI maxSpeedText;
    [Space]
    [SerializeField] TextMeshProUGUI maxPassangersText;

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
}
