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
    [Header("Acceleration")]
    [SerializeField] TextMeshProUGUI accelerationText;
    [SerializeField] TextMeshProUGUI accelerationCostText;
    [SerializeField] float accelerationCost;
    [SerializeField] float addToAcceleration;
    [Header("Profit")]
    [SerializeField] TextMeshProUGUI profitText;
    [SerializeField] TextMeshProUGUI proiftCostText;
    [SerializeField] float profitCost;
    [SerializeField] float addToProfit;
    [Space]
    [SerializeField] float costIncrease;
    [SerializeField] GameObject upgradeMenu;

    float maxSpeed;
    float maxPassangers;
    float acceleration;

    GameManager gameManager;
    Train train;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        train = FindObjectOfType<Train>();
    }

    void Start()
    {
        upgradeMenu.SetActive(false);

        if (PlayerPrefs.HasKey("MaxSpeedCost"))
        {
            maxSpeedCost = PlayerPrefs.GetFloat("MaxSpeedCost");
        }
        if (PlayerPrefs.HasKey("MaxPassangerCost"))
        {
            maxPassangerCost = PlayerPrefs.GetFloat("MaxPassangerCost");
        }
        if (PlayerPrefs.HasKey("AccelerationCost"))
        {
            accelerationCost = PlayerPrefs.GetFloat("AccelerationCost");
        }
        if (PlayerPrefs.HasKey("ProfitCost"))
        {
            profitCost = PlayerPrefs.GetFloat("ProfitCost");
        }

        accelerationCostText.text = accelerationCost.ToString();
        maxSpeedCostText.text = maxSpeedCost.ToString();
        maxPassangerCostText.text = maxPassangerCost.ToString();
        proiftCostText.text = profitCost.ToString();
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

        acceleration = train.GetAcceleration();
        accelerationText.text = "Current: " + acceleration.ToString();
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
        gameManager?.AddToMaxPassangers(addToMaxPassangers);
    }

    public void UpgradeAcceleration()
    {
        float coins = gameManager.GetCoins();
        if (coins < accelerationCost) { return; }

        gameManager.Buy(accelerationCost);
        accelerationCost *= costIncrease;
        accelerationCost = Mathf.Floor(accelerationCost);
        accelerationCostText.text = accelerationCost.ToString();

        PlayerPrefs.SetFloat("AccelerationCost", accelerationCost);
        train?.AddToAcceleration(addToAcceleration);
    }

    public void UprgadeProfit()
    {
        float coins = gameManager.GetCoins();
        if (coins < profitCost) { return; }

        gameManager.Buy(profitCost);
        profitCost *= costIncrease;
        profitCost = Mathf.Floor(profitCost);
        proiftCostText.text = profitCost.ToString();

        PlayerPrefs.SetFloat("ProfitCost", profitCost);
        gameManager?.AddToProfit(addToProfit);
    }
}
