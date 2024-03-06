using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI maxSpeedText;

    float maxSpeed;

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        maxSpeed = gameManager.GetMaxSpeed();

        string text = "Current: " + maxSpeed.ToString();
        maxSpeedText.text = text;
    }
}
