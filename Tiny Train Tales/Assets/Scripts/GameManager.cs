using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Coins")]
    [SerializeField] float coins;
    [SerializeField] TextMeshProUGUI cointext;
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

    void Awake()
    {
        train = FindObjectOfType<Train>();
    }

    void Start()
    {
        distance = Random.Range(10, distance + 1);
        distance = Mathf.Round(distance);

        distanceSlider.maxValue = distance;
        remainingDistance = distance;
        remainingDistanceText.text = distance.ToString() + "km";

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
    }

    void Update()
    {
        HandleMaxSpeed();
        HandleDestionationDistance();

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
        }
    }

    public void AddAndSubtractPassangers()
    {
        if (hasCalculatedPassangers) { return; }

        int subPassangers = (int)Random.Range(0, passangers + 1);
        int addPassangers = (int)Random.Range(0, maxPassangers - passangers + 1);

        passangers -= subPassangers;
        passangers += addPassangers;
        coins += coinsPerPassanger * subPassangers;

        Debug.Log(subPassangers + " deborded the train");
        Debug.Log(addPassangers + " borded the train");

        PlayerPrefs.SetFloat("Passangers", passangers);

        hasCalculatedPassangers = true;
    }

    public bool GetAutoCollect()
    {
        return autoCollectToggle;
    }

    public void AddCoins(int amountAdded)
    {
        coins += amountAdded;
        PlayerPrefs.SetFloat("Coins", coins);
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

    public void Buy(float cost)
    {
        coins -= cost;
        PlayerPrefs.SetFloat("Coins", coins);
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public float GetCoins()
    {
        return coins;
    }

    public float GetMaxPassangers()
    {
        return maxPassangers;
    }

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

    public void SaveAll()
    {
        PlayerPrefs.SetFloat("MaxPassangers", maxPassangers);
        PlayerPrefs.SetFloat("MaxSpeed", maxSpeed);
        PlayerPrefs.SetFloat("Coins", coins);
        PlayerPrefs.SetFloat("Coins", coins);
        PlayerPrefs.SetFloat("Passangers", passangers);
    }
}
