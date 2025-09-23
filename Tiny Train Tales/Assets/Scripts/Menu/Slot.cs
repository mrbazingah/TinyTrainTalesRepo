using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] float pricePerPoint;
    [SerializeField] float startPrice;
    [SerializeField] float priceIncrease;
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
    [SerializeField] string slotID; 
    [SerializeField] float cost;
    [Space]
    [SerializeField] float resetTime; // in hours

    bool hasBeenBought;
    float coins;

    int weight;
    int speed;
    int income;

    int timesBought;

    Button button;
    ColorBlock buyColorBlock;

    UpgradeManager upgradeManager;
    GameManager gameManager;
    CarManager carManager;
    TimeManager timeManager;

    void Awake()
    {
        if (string.IsNullOrEmpty(slotID))
        {
            slotID = gameObject.name;
        }

        gameManager = FindObjectOfType<GameManager>();
        carManager = FindObjectOfType<CarManager>();
        upgradeManager = FindObjectOfType<UpgradeManager>();
        timeManager = FindObjectOfType<TimeManager>();

        button = costText.GetComponentInParent<Button>();
    }

    void Start()
    {
        cross.SetActive(false);
        SetUpSlot();
    }

    public void SetUpSlot()
    {
        string keyPrefix = slotID;
        if (timeManager.GetCurrentTime(resetTime, keyPrefix + "Time"))
        {
            // Reset keys if it’s a new day
            PlayerPrefs.DeleteKey(keyPrefix + "Weight");
            PlayerPrefs.DeleteKey(keyPrefix + "Speed");
            PlayerPrefs.DeleteKey(keyPrefix + "Income");
            PlayerPrefs.DeleteKey(keyPrefix + "HasBeenBought");
        }

        if (PlayerPrefs.HasKey(keyPrefix + "Weight"))
        {
            weight = PlayerPrefs.GetInt(keyPrefix + "Weight");
            speed = PlayerPrefs.GetInt(keyPrefix + "Speed");
            income = PlayerPrefs.GetInt(keyPrefix + "Income");

            hasBeenBought = PlayerPrefs.HasKey(keyPrefix + "HasBeenBought");
            cross.SetActive(hasBeenBought);
        }
        else
        {
            weight = Random.Range(1, 6);
            PlayerPrefs.SetInt(keyPrefix + "Weight", weight);

            speed = Random.Range(1, 6);
            PlayerPrefs.SetInt(keyPrefix + "Speed", speed);

            income = Random.Range(1, 6);
            PlayerPrefs.SetInt(keyPrefix + "Income", income);

            timeManager.SaveCurrentTime(keyPrefix + "Time");
        }

        weightText.text = "Weight: " + weight.ToString() + "/5";
        speedText.text = "Speed: " + speed.ToString() + "/5";
        incomeText.text = "Income: " + income.ToString() + "/5";

        CalculateCost();
    }

    void Update()
    {
        UpdateButtons();
        CalculateCost();
    }

    public void CalculateCost()
    {
        if (hasBeenBought) { return; }

        timesBought = PlayerPrefs.GetInt("TimesBought");

        // Calculate cost using startPrice as a base and ensure the cost is not negative.
        cost = startPrice + (speed + income - weight) * pricePerPoint + (timesBought * priceIncrease);
        cost = Mathf.Max(cost, 0f);
        costText.text = cost.ToString();
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

        string keyPrefix = slotID;
        // Save the purchase state so it persists across sessions.
        PlayerPrefs.SetInt(keyPrefix + "HasBeenBought", 1);

        timesBought = PlayerPrefs.GetInt("TimesBought");
        PlayerPrefs.SetInt("TimesBought", timesBought + 1);
    }

    public void ResetPlayerPrefs()
    {
        string keyPrefix = slotID;
        PlayerPrefs.DeleteKey(keyPrefix + "HasBeenBought");
        PlayerPrefs.DeleteKey(keyPrefix + "Weight");
        PlayerPrefs.DeleteKey(keyPrefix + "Speed");
        PlayerPrefs.DeleteKey(keyPrefix + "Income");

        hasBeenBought = false;
        cross.SetActive(false);
    }
}
