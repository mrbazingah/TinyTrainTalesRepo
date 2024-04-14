using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [Header("MaxSpeed")]
    [SerializeField] TextMeshProUGUI maxSpeedText;
    [SerializeField] TextMeshProUGUI maxSpeedCostText;
    [SerializeField] float maxSpeedCost;
    [SerializeField] float addToMaxSpeed;
    [Header("MaxPassangers")]
    [SerializeField] TextMeshProUGUI maxPassangersText;
    [SerializeField] TextMeshProUGUI maxPassangerCostText;
    [SerializeField] float maxPassangerCost;
    [SerializeField] float addToMaxPassangers;
    [Space]
    [SerializeField] float costIncrease;
    [SerializeField] GameObject upgradeMenu;

    float maxSpeed;
    float maxPassangers;

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        upgradeMenu.SetActive(false);

        if (PlayerPrefs.HasKey("MaxSpeedCost"))
        {
            maxSpeedCost = PlayerPrefs.GetFloat("MaxSpeedCost");
            maxSpeedCostText.text = maxSpeedCost.ToString();
        }
        else
        {
            PlayerPrefs.SetFloat("MaxSpeedCost", maxSpeedCost);
        }

        if (PlayerPrefs.HasKey("MaxPassangerCost"))
        {
            maxPassangerCost = PlayerPrefs.GetFloat("MaxPassangerCost");
            maxPassangerCostText.text = maxPassangerCost.ToString();
        }
        else
        {
            PlayerPrefs.SetFloat("MaxPassangerCost", maxPassangerCost);
        }
    }

    public void OpenUpgradeMenu()
    {
        upgradeMenu.SetActive(true);
    }

    public void CloseUpgradeMenu()
    {
        upgradeMenu.SetActive(false);
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
        if (coins < maxSpeedCost) { return; }

        gameManager.Buy(maxSpeedCost);
        maxSpeedCost *= costIncrease;
        maxSpeedCost = Mathf.Floor(maxSpeedCost);
        maxSpeedCostText.text = maxSpeedCost.ToString();

        PlayerPrefs.SetFloat("MaxSpeedCost", maxSpeedCost);
        gameManager?.AddToMaxSpeed(addToMaxSpeed);
    }

    public void UpgradeMaxPassangers()
    {
        float coins = gameManager.GetCoins();
        if (coins < maxPassangerCost) { return; }

        gameManager.Buy(maxPassangerCost);
        maxPassangerCost *= costIncrease;
        maxPassangerCost = Mathf.Floor(maxPassangerCost);
        maxPassangerCostText.text = maxPassangerCost.ToString();

        PlayerPrefs.SetFloat("MaxPassangerCost", maxPassangerCost);
        gameManager?.AddToMaxPassangers(addToMaxSpeed);
    }
}
