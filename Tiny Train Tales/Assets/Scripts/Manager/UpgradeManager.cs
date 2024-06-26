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
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] int maxSpeedAmount;
    [Header("Max Passangers")]
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
    [SerializeField] TextMeshProUGUI profitCostText;
    [SerializeField] float profitCost;
    [SerializeField] float addToProfit;
    [SerializeField] float profitCostIncrease;
    [Header("Cars")]
    [SerializeField] TextMeshProUGUI carsText;
    [SerializeField] TextMeshProUGUI carsCostText;
    [SerializeField] float carsCost;
    [SerializeField] float carsCostIncrease;
    [Header("Colors")]
    [SerializeField] Color originalColor;
    [SerializeField] Color cantAffordColor;
    [SerializeField] Color doneColor;

    float coins;
    int amountOfCars = 1;

    int maxSpeedCurrentAmount;

    ColorBlock accelerationColorBlock;
    ColorBlock maxSpeedColorBlock;
    ColorBlock maxPassangersColorBlock;
    ColorBlock profitColorBlock;
    ColorBlock carsColorBlock;

    GameManager gameManager;
    Train train;
    CameraMovement cam;
    CarManager carManager;

    Button accelerationButton;
    Button maxSpeedButton;
    Button maxPassangersButton;
    Button profitButton;
    Button carsButton;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        train = FindObjectOfType<Train>();
        cam = FindObjectOfType<CameraMovement>();
        carManager = FindObjectOfType<CarManager>();

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

        accelerationCostText.text = accelerationCost.ToString();
        maxSpeedCostText.text = maxSpeedCost.ToString();
        maxPassangerCostText.text = maxPassangerCost.ToString();
        profitCostText.text = profitCost.ToString();
        carsCostText.text = carsCost.ToString();

        amountText.text = maxSpeedCurrentAmount.ToString() + "/" + maxSpeedAmount.ToString();
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

        if (maxSpeedAmount == maxSpeedCurrentAmount)
        {
            maxSpeedColor = doneColor;
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
        accelerationText.text = "Current: " + train.GetAcceleration().ToString();
        profitText.text = "Current: " + gameManager.GetProfit().ToString() + "X";
        carsText.text = "Current: " + amountOfCars.ToString();
    }

    public void UpgradeMaxSpeed()
    {
        if (coins < maxSpeedCost || maxSpeedCurrentAmount == maxSpeedAmount) { return; }

        maxSpeedCurrentAmount++;
        amountText.text = maxSpeedCurrentAmount.ToString() + "/" + maxSpeedAmount.ToString();

        gameManager.BuyWithCoins(maxSpeedCost);
        maxSpeedCost += maxSpeedCostIncrease;
        maxSpeedCost = Mathf.Floor(maxSpeedCost);
        maxSpeedCostText.text = maxSpeedCost.ToString();

        PlayerPrefs.SetFloat("MaxSpeedCost", maxSpeedCost);
        gameManager?.AddToMaxSpeed(addToMaxSpeed);
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
    }

    public void UpgradeAcceleration()
    {
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
        if (coins < profitCost) { return; }

        gameManager.BuyWithCoins(profitCost);
        profitCost += profitCostIncrease;
        profitCost = Mathf.Floor(profitCost);
        profitCostText.text = profitCost.ToString();

        PlayerPrefs.SetFloat("ProfitCost", profitCost);
        gameManager?.AddToProfit(addToProfit);
    }

    public void UpgradeCars()
    {
        if (coins < carsCost) { return; }

        gameManager.BuyWithCoins(carsCost);
        carsCost += carsCostIncrease;
        carsCost = Mathf.Floor(carsCost);
        carsCostText.text = carsCost.ToString();
        amountOfCars++;

        PlayerPrefs.SetFloat("CarsCost", carsCost);
        PlayerPrefs.SetInt("AmountOfCars", amountOfCars);
    }

    public int GetAmountOfCars()
    {
        return amountOfCars; 
    }
}
