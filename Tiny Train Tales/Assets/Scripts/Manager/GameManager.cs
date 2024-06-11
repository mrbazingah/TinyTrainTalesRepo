using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables
    [Header("Coins")]
    [SerializeField] float coins;
    [SerializeField] TextMeshProUGUI cointext;
    [SerializeField] float profitMultiplier = 1;
    [Header("Gems")]
    [SerializeField] float gems;
    [SerializeField] TextMeshProUGUI gemsText;
    [Header("Speed")]
    [SerializeField] float maxSpeed;
    [SerializeField] TextMeshProUGUI speedText;
    [Header("Progress")]
    [SerializeField] float distance;
    [SerializeField] bool destinationReached;
    [SerializeField] Slider distanceSlider;
    [SerializeField] TextMeshProUGUI remainingDistanceText;
    [Space]
    [SerializeField] TextMeshProUGUI currentCityText;
    [SerializeField] TextMeshProUGUI destinationCityText;
    [Space]
    [SerializeField] float hey;
    [Header("Station")]
    [SerializeField] GameObject startStation;
    [SerializeField] GameObject stationBlockPrefab;
    [SerializeField] bool stationHasSpawned;
    [SerializeField] bool hasArrivedAtStation;
    [SerializeField] float stationDestructDistance;
    [Header("Settings")]
    [SerializeField] Toggle autoCollectToggle;
    [SerializeField] Toggle autoLeaveStation;
    [Header("Passangers")]
    [SerializeField] float maxPassangers;
    [SerializeField] float passangers;
    [SerializeField] TextMeshProUGUI passangerText;
    [SerializeField] float coinsPerPassanger;

    float remainingDistance;
    float velocity;

    bool hasDeletedKeys;
    bool hasCalculatedPassangers;
    bool hasClosedMenus;

    Train train;
    CameraMovement cam;
    UpgradeManager upgrades;
    CityManager cityManager;
    City city;
    QuestManager questManager;
    CarManager carManager;
    #endregion

    void Awake()
    {
        train = FindObjectOfType<Train>();
        cam = FindObjectOfType<CameraMovement>();
        upgrades = FindObjectOfType<UpgradeManager>();
        cityManager = FindObjectOfType<CityManager>();
        city = FindObjectOfType<City>();    
        questManager = FindObjectOfType<QuestManager>();
        carManager = FindObjectOfType<CarManager>();
    }

    void Start()
    {
        PlayerPrefsSetUp();
    }

    public void UpdateCityTexts()
    {
        cityManager.UpdateCityTexts(currentCityText, destinationCityText);
    }

    void PlayerPrefsSetUp()
    {
        if (PlayerPrefs.HasKey("MaxSpeed"))
        {
            maxSpeed = PlayerPrefs.GetFloat("MaxSpeed");
        }
        if (PlayerPrefs.HasKey("Coins"))
        {
            coins = PlayerPrefs.GetFloat("Coins");
        }
        if (PlayerPrefs.HasKey("Gems"))
        {
            gems = PlayerPrefs.GetFloat("Gems");
        }
        if (PlayerPrefs.HasKey("MaxPassangers"))
        {
            maxPassangers = PlayerPrefs.GetFloat("MaxPassangers");
        }
        if (PlayerPrefs.HasKey("Passangers"))
        {
            passangers = PlayerPrefs.GetFloat("Passangers");
        }
        if (PlayerPrefs.HasKey("AutoCollect"))
        {
            int i = PlayerPrefs.GetInt("AutoCollect");
            if (i == 1)
            {
                autoCollectToggle.isOn = true;
            }
            else
            {
                autoCollectToggle.isOn = false;
            }
        }
        if (PlayerPrefs.HasKey("AutoLeave"))
        {
            int i = PlayerPrefs.GetInt("AutoLeave");
            if (i == 1)
            {
                autoLeaveStation.isOn = true;
            }
            else
            {
                autoLeaveStation.isOn = false;
            }
        }
        if (PlayerPrefs.HasKey("Profit"))
        {
            profitMultiplier = PlayerPrefs.GetFloat("Profit");
        }
        if (PlayerPrefs.HasKey("Distance") && PlayerPrefs.HasKey("RemainingDistance"))
        {
            distance = PlayerPrefs.GetFloat("Distance");
            remainingDistance = PlayerPrefs.GetFloat("RemainingDistance");

            if (distance - remainingDistance > stationDestructDistance)
            {
                Destroy(startStation);
            }
            else
            {
                Destroy(startStation, 5f);
            }
        }
        else if (PlayerPrefs.HasKey("Distance"))
        {
            float temp = PlayerPrefs.GetFloat("Distance");
            if (temp != 0)
            {
                distance = temp;
            }

            remainingDistance = distance;
        }
        else
        {
            remainingDistance = distance;
        }

        distanceSlider.maxValue = distance;
        distanceSlider.value = remainingDistance;
        remainingDistance = Mathf.Round(remainingDistance);
        remainingDistanceText.text = remainingDistance.ToString() + "km";
    }

    void Update()
    {
        HandleMaxSpeed();
        HandleDestionationDistance();

        coins = Mathf.Round(coins);
        cointext.text = coins.ToString();
        passangerText.text = passangers.ToString() + "/" + maxPassangers.ToString();
    }

    #region Coins
    public bool GetAutoCollect()
    {
        return autoCollectToggle.isOn;
    }

    public void OnAutoCollectChange()
    {
        if (autoCollectToggle.isOn)
        {
            PlayerPrefs.SetInt("AutoCollect", 1);
        }
        else
        {
            PlayerPrefs.SetInt("AutoCollect", 0);
        }
    }

    public void AddCoins(float amountAdded)
    {
        coins += amountAdded * profitMultiplier;
        PlayerPrefs.SetFloat("Coins", coins);
    }

    public void BuyWithCoins(float cost)
    {
        coins -= cost;
        PlayerPrefs.SetFloat("Coins", coins);
    }

    public float GetCoins()
    {
        return coins;
    }
    #endregion

    #region Gems
    public void AddToGems(float amount)
    {
        gems += amount;
        PlayerPrefs.SetFloat("Gems", gems);
    }

    public void BuyWithGems(float cost)
    {
        gems -= cost;
        PlayerPrefs.SetFloat("Gems", gems);
    }
    #endregion

    #region Upgrades
    void HandleMaxSpeed()
    {
        velocity = train.GetVelocity();
        if (velocity <= 0)
        {
            velocity *= 5;
        }
        else
        {
            velocity = velocity * 5f + 1f;
        }

        velocity = Mathf.Floor(velocity);

        if (velocity >= maxSpeed)
        {
            speedText.text = maxSpeed.ToString() + " km/h";
        }
        else
        {
            speedText.text = velocity.ToString() + " km/h";
        }
    }

    public void AddToMaxSpeed(float amountAdded)
    {
        maxSpeed += amountAdded;
        PlayerPrefs.SetFloat("MaxSpeed", maxSpeed);
    }

    public void AddToMaxPassangers(float amountAdded)
    {
        maxPassangers += amountAdded;
        PlayerPrefs.SetFloat("MaxPassangers", maxPassangers);
    }

    public void AddToProfit(float amountAdded)
    {
        profitMultiplier += amountAdded;
        profitMultiplier = Mathf.Round(profitMultiplier * 100f) / 100f;
        PlayerPrefs.SetFloat("Profit", profitMultiplier);
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public float GetMaxPassangers()
    {
        return maxPassangers;
    }

    public float GetProfit()
    {
        return profitMultiplier;
    }
    #endregion

    #region Destination
    void HandleDestionationDistance()
    {
        remainingDistance -= velocity * Time.deltaTime / 60f;
        distanceSlider.value = distance - remainingDistance;

        if ((int)remainingDistance < (int)(remainingDistance + velocity * Time.deltaTime / 60f))
        {
            remainingDistance = Mathf.Round(remainingDistance);
            remainingDistanceText.text = remainingDistance.ToString() + "km";
        }

        if (remainingDistance <= 0f)
        {
            remainingDistance = 0f;

            if (!destinationReached)
            {
                destinationReached = true;
                train?.StopTrain();

                float speed = train.GetSpeed();
                float deceleration = train.GetDecelartion();
                float time = 3;

                float distance = speed * time - deceleration / 2 * 9;
                distance /= hey;

                Instantiate(stationBlockPrefab, new Vector2(transform.position.x + distance, 0.72f), Quaternion.identity);
            }
        }
    }

    public void SetNewDestination(string oldcity, string newCity, float distanceToCity)
    {
        destinationCityText.text = newCity;
        currentCityText.text = oldcity;

        distance = distanceToCity;
        PlayerPrefs.SetFloat("Distance", distance);
    }

    public void AddAndSubtractPassangers()
    {
        if (hasCalculatedPassangers) { return; }

        int subPassangers = (int)Random.Range(0, passangers + 1);
        int addPassangers = (int)Random.Range(0, maxPassangers - passangers + 1);

        passangers -= subPassangers;
        passangers += addPassangers;
        float coinsAdded = coinsPerPassanger * subPassangers * profitMultiplier;

        Station station = FindObjectOfType<Station>();
        station.GetPassangers(subPassangers, addPassangers, coinsAdded);
        AddCoins(coinsAdded);

        PlayerPrefs.SetFloat("Passangers", passangers);

        hasCalculatedPassangers = true;
    }

    public void ResetDestination()
    {
        distance = 10;

        distanceSlider.value = distance;
        distanceSlider.maxValue = distance;

        remainingDistance = distance;
        remainingDistanceText.text = distance.ToString() + "km";

        destinationReached = false;
    }

    public void HandleArrival(bool b)
    {
        hasArrivedAtStation = b;

        if (!hasClosedMenus)
        {
            SaveAll();
            AddAndSubtractPassangers();

            if (autoLeaveStation.isOn)
            {
                StartCoroutine(LeaveStationAutomatically());
            }

            hasClosedMenus = true;
        }
    }

    IEnumerator LeaveStationAutomatically()
    {
        yield return new WaitForSeconds(1);

        Station station = FindObjectOfType<Station>();
        station.LeaveStation();
    }

    public void OnAutoLeaveStationChange()
    {
        if (autoLeaveStation.isOn)
        {
            PlayerPrefs.SetInt("AutoLeave", 1);
        }
        else
        {
            PlayerPrefs.SetInt("AutoLeave", 0);
        }
    }

    public void HandleStationSpawn(bool b)
    {
        stationHasSpawned = b;
    }

    public bool GetStationHasSpawned()
    {
        return stationHasSpawned;
    }

    public bool GetHasArrivedAtStation()
    {
        return hasArrivedAtStation;
    }

    public bool GetDestinationReached()
    {
        return destinationReached;
    }

    public float GetDistance()
    {
        return distance;
    }

    public float GetRemainingDistance()
    {
        return remainingDistance;
    }
    #endregion

    #region Save
    public void SaveCar(float currentTime, float time, string name)
    {
        PlayerPrefs.SetFloat(name + "Time", time);
        PlayerPrefs.SetFloat(name + "CurrentTime", currentTime);
    }

    void SaveProgress()
    {
        if (remainingDistance <= 0 || train.GetSpeed() <= 0)
        {
            DeleteSavedDestination(false);
            return;
        }

        PlayerPrefs.SetFloat("Distance", distance);
        PlayerPrefs.SetFloat("RemainingDistance", remainingDistance);
    }

    public void DeleteSavedDestination(bool isButton)
    {
        if (hasDeletedKeys || PlayerPrefs.GetInt("Dont Destroy") == 1) { return; }

        PlayerPrefs.DeleteKey("Distance");
        PlayerPrefs.DeleteKey("RemainingDistance");
        PlayerPrefs.DeleteKey("Speed");

        if (isButton)
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentScene);
        }

        hasDeletedKeys = true;
    }

    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public void SaveAll()
    {
        train?.SaveTrain();
        questManager?.SaveTravelDistance();
        cam?.SavePos();
        carManager?.SaveCars();

        if (hasArrivedAtStation)
        {
            cityManager?.SaveOnDeparture();
        }
        else
        {
            cityManager?.SaveCityOnQuit();
        }

        MenuAnimationY[] buttons = FindObjectsOfType<MenuAnimationY>();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SavePos();
        }

        PlayerPrefs.SetFloat("MaxPassangers", maxPassangers);
        PlayerPrefs.SetFloat("MaxSpeed", maxSpeed);
        PlayerPrefs.SetFloat("Coins", coins);
        PlayerPrefs.SetFloat("Gems", gems);
        PlayerPrefs.SetFloat("Passangers", passangers);

        SaveProgress();
    }

    void OnApplicationQuit()
    {
        SaveAll();
    }
    #endregion
}
