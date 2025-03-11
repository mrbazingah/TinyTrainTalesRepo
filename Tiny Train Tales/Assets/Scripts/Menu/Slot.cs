using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] float pricePerPoint;
    [SerializeField] float startPrice;
    [SerializeField] float priceOffset;
    [Space]
    [SerializeField] TextMeshProUGUI weightText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI incomeText;
    [SerializeField] TextMeshProUGUI costText;
    [Space]
    [SerializeField] GameObject cross;
    [Space]
    [SerializeField] Color cantBuyColor;
    [SerializeField] Color originalColor;

    [SerializeField] float cost;
    bool hasBeenBought;
    float coins;

    int weight;
    int speed;
    int income;

    Button button;
    ColorBlock buyColorBlock;

    UpgradeManager upgradeManager;
    GameManager gameManager;
    CarManager carManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        carManager = FindObjectOfType<CarManager>();
        upgradeManager = FindObjectOfType<UpgradeManager>();

        button = costText.GetComponentInParent<Button>();
    }

    void Start()
    {
        cross.SetActive(false);
        SetUpSlot();
    }

    public void SetUpSlot()
    {
        int pastDay = PlayerPrefs.GetInt(gameObject.name + "PastDay");
        int pasthMonth = PlayerPrefs.GetInt(gameObject.name + "PastMonth");
        int pastYear = PlayerPrefs.GetInt(gameObject.name + "PastYear");

        int currentDay = System.DateTime.Now.Day;
        int currentMonth = System.DateTime.Now.Month;
        int currentYear = System.DateTime.Now.Year;

        if (pastDay < currentDay || pasthMonth < currentMonth || pastYear < currentYear)
        {
            PlayerPrefs.DeleteKey(gameObject.name + "Weight");
            PlayerPrefs.DeleteKey(gameObject.name + "Speed");
            PlayerPrefs.DeleteKey(gameObject.name + "Income");
            PlayerPrefs.DeleteKey(gameObject.name + "HasBeenBought");

            PlayerPrefs.SetInt(gameObject.name + "PastDay", currentDay);
            PlayerPrefs.SetInt(gameObject.name + "PastMonth", currentMonth);
            PlayerPrefs.SetInt(gameObject.name + "PastYear", currentYear);
        }

        if (PlayerPrefs.HasKey(gameObject.name + "Weight"))
        {
            weight = PlayerPrefs.GetInt(gameObject.name + "Weight");
            speed = PlayerPrefs.GetInt(gameObject.name + "Speed");
            income = PlayerPrefs.GetInt(gameObject.name + "Income");

            hasBeenBought = PlayerPrefs.HasKey(gameObject.name + "HasBeenBought");
            cross.SetActive(hasBeenBought);
        }
        else
        {
            weight = Random.Range(1, 6);
            PlayerPrefs.SetInt(gameObject.name + "Weight", weight);

            speed = Random.Range(1, 6);
            PlayerPrefs.SetInt(gameObject.name + "Speed", speed);

            income = Random.Range(1, 6);
            PlayerPrefs.SetInt(gameObject.name + "Income", income);
        }

        weightText.text = "Weight: " + weight.ToString() + "/5";
        speedText.text = "Speed: " + speed.ToString() + "/5";
        incomeText.text = "Income: " + income.ToString() + "/5";

        if (PlayerPrefs.HasKey(gameObject.name + "Cost"))
        {
            cost = PlayerPrefs.GetFloat(gameObject.name + "Cost");
            costText.text = cost.ToString();
        }
        else
        {
            CalculateCost();
        }
    }

    void Update()
    {
        UpdateButtons();
    }

    public void CalculateCost()
    {
        if (hasBeenBought) { return; }

        float networth = gameManager.GetNetworth();

        // Calculate a slot quality metric.
        // Higher speed and income improve quality, while weight detracts.
        float slotQuality = Mathf.Max(1, (speed + income - weight));

        // Use a scaling factor to adjust how networth influences the cost.
        // For example, if networth is 5000 and scalingFactor is 10000, multiplier becomes 1 + (5000/10000) = 1.5.
        float costMultiplier = 1 + (networth / priceOffset);

        // Apply exponential scaling to emphasize differences in slot quality.
        float exponent = 1.2f; // Adjust for more/less aggressive scaling.
        float baseCost = startPrice + pricePerPoint * Mathf.Pow(slotQuality, exponent);

        cost = Mathf.Round(baseCost * costMultiplier);

        // Update the UI with the calculated cost.
        costText.text = cost.ToString();

        PlayerPrefs.SetFloat(gameObject.name + "Cost", cost);
    }

    void UpdateButtons()
    {
        Color buttonColor = originalColor;

        coins = gameManager.GetCoins();
        if (coins < cost || upgradeManager.GetAmountOfCars() <= carManager.GetLength() || hasBeenBought)
        {
            buttonColor = cantBuyColor;
        }

        buyColorBlock = button.colors;
        buyColorBlock.normalColor = buttonColor;
        buyColorBlock.highlightedColor = buttonColor;
        buyColorBlock.selectedColor = buttonColor;
        button.colors = buyColorBlock;
    }

    public void Buy()
    {
        coins = gameManager.GetCoins();
        if (coins < cost || hasBeenBought || upgradeManager.GetAmountOfCars() <= carManager.GetLength()) { return; }

        carManager.BuyNewCar(weight, speed, income);
        gameManager.BuyWithCoins(cost);

        cross.SetActive(true);
        hasBeenBought = true;

        PlayerPrefs.SetString(gameObject.name + "HasBeenBought", "hey");
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteKey(gameObject.name + "HasBeenBought");
        PlayerPrefs.DeleteKey(gameObject.name + "Weight");
        PlayerPrefs.DeleteKey(gameObject.name + "Speed");
        PlayerPrefs.DeleteKey(gameObject.name + "Income");
        PlayerPrefs.DeleteKey(gameObject.name + "PastDay");
        PlayerPrefs.DeleteKey(gameObject.name + "PastMonth");
        PlayerPrefs.DeleteKey(gameObject.name + "PastYear");

        hasBeenBought = false;
        cross.SetActive(false);
    }
}
