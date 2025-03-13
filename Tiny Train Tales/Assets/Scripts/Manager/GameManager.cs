using System.Collections;
using System.Collections.Generic;
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
    [Space]
    [SerializeField] float networth;
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
    [SerializeField] float decelerationOffset;
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
    [Header("Audio")]
    [SerializeField] AudioSource trainAudioSource;
    [Header("Regions")]
    [SerializeField] GameObject[] allRegions;
    [SerializeField] List<GameObject> unlockedRegions;
    [SerializeField] List<GameObject> unlockableRegions;
    [SerializeField] List<int> unlockedRegionIndexes;

    float remainingDistance;
    float velocity;
    int subPassangers = 0;

    bool hasDeletedKeys;
    bool hasCalculatedPassangers;
    bool hasClosedMenus;

    Train train;
    CameraMovement cam;
    CityManager cityManager;
    QuestManager questManager;
    CarManager carManager;
    AudioManager audioManager;
    #endregion

    void Awake()
    {
        train = FindObjectOfType<Train>();
        cam = FindObjectOfType<CameraMovement>();
        cityManager = FindObjectOfType<CityManager>();
        questManager = FindObjectOfType<QuestManager>();
        carManager = FindObjectOfType<CarManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Start()
    {
        PlayerPrefsSetUp();
        LoadUnlockedRegions();
    }

    public void UpdateCityTexts()
    {
        cityManager.UpdateCityTexts(currentCityText, destinationCityText);
    }

    #region PlayerPrefs
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
        if (PlayerPrefs.HasKey("Networth"))
        {
            networth = PlayerPrefs.GetFloat("Networth");
        }
        else
        {
            networth = coins;
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
    #endregion

    void Update()
    {
        HandleMaxSpeed();
        HandleDestionationDistance();

        coins = Mathf.Round(coins);
        cointext.text = coins.ToString();
        gemsText.text = gems.ToString();

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

        networth += coins;
        PlayerPrefs.SetFloat("Networth", networth);
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

    public float GetNetworth()
    {
        return networth;
    }
    #endregion

    #region Gems
    public void AddGems(float amount)
    {
        gems += amount;
        gemsText.text = gems.ToString();

        PlayerPrefs.SetFloat("Gems", gems);
    }

    public void BuyWithGems(float cost)
    {
        gems -= cost;
        gemsText.text = gems.ToString();

        PlayerPrefs.SetFloat("Gems", gems);
    }

    public float GetGems()
    {
        return gems;
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
                distance /= decelerationOffset;

                Instantiate(stationBlockPrefab, new Vector2(transform.position.x + distance, 0.72f), Quaternion.identity);
            }
            
            if (trainAudioSource.clip != audioManager.trainAudios[0] || hasArrivedAtStation) { return; }

            audioManager.PlayTrainAudio(audioManager.trainAudios[1]);
        }
        else
        {
            if (!trainAudioSource.isPlaying)
            {
                trainAudioSource.loop = true;
                audioManager.PlayTrainAudio(audioManager.trainAudios[0]);
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

        subPassangers = (int)Random.Range(0, passangers + 1);
        passangers -= subPassangers;

        int addPassangers = (int)Random.Range(0, maxPassangers - passangers + 1);
        passangers += addPassangers;

        float coinsAdded = coinsPerPassanger * subPassangers * profitMultiplier;

        Station station = FindObjectOfType<Station>();
        station.GetPassangers(subPassangers, addPassangers, coinsAdded);
        AddCoins(coinsAdded);

        PlayerPrefs.SetFloat("Passangers", passangers);

        PassangerQuest[] passangerQuests = FindObjectsOfType<PassangerQuest>();
        for (int i = 0; i < passangerQuests.Length; i++)
        {
            passangerQuests[i].TrackPassangers(subPassangers);
        }

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

        FindObjectOfType<Station>().LeaveStation();
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

    public int GetSubPassangers()
    {
        return subPassangers;
    }
    #endregion

    #region Regions
    void LoadUnlockedRegions()
    {
        // Process each region.
        for (int i = 0; i < allRegions.Length; i++)
        {
            Region currentRegionScript = allRegions[i].GetComponent<Region>();

            // If there are manually tracked unlocked region indexes, use those.
            if (PlayerPrefs.HasKey("UnlockedRegion" + i))
            {
                currentRegionScript.SetCityActivity(true);
                if (!unlockedRegions.Contains(allRegions[i]))
                    unlockedRegions.Add(allRegions[i]);

                unlockedRegionIndexes.Add(i);
                continue;
            }
            else
            {
                currentRegionScript.SetCityActivity(false);
            }

            if (unlockedRegionIndexes.Count != 0)
            {
                if (unlockedRegionIndexes.Contains(i))
                {
                    currentRegionScript.SetCityActivity(true);
                    if (!unlockedRegions.Contains(allRegions[i]))
                        unlockedRegions.Add(allRegions[i]);

                    unlockedRegionIndexes.Add(i);
                    PlayerPrefs.SetInt("UnlockedRegion" + i, 1);
                }
                else
                {
                    currentRegionScript.SetCityActivity(false);
                }
            }

            // Fallback to using PlayerPrefs.
            
        }

        // Process neighbor regions.
        for (int i = 0; i < unlockedRegions.Count; i++)
        {
            GameObject[] currentNeighbors = unlockedRegions[i].GetComponent<Region>().GetNeighbors();

            for (int ii = 0; ii < currentNeighbors.Length; ii++)
            {
                if (unlockableRegions.Contains(currentNeighbors[ii])) { continue; }

                unlockableRegions.Add(currentNeighbors[ii]);
            }
        }

        // Finally, update all connection lines globally.
        UpdateAllConnectionLines();
    }

    /// <summary>
    /// Checks every connection line (from every city's City script) and sets its active state.
    /// A connection line will be active only if all the cities that reference it are active.
    /// This ensures that if one region is locked, its city (and thus its shared connection) takes priority.
    /// </summary>
    void UpdateAllConnectionLines()
    {
        // Dictionary to map each line to the list of cities that reference it.
        Dictionary<GameObject, List<GameObject>> lineToCities = new Dictionary<GameObject, List<GameObject>>();

        // Loop through every region.
        foreach (GameObject regionObj in allRegions)
        {
            Region regionScript = regionObj.GetComponent<Region>();
            if (regionScript == null)
                continue;

            // Loop through each city in this region.
            foreach (GameObject cityObj in regionScript.regionCities)
            {
                if (cityObj == null)
                    continue;

                City cityScript = cityObj.GetComponent<City>();
                if (cityScript == null)
                    continue;

                GameObject[] cityLines = cityScript.GetCityNeighborLines();
                if (cityLines == null)
                    continue;

                foreach (GameObject line in cityLines)
                {
                    if (line == null)
                        continue;

                    // Add this city to the mapping for the line.
                    if (!lineToCities.ContainsKey(line))
                    {
                        lineToCities[line] = new List<GameObject>();
                    }
                    if (!lineToCities[line].Contains(cityObj))
                    {
                        lineToCities[line].Add(cityObj);
                    }
                }
            }
        }

        // For each connection line, if any connected city is inactive, turn the line off.
        foreach (var kvp in lineToCities)
        {
            GameObject line = kvp.Key;
            List<GameObject> connectedCities = kvp.Value;
            bool allCitiesActive = true;
            foreach (GameObject city in connectedCities)
            {
                if (!city.activeInHierarchy)
                {
                    allCitiesActive = false;
                    break;
                }
            }
            line.SetActive(allCitiesActive);
            // Uncomment below for debugging:
            // Debug.Log("Line " + line.name + " set to " + allCitiesActive);
        }
    }

    public void UnlockNewRegion(int selectedRegionIndex)
    {
        PlayerPrefs.SetInt("UnlockedRegion" + selectedRegionIndex, 1);

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    //Choose which region to start in
    //Choose next region to unlock
    //Turn off all cities and lines inside locked regions
    //Keep track and remember which regions are unlocked
    //Each region is assigned a number
    #endregion

    #region Save
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
        train?.SaveTrain();
        cam?.SavePos();
        questManager?.SaveQuests();
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

        Car[] cars = FindObjectsOfType<Car>();
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].SaveCar();
        }

        PlayerPrefs.SetFloat("MaxPassangers", maxPassangers);
        PlayerPrefs.SetFloat("MaxSpeed", maxSpeed);
        PlayerPrefs.SetFloat("Coins", coins);
        PlayerPrefs.SetFloat("Networth", networth);
        PlayerPrefs.SetFloat("Gems", gems);
        PlayerPrefs.SetFloat("Passangers", passangers);

        SaveProgress();

        PlayerPrefs.Save();
    }

    void OnApplicationQuit()
    {
        SaveAll();
    }
    #endregion
}
