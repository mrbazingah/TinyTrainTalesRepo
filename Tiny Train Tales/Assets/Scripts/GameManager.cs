using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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
    [Space]
    [SerializeField] GameObject map;
    [Header("Station")]
    [SerializeField] bool stationHasSpawned;
    [SerializeField] bool hasArrivedAtStation;
    [Header("Passangers")]
    [SerializeField] float maxPassangers;
    [SerializeField] float passangers;
    [SerializeField] TextMeshProUGUI passangerText;
    [SerializeField] float coinsPerPassanger;

    float remainingDistance;
    float velocity;

    bool hasCalculatedPassangers;

    Train train;
    CameraMovement cam;

    void Awake()
    {
        train = FindObjectOfType<Train>();
        cam = FindObjectOfType<CameraMovement>();
    }

    void Start()
    {
        PlayerPrefsSetUp();
        map.SetActive(false);
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
            distance = PlayerPrefs.GetFloat("Distace");
            remainingDistance = PlayerPrefs.GetFloat("RemainingDistance");
        }
        else
        {
            distance = Random.Range(10, distance + 1);
            distance = Mathf.Round(distance);

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

    void HandleMaxSpeed()
    {
        velocity = train.GetVelocity();
        if (velocity <= 0)
        {
            velocity *= 5;
        }
        else
        {
            velocity = velocity * 5 + 1;
        }

        velocity = Mathf.Floor(velocity);
        speedText.text = velocity.ToString() + " km/h";
    }

    public void AddAndSubtractPassangers()
    {
        if (hasCalculatedPassangers) { return; }

        int subPassangers = (int)Random.Range(0, passangers + 1);
        int addPassangers = (int)Random.Range(0, maxPassangers - passangers + 1);

        passangers -= subPassangers;
        passangers += addPassangers;
        AddCoins(coinsPerPassanger * subPassangers);

        PlayerPrefs.SetFloat("Passangers", passangers);

        hasCalculatedPassangers = true;
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
        profitMultiplier = Mathf.Round(profitMultiplier * 10.0f) * 0.1f;
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

    #region Map
    public void OpenMap()
    {
        map.SetActive(true);
        cam.LockMovement(true);
    }

    public void CloseMap()
    {
        map.SetActive(false);
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

        if (remainingDistance <= 0)
        {
            remainingDistance = 0;
            destinationReached = true;
            train.StopTrain();
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

    public void HandleArrival(bool b)
    {
        hasArrivedAtStation = b;

        SaveAll();
        AddAndSubtractPassangers();
    }

    public bool GetHasArrivedAtStation()
    {
        return hasArrivedAtStation;
    }

    public bool GetDestinationReached()
    {
        return destinationReached;
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

    public void newDestination(string newCity, float distanceToCity)
    {
        currentCityText.text = destinationCityText.text;
        destinationCityText.text = newCity;

        distance = distanceToCity;
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
        PlayerPrefs.SetFloat("Distance", distance);
        PlayerPrefs.SetFloat("RemainingDistance", remainingDistance);
    }

    public void ResetDestionation()
    {
        PlayerPrefs.DeleteKey("Distance");
        PlayerPrefs.DeleteKey("RemainingDistance");
    }

    public void SaveAll()
    {
        PlayerPrefs.SetFloat("MaxPassangers", maxPassangers);
        PlayerPrefs.SetFloat("MaxSpeed", maxSpeed);
        PlayerPrefs.SetFloat("Coins", coins);
        PlayerPrefs.SetFloat("Coins", coins);
        PlayerPrefs.SetFloat("Passangers", passangers);
    }

    void OnApplicationQuit()
    {
        SaveProgress();
        SaveAll();
    }
    #endregion
}
