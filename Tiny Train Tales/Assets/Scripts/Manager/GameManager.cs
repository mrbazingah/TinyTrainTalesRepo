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
    [SerializeField] Toggle autoCollectToggle;
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
    [Header("Map")]
    [SerializeField] GameObject map;
    [SerializeField] Vector2 mapOffset;
    [SerializeField] Vector2 mapStartPos;
    [Header("Settings")]
    [SerializeField] GameObject settingsMenu;
    [Header("Station")]
    [SerializeField] GameObject station;
    [SerializeField] bool stationHasSpawned;
    [SerializeField] bool hasArrivedAtStation;
    [SerializeField] float stationDestructDistance;
    [Header("Passangers")]
    [SerializeField] float maxPassangers;
    [SerializeField] float passangers;
    [SerializeField] TextMeshProUGUI passangerText;
    [SerializeField] float coinsPerPassanger;
    [Header("Cars")]
    [SerializeField] GameObject[] currentCars;

    float remainingDistance;
    float velocity;

    bool hasDeletedKeys;
    bool hasCalculatedPassangers;

    Train train;
    CameraMovement cam;
    UpgradeManager upgrades;
    CityManager cityManager;
    MapProgress mapTrain;
    City city;
    #endregion

    void Awake()
    {
        train = FindObjectOfType<Train>();
        cam = FindObjectOfType<CameraMovement>();
        upgrades = FindObjectOfType<UpgradeManager>();
        cityManager = FindObjectOfType<CityManager>();
        mapTrain = FindObjectOfType<MapProgress>();
        city = FindObjectOfType<City>();    
    }

    void Start()
    {
        PlayerPrefsSetUp();
        CloseAllMenus();
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
                Destroy(station);
            }
            else
            {
                Destroy(station, 5f);
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

    public void Buy(float cost)
    {
        coins -= cost;
        PlayerPrefs.SetFloat("Coins", coins);
    }

    public float GetCoins()
    {
        return coins;
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

    #region Menus
    void CloseAllMenus()
    {
        CloseMap();
        CloseSettings(settingsMenu);
        upgrades.CloseUpgradeMenu();
    }

    public void OpenMap()
    {
        Vector2 camPos = cam.transform.position;

        map.transform.position = new Vector3(camPos.x, camPos.y, 0f);
        cam.LockMovement(true);
    }

    public void CloseMap()
    {
        map.transform.position = mapOffset;
        cam.LockMovement(false);

        city.CloseMenu();
    }

    public void OpenSettings(GameObject settingsMenu)
    {
        settingsMenu.SetActive(true);
        cam.LockMovement(true);
    }

    public void CloseSettings(GameObject settingsMenu)
    {
        settingsMenu.SetActive(false);
        cam.LockMovement(false);
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
            destinationReached = true;
            train.StopTrain();
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

        Station station = FindObjectOfType<Station>();
        station.GetPassangers(subPassangers, addPassangers);
        AddCoins(coinsPerPassanger * subPassangers);

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

        SaveAll();
        AddAndSubtractPassangers();
        DeleteSavedDestination(false);
        CloseAllMenus();
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

    #region Cars
    public void AddCar()
    {
        GameObject[] tempCars = new GameObject[currentCars.Length + 1];
        currentCars.CopyTo(tempCars, 0);
        currentCars = tempCars;
    }

    public void RemoveCar()
    {
        GameObject[] tempCars = new GameObject[currentCars.Length - 1];
        currentCars.CopyTo(tempCars, 0);
        currentCars = tempCars;
    }
    #endregion

    #region Save
    public void SaveCar(bool isActive, string name)
    {
        if (isActive)
        {
            PlayerPrefs.SetInt(name, 1);
        }
        else
        {
            PlayerPrefs.SetInt(name, 0);
        }
    }

    void SaveProgress()
    {
        if (remainingDistance <= 0)
        {
            DeleteSavedDestination(false);
            return;
        }

        PlayerPrefs.SetFloat("Distance", distance);
        PlayerPrefs.SetFloat("RemainingDistance", remainingDistance);
    }

    public void DeleteSavedDestination(bool isButton)
    {
        if (hasDeletedKeys) { return; }

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
        train.SaveTrain();

        PlayerPrefs.SetFloat("MaxPassangers", maxPassangers);
        PlayerPrefs.SetFloat("MaxSpeed", maxSpeed);
        PlayerPrefs.SetFloat("Coins", coins);
        PlayerPrefs.SetFloat("Passangers", passangers);

        SaveProgress();
    }

    void OnApplicationQuit()
    {
        SaveAll();
    }
    #endregion
}
