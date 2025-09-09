using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [Header("Max Speed")]
    [SerializeField] TextMeshProUGUI maxSpeedText;
    [SerializeField] TextMeshProUGUI maxSpeedCostText;
    [SerializeField] float maxSpeedCost;
    [SerializeField] float addToMaxSpeed;
    [SerializeField] float maxSpeedCostIncrease;
    [Space]
    [SerializeField] TextMeshProUGUI maxSpeedUpgradeAmountText;
    [SerializeField] int maxSpeedUpgradeAmount;
    [Header("Max Passangers")]
    [SerializeField] TextMeshProUGUI maxPassangersText;
    [SerializeField] TextMeshProUGUI maxPassangerCostText;
    [SerializeField] float maxPassangerCost;
    [SerializeField] float addToMaxPassangers;
    [SerializeField] float maxPassangerCostIncrease;
    [Space]
    [SerializeField] TextMeshProUGUI maxPassangerUpgradeAmountText;
    [Header("Acceleration")]
    [SerializeField] TextMeshProUGUI accelerationText;
    [SerializeField] TextMeshProUGUI accelerationCostText;
    [SerializeField] float accelerationCost;
    [SerializeField] float addToAcceleration;
    [SerializeField] float accelerationCostIncrease;
    [Space]
    [SerializeField] TextMeshProUGUI maxAccelerationUpgradeAmountText;
    [SerializeField] int maxAccelerationUpgradeAmount;
    [Header("Profit")]
    [SerializeField] TextMeshProUGUI profitText;
    [SerializeField] TextMeshProUGUI profitCostText;
    [SerializeField] float profitCost;
    [SerializeField] float addToProfit;
    [SerializeField] float profitCostIncrease;
    [Space]
    [SerializeField] TextMeshProUGUI maxProfitUpgradeAmountText;
    [SerializeField] int maxProfitUpgradeAmount;
    [Header("Cars")]
    [SerializeField] TextMeshProUGUI carsText;
    [SerializeField] TextMeshProUGUI carsCostText;
    [SerializeField] float carsCost;
    [SerializeField] float carsCostIncrease;
    [Space]
    [SerializeField] TextMeshProUGUI maxCarUpgradeAmountText;
    [SerializeField] int maxCarUpgradeAmount;
    [Header("Colors")]
    [SerializeField] Color originalColor;
    [SerializeField] Color cantAffordColor;
    [SerializeField] Color doneColor;

    float coins;
    int amountOfCars = 1;

    int currentMaxSpeedAmount;
    int currentMaxPassangerAmount;
    int currentAccelerationAmount;
    int currentProfitAmount;
    int currentCarsAmount;

    ColorBlock accelerationColorBlock;
    ColorBlock maxSpeedColorBlock;
    ColorBlock maxPassangersColorBlock;
    ColorBlock profitColorBlock;
    ColorBlock carsColorBlock;

    GameManager gameManager;
    Train train;

    Button accelerationButton;
    Button maxSpeedButton;
    Button maxPassangersButton;
    Button profitButton;
    Button carsButton;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        train = FindObjectOfType<Train>();

        accelerationButton = accelerationCostText.GetComponentInParent<Button>();
        maxSpeedButton = maxSpeedCostText.GetComponentInParent<Button>();
        maxPassangersButton = maxPassangerCostText.GetComponentInParent<Button>();
        profitButton = profitCostText.GetComponentInParent<Button>();
        carsButton = carsCostText.GetComponentInParent<Button>();

        ChangeColor();
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
        if (PlayerPrefs.HasKey("CarsCost"))
        {
            carsCost = PlayerPrefs.GetFloat("CarsCost");
        }
        if (PlayerPrefs.HasKey("AmountOfCars"))
        {
            amountOfCars = PlayerPrefs.GetInt("AmountOfCars");
        }

        currentMaxSpeedAmount = PlayerPrefs.GetInt("CurrentMaxSpeedAmount");
        currentMaxPassangerAmount = PlayerPrefs.GetInt("CurrentMaxPassangerAmount");
        currentAccelerationAmount = PlayerPrefs.GetInt("CurrentAccelerationAmount");
        currentProfitAmount = PlayerPrefs.GetInt("CurrentProfitAmount");
        currentCarsAmount = PlayerPrefs.GetInt("CurrentCarsAmount");

        accelerationCostText.text = accelerationCost.ToString();
        maxSpeedCostText.text = maxSpeedCost.ToString();
        maxPassangerCostText.text = maxPassangerCost.ToString();
        profitCostText.text = profitCost.ToString();
        carsCostText.text = carsCost.ToString();
    }

    void Update()
    {
        UpdateText();
        ChangeColor();
    }

    void ChangeColor()
    {
        Color maxSpeedColor = originalColor;
        Color accelartionColor = originalColor;
        Color maxPassangersColor = originalColor;
        Color profitColor = originalColor;
        Color carsColor = originalColor;

        coins = gameManager.GetCoins();
        if (coins < maxSpeedCost )
        {
            maxSpeedColor = cantAffordColor;
        }
        if (coins < accelerationCost)
        {
            accelartionColor = cantAffordColor;
        }
        if (coins < profitCost)
        {
            profitColor = cantAffordColor;
        }
        if (coins < maxPassangerCost)
        {
            maxPassangersColor = cantAffordColor;
        }
        if (coins < carsCost)
        {
            carsColor = cantAffordColor;
        }

        accelerationColorBlock = accelerationButton.colors;
        accelerationColorBlock.normalColor = accelartionColor;
        accelerationColorBlock.highlightedColor = accelartionColor;
        accelerationColorBlock.selectedColor = accelartionColor;
        accelerationButton.colors = accelerationColorBlock;

        maxSpeedColorBlock = maxSpeedButton.colors;
        maxSpeedColorBlock.normalColor = maxSpeedColor;
        maxSpeedColorBlock.highlightedColor = maxSpeedColor;
        maxSpeedColorBlock.selectedColor = maxSpeedColor;
        maxSpeedButton.colors = maxSpeedColorBlock;

        maxPassangersColorBlock = maxPassangersButton.colors;
        maxPassangersColorBlock.normalColor = maxPassangersColor;
        maxPassangersColorBlock.highlightedColor = maxPassangersColor;
        maxPassangersColorBlock.selectedColor = maxPassangersColor;
        maxPassangersButton.colors = maxPassangersColorBlock;

        profitColorBlock = profitButton.colors;
        profitColorBlock.normalColor = profitColor;
        profitColorBlock.highlightedColor = profitColor;
        profitColorBlock.selectedColor = profitColor;
        profitButton.colors = profitColorBlock;

        carsColorBlock = carsButton.colors;
        carsColorBlock.normalColor = carsColor;
        carsColorBlock.highlightedColor = carsColor;
        carsColorBlock.selectedColor = carsColor;
        carsButton.colors = carsColorBlock;
    }

    void UpdateText()
    {
        maxSpeedText.text = "Current: " + gameManager.GetMaxSpeed().ToString() + " km/h";
        maxPassangersText.text = "Current: " + gameManager.GetMaxPassangers().ToString();
        accelerationText.text = "Current: " + (train.GetAcceleration() * 10).ToString();
        profitText.text = "Current: " + gameManager.GetProfit().ToString() + "X";
        carsText.text = "Current: " + amountOfCars.ToString();

        maxSpeedUpgradeAmountText.text = currentMaxSpeedAmount + "/" + maxSpeedUpgradeAmount;
        maxPassangerUpgradeAmountText.text = currentMaxPassangerAmount.ToString();
        maxAccelerationUpgradeAmountText.text = currentAccelerationAmount + "/" + maxAccelerationUpgradeAmount;
        maxProfitUpgradeAmountText.text = currentProfitAmount + "/" + maxProfitUpgradeAmount;
        maxCarUpgradeAmountText.text = currentCarsAmount + "/" + maxCarUpgradeAmount;
    }

    public void UpgradeMaxSpeed()
    {
        if (coins < maxSpeedCost || currentMaxSpeedAmount >= maxSpeedUpgradeAmount) { return; }

        gameManager.BuyWithCoins(maxSpeedCost);
        maxSpeedCost += maxSpeedCostIncrease;
        maxSpeedCost = Mathf.Floor(maxSpeedCost);
        maxSpeedCostText.text = maxSpeedCost.ToString();

        PlayerPrefs.SetFloat("MaxSpeedCost", maxSpeedCost);
        gameManager?.AddToMaxSpeed(addToMaxSpeed);

        currentMaxSpeedAmount++;
        PlayerPrefs.SetInt("CurrentMaxSpeedAmount", currentMaxSpeedAmount);
    }

    public void UpgradeMaxPassangers()
    {
        if (coins < maxPassangerCost) { return; }

        gameManager.BuyWithCoins(maxPassangerCost);
        maxPassangerCost += maxPassangerCostIncrease;
        maxPassangerCost = Mathf.Floor(maxPassangerCost);
        maxPassangerCostText.text = maxPassangerCost.ToString();

        PlayerPrefs.SetFloat("MaxPassangerCost", maxPassangerCost);
        gameManager?.AddToMaxPassangers(addToMaxPassangers);

        currentMaxPassangerAmount++;
        PlayerPrefs.SetInt("CurrentMaxPassangerAmount", currentMaxPassangerAmount);
    }

    public void UpgradeAcceleration()
    {
        if (coins < accelerationCost || currentAccelerationAmount >= maxAccelerationUpgradeAmount) { return; }

        gameManager.BuyWithCoins(accelerationCost);
        accelerationCost += accelerationCostIncrease;
        accelerationCost = Mathf.Floor(accelerationCost);
        accelerationCostText.text = accelerationCost.ToString();

        PlayerPrefs.SetFloat("AccelerationCost", accelerationCost);
        train?.AddToAcceleration(addToAcceleration);

        currentAccelerationAmount++;
        PlayerPrefs.SetInt("CurrentAccelerationAmount", currentAccelerationAmount);
    }

    public void UprgadeProfit()
    {
        if (coins < profitCost || currentProfitAmount >= maxProfitUpgradeAmount) { return; }

        gameManager.BuyWithCoins(profitCost);
        profitCost += profitCostIncrease;
        profitCost = Mathf.Floor(profitCost);
        profitCostText.text = profitCost.ToString();

        PlayerPrefs.SetFloat("ProfitCost", profitCost);
        gameManager?.AddToProfit(addToProfit);

        currentProfitAmount++;
        PlayerPrefs.SetInt("CurrentProfitAmount", currentProfitAmount);
    }

    public void UpgradeCars()
    {
        if (coins < carsCost || currentCarsAmount >= maxCarUpgradeAmount) { return; }

        gameManager.BuyWithCoins(carsCost);
        carsCost += carsCostIncrease;
        carsCost = Mathf.Floor(carsCost);
        carsCostText.text = carsCost.ToString();
        amountOfCars++;

        PlayerPrefs.SetFloat("CarsCost", carsCost);
        PlayerPrefs.SetInt("AmountOfCars", amountOfCars);

        currentCarsAmount++;
        PlayerPrefs.SetInt("CurrentCarsAmount", currentCarsAmount);
    }

    public int GetAmountOfCars()
    {
        return amountOfCars; 
    }

    public float GetAverageCost()
    {
        return (maxSpeedCost + carsCost + accelerationCost + profitCost + maxPassangerCost) / 5;
    }
}
