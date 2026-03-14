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
    [Header("Max Cars")]
    [SerializeField] TextMeshProUGUI carsText;
    [SerializeField] TextMeshProUGUI carsCostText;
    [SerializeField] float carsCost;
    [SerializeField] float carsCostIncrease;
    [Space]
    [SerializeField] TextMeshProUGUI maxCarUpgradeAmountText;
    [SerializeField] int maxCarUpgradeAmount;
    [Header("Max Cargo")]
    [SerializeField] TextMeshProUGUI maxCargoText;
    [SerializeField] TextMeshProUGUI maxCargoCostText;
    [SerializeField] float maxCargoCost;
    [SerializeField] float maxCargoCostIncrease;
    [Space]
    [SerializeField] TextMeshProUGUI maxCargoUpgradeAmountText;
    [SerializeField] int maxCargoUpgradeAmount;
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
    int currentMaxCarsAmount;
    int currentMaxCargoAmount;

    ColorBlock accelerationColorBlock;
    ColorBlock maxSpeedColorBlock;
    ColorBlock maxPassangersColorBlock;
    ColorBlock profitColorBlock;
    ColorBlock maxCarsColorBlock;
    ColorBlock maxCargoColorBlock;

    GameManager gameManager;
    Train train;

    Button accelerationButton;
    Button maxSpeedButton;
    Button maxPassangersButton;
    Button profitButton;
    Button maxCarsButton;
    Button maxCargoButton;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        train = FindObjectOfType<Train>();

        accelerationButton = accelerationCostText.GetComponentInParent<Button>();
        maxSpeedButton = maxSpeedCostText.GetComponentInParent<Button>();
        maxPassangersButton = maxPassangerCostText.GetComponentInParent<Button>();
        profitButton = profitCostText.GetComponentInParent<Button>();
        maxCarsButton = carsCostText.GetComponentInParent<Button>();
        maxCargoButton = maxCargoCostText.GetComponentInParent<Button>();

        ChangeColor();
    }

    void Start()
    {
        LoadSavedData();

        accelerationCostText.text = accelerationCost.ToString();
        maxSpeedCostText.text = maxSpeedCost.ToString();
        maxPassangerCostText.text = maxPassangerCost.ToString();
        profitCostText.text = profitCost.ToString();
        carsCostText.text = carsCost.ToString();
        maxCargoCostText.text = maxCargoCost.ToString();
    }

    void LoadSavedData()
    {
        UpgradeSaveData data = SaveSystem.Instance.GetUpgradeData();
        if (data != null)
        {
            maxSpeedCost = data.maxSpeedCost;
            maxPassangerCost = data.maxPassangerCost;
            accelerationCost = data.accelerationCost;
            profitCost = data.profitCost;
            carsCost = data.carsCost;
            amountOfCars = data.amountOfCars > 0 ? data.amountOfCars : amountOfCars;
            currentMaxSpeedAmount = data.currentMaxSpeedAmount;
            currentMaxPassangerAmount = data.currentMaxPassangerAmount;
            currentAccelerationAmount = data.currentAccelerationAmount;
            currentProfitAmount = data.currentProfitAmount;
            currentMaxCarsAmount = data.currentCarsAmount;
        }
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
        Color maxCarsColor = originalColor;
        Color maxCargoColor = originalColor;

        coins = gameManager.GetCoins();
        if (coins < maxSpeedCost || currentMaxSpeedAmount == maxSpeedUpgradeAmount)
        {
            maxSpeedColor = cantAffordColor;
        }
        if (coins < accelerationCost || currentAccelerationAmount == maxAccelerationUpgradeAmount)
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
            maxCarsColor = cantAffordColor;
        }
        if (coins < maxCargoCost)
        {
            maxCargoColor = cantAffordColor;
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

        maxCarsColorBlock = maxCarsButton.colors;
        maxCarsColorBlock.normalColor = maxCarsColor;
        maxCarsColorBlock.highlightedColor = maxCarsColor;
        maxCarsColorBlock.selectedColor = maxCarsColor;
        maxCarsButton.colors = maxCarsColorBlock;
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
        maxCarUpgradeAmountText.text = currentMaxCarsAmount + "/" + maxCarUpgradeAmount;
    }

    public void UpgradeMaxSpeed()
    {
        if (coins < maxSpeedCost || currentMaxSpeedAmount >= maxSpeedUpgradeAmount) { return; }

        gameManager.BuyWithCoins(maxSpeedCost);
        maxSpeedCost += maxSpeedCostIncrease;
        maxSpeedCost = Mathf.Floor(maxSpeedCost);
        maxSpeedCostText.text = maxSpeedCost.ToString();
        gameManager?.AddToMaxSpeed(addToMaxSpeed);
        currentMaxSpeedAmount++;

        SaveUpgradeData();
    }

    public void UpgradeMaxPassangers()
    {
        if (coins < maxPassangerCost) { return; }

        gameManager.BuyWithCoins(maxPassangerCost);
        maxPassangerCost += maxPassangerCostIncrease;
        maxPassangerCost = Mathf.Floor(maxPassangerCost);
        maxPassangerCostText.text = maxPassangerCost.ToString();
        gameManager?.AddToMaxPassangers(addToMaxPassangers);
        currentMaxPassangerAmount++;

        SaveUpgradeData();
    }

    public void UpgradeAcceleration()
    {
        if (coins < accelerationCost || currentAccelerationAmount >= maxAccelerationUpgradeAmount) { return; }

        gameManager.BuyWithCoins(accelerationCost);
        accelerationCost += accelerationCostIncrease;
        accelerationCost = Mathf.Floor(accelerationCost);
        accelerationCostText.text = accelerationCost.ToString();
        train?.AddToAcceleration(addToAcceleration);
        currentAccelerationAmount++;

        SaveUpgradeData();
    }

    public void UprgadeProfit()
    {
        if (coins < profitCost || currentProfitAmount >= maxProfitUpgradeAmount) { return; }

        gameManager.BuyWithCoins(profitCost);
        profitCost += profitCostIncrease;
        profitCost = Mathf.Floor(profitCost);
        profitCostText.text = profitCost.ToString();
        gameManager?.AddToProfit(addToProfit);
        currentProfitAmount++;

        SaveUpgradeData();
    }

    public void UpgradeCars()
    {
        if (coins < carsCost || currentMaxCarsAmount >= maxCarUpgradeAmount) { return; }

        gameManager.BuyWithCoins(carsCost);
        carsCost += carsCostIncrease;
        carsCost = Mathf.Floor(carsCost);
        carsCostText.text = carsCost.ToString();
        amountOfCars++;
        currentMaxCarsAmount++;

        SaveUpgradeData();
    }

    public void SaveUpgradeData()
    {
        UpgradeSaveData data = SaveSystem.Instance.GetUpgradeData() ?? new UpgradeSaveData();
        data.maxSpeedCost = maxSpeedCost;
        data.maxPassangerCost = maxPassangerCost;
        data.accelerationCost = accelerationCost;
        data.profitCost = profitCost;
        data.carsCost = carsCost;
        data.amountOfCars = amountOfCars;
        data.currentMaxSpeedAmount = currentMaxSpeedAmount;
        data.currentMaxPassangerAmount = currentMaxPassangerAmount;
        data.currentAccelerationAmount = currentAccelerationAmount;
        data.currentProfitAmount = currentProfitAmount;
        data.currentCarsAmount = currentMaxCarsAmount;
        SaveSystem.Instance.SetUpgradeData(data);
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
