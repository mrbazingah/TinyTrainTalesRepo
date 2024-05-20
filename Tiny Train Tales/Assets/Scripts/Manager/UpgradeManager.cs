using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [Header("MaxSpeed")]
    [SerializeField] TextMeshProUGUI maxSpeedText;
    [SerializeField] TextMeshProUGUI maxSpeedCostText;
    [SerializeField] float maxSpeedCost;
    [SerializeField] float addToMaxSpeed;
    [SerializeField] float maxSpeedCostIncrease;
    [Header("MaxPassangers")]
    [SerializeField] TextMeshProUGUI maxPassangersText;
    [SerializeField] TextMeshProUGUI maxPassangerCostText;
    [SerializeField] float maxPassangerCost;
    [SerializeField] float addToMaxPassangers;
    [SerializeField] float maxPassangerCostIncrease;
    [Header("Acceleration")]
    [SerializeField] TextMeshProUGUI accelerationText;
    [SerializeField] TextMeshProUGUI accelerationCostText;
    [SerializeField] float accelerationCost;
    [SerializeField] float addToAcceleration;
    [SerializeField] float accelerationCostIncrease;
    [Header("Profit")]
    [SerializeField] TextMeshProUGUI profitText;
    [SerializeField] TextMeshProUGUI proiftCostText;
    [SerializeField] float profitCost;
    [SerializeField] float addToProfit;
    [SerializeField] float profitCostIncrease;

    GameManager gameManager;
    Train train;
    CameraMovement cam;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        train = FindObjectOfType<Train>();
        cam = FindObjectOfType<CameraMovement>();
    }

    void Start()
    {
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

    void Update()
    {
        maxSpeedText.text = "Current: " + gameManager.GetMaxSpeed().ToString() + " km/h";
        maxPassangersText.text = "Current: " + gameManager.GetMaxPassangers().ToString();
        accelerationText.text = "Current: " + train.GetAcceleration().ToString();
        profitText.text = "Current: " + gameManager.GetProfit().ToString() + "X";
    }

    public void UpgradeMaxSpeed()
    {
        float coins = gameManager.GetCoins();
        if (coins < maxSpeedCost) { return; }

        gameManager.BuyWithCoins(maxSpeedCost);
        maxSpeedCost += maxSpeedCostIncrease;
        maxSpeedCost = Mathf.Floor(maxSpeedCost);
        maxSpeedCostText.text = maxSpeedCost.ToString();

        PlayerPrefs.SetFloat("MaxSpeedCost", maxSpeedCost);
        gameManager?.AddToMaxSpeed(addToMaxSpeed);
    }

    public void UpgradeMaxPassangers()
    {
        float coins = gameManager.GetCoins();
        if (coins < maxPassangerCost) { return; }


        gameManager.BuyWithCoins(maxPassangerCost);
        maxPassangerCost += maxPassangerCostIncrease;
        maxPassangerCost = Mathf.Floor(maxPassangerCost);
        maxPassangerCostText.text = maxPassangerCost.ToString();

        PlayerPrefs.SetFloat("MaxPassangerCost", maxPassangerCost);
        gameManager?.AddToMaxPassangers(addToMaxPassangers);
    }

    public void UpgradeAcceleration()
    {
        float coins = gameManager.GetCoins();
        if (coins < accelerationCost) { return; }

        gameManager.BuyWithCoins(accelerationCost);
        accelerationCost += accelerationCostIncrease;
        accelerationCost = Mathf.Floor(accelerationCost);
        accelerationCostText.text = accelerationCost.ToString();

        PlayerPrefs.SetFloat("AccelerationCost", accelerationCost);
        train?.AddToAcceleration(addToAcceleration);
    }

    public void UprgadeProfit()
    {
        float coins = gameManager.GetCoins();
        if (coins < profitCost) { return; }

        gameManager.BuyWithCoins(profitCost);
        profitCost += profitCostIncrease;
        profitCost = Mathf.Floor(profitCost);
        proiftCostText.text = profitCost.ToString();

        PlayerPrefs.SetFloat("ProfitCost", profitCost);
        gameManager?.AddToProfit(addToProfit);

        
    }
}
