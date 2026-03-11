using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using TMPro;

public class City : MonoBehaviour
{
    [SerializeField] GameObject cityMenu;
    [SerializeField] GameObject[] cityNeighbors = new GameObject[0];
    [SerializeField] int[] cityNeighborsDistances = new int[0];
    [SerializeField] GameObject[] neighborLines = new GameObject[0];
    [SerializeField] string cargoDemandName;
    [SerializeField] int cargoDemandAmount;
    
    TextMeshProUGUI resetTimerText; 
    TimeSpan timeUntilReset;

    int[] discounts = {15, 20, 25, 105, 110, 115};

    float cargoResetTime; 
    string cityName;
    string countryName;
    int currentCargoDemandCount;
    bool cargoHasBeenCreated;

    bool isUnlocked = false;

    List<GameObject> cargo = new List<GameObject>();

    CityMenu cityMenuScript;
    CargoManager cargoManager;
    CityMarketMenu cityMarketMenu;
    TimeManager timeManager;
    CargoDemand cargoDemand;

    void Awake()
    {
        cityMenuScript = FindObjectOfType<CityMenu>();
        cargoManager = FindObjectOfType<CargoManager>();
        cityMarketMenu = FindObjectOfType<CityMarketMenu>();
        timeManager = FindObjectOfType<TimeManager>();
        cargoDemand = FindObjectOfType<CargoDemand>();
    }

    void Start()
    {
        cityName = gameObject.name;
        countryName = transform.parent.name;

        cityMenu?.SetActive(false);

        if (isUnlocked && cityNeighbors.Length != 0 && neighborLines.Length != 0)
        {
            for (int i = 0; i < cityNeighbors.Length; i++)
            {
                neighborLines[i]?.SetActive(cityNeighbors[i].GetComponent<City>().GetIsUnlocked());
            }
        }

        cargoResetTime = cargoManager.GetCityCargoResetTime();
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && cityMenu.activeInHierarchy)
        {
            if (cityMenuScript == null)
            {
                cityMenuScript = FindObjectOfType<CityMenu>();
            }

            bool mouseIsOnMenu = cityMenuScript.GetMouseIsOnMenu();
            if (!mouseIsOnMenu)
            {
                CloseMenu();
            }
        }

        // --- Add countdown timer update ---
        if (SaveSystem.Instance != null)
        {
            timeUntilReset = timeManager.GetTimeUntilResetFromString(cargoResetTime, SaveSystem.Instance.GetResetTime(gameObject.name));
        }
        else
        {
            Debug.Log("SaveSystem.Instance not found");
        }

        if (resetTimerText != null)
        {
            if (timeUntilReset.TotalSeconds > 0)
            {
                resetTimerText.text = "Resets in: " + string.Format("{0:D2}:{1:D2}:{2:D2}",
                    timeUntilReset.Hours,
                    timeUntilReset.Minutes,
                    timeUntilReset.Seconds);
            }
            else
            {
                resetTimerText.text = "Ready to reset!";
            }
        }
    }


    public void OpenMenu()
    {
        cityMenu?.SetActive(true);

        if (cityMenuScript == null)
        {
            cityMenuScript = FindObjectOfType<CityMenu>();
        }

        cityMenuScript.SetTexts(cityName, countryName);
    }

    public void CloseMenu()
    {
        cityMenu?.SetActive(false);
    }

    public void SetUnlockedState(bool b)
    {
        isUnlocked = b;
    }

    public void CreateCargoItemForCity()
    {
        cargoHasBeenCreated = true;
        resetTimerText = cityMarketMenu.GetResetTimerText();

        CitySaveData savedData = SaveSystem.Instance.GetCityData(gameObject.name);

        // If reset time has elapsed, discard saved data and regenerate
        if (savedData != null && timeManager.GetCurrentTimeFromString(cargoResetTime, savedData.resetTime))
        {
            SaveSystem.Instance.DeleteCityData(gameObject.name);
            savedData = null;
        }

        if (savedData != null && savedData.cargoItems != null && savedData.cargoItems.Count > 0)
        {
            int loadCount = 0;
            for (int i = 0; i < savedData.cargoItems.Count; i++)
            {
                if (loadCount >= 3) break;
                GameObject newCargoItem = cargoManager.CreateSavedCargoItemForCity(savedData.cargoItems[i], gameObject.name);
                cargo.Add(newCargoItem);
                loadCount++;
            }
            Debug.Log($"[City] {gameObject.name}: loaded {cargo.Count} saved cargo items");
        }
        else
        {
            int count = Random.Range(cargoManager.GetCityMinCargoAmount(), cargoManager.GetCityMaxCargoAmount() + 1);

            for (int i = 0; i < count; i++)
            {
                if (cargoManager == null) { return; }

                GameObject newCargoItem = cargoManager.CreateCargoItemForCity(gameObject.name);
                if (!CheckForDuplicates(newCargoItem))
                {
                    cargo.Add(newCargoItem);
                }
                else
                {
                    newCargoItem.SetActive(false);
                    Destroy(newCargoItem);
                    continue;
                }
            }

            CitySaveData newData = SaveSystem.Instance.GetCityData(gameObject.name) ?? new CitySaveData { cityName = gameObject.name };
            newData.resetTime = timeManager.GetCurrentTimeString();
            SaveSystem.Instance.SetCityData(newData);
            Debug.Log($"[City] {gameObject.name}: generated {cargo.Count} new cargo items");
        }

        currentCargoDemandCount = savedData?.cargoDemandCount ?? 0;

        cityMarketMenu.SetCargoList(cargo);

        if (Random.Range(1, 4) == 1)
        {
            int discountIndex = Random.Range(0, discounts.Length);
            cityMarketMenu.SetDiscount(discounts[discountIndex]);
        }

        SetUpDemandCargo();
    }

    bool CheckForDuplicates(GameObject newCargo)
    {
        for (int i = 0; i < cargo.Count; i++)
        {
            if (cargo[i].GetComponent<CargoItem>().GetItemIcon().sprite == newCargo.GetComponent<CargoItem>().GetItemIcon().sprite)
            {
                return true;
            }
        }

        return false;
    }

    public void HandleMissingCargo(GameObject brokenItem)
    {
        // Try to replace with a new one instead of regenerating everything

        GameObject newCargoItem = cargoManager.CreateCargoItemForCity(gameObject.name);

        int index = cargo.IndexOf(brokenItem);
        if (index != -1)
        {
            cargo.RemoveAt(index);
        }

        cargo.Add(newCargoItem);
        Destroy(brokenItem);
        cityMarketMenu.SetCargoList(cargo);
    }

    void SetUpDemandCargo()
    {
        if (cargoDemand == null)
        {
            Debug.LogWarning("CargoDemand not found in scene for city: " + gameObject.name);
            return;
        }

        if (string.IsNullOrEmpty(cargoDemandName))
        {
            Debug.LogWarning("cargoDemandName is not set on city: " + gameObject.name);
            return;
        }

        bool found = false;
        for (int i = 0; i < cargoManager.GetCargoItemsNames().Length; i++)
        {
            if (cargoManager.GetCargoItemsNames()[i] == cargoDemandName)
            {
                cargoDemand.SetItemIcon(cargoManager.GetCargoItemsSprites()[i]);
                cargoDemand.SetItemCount(currentCargoDemandCount, cargoDemandAmount);
                cargoDemand.SetItemName(cargoDemandName);
                cargoDemand.SetCity(this);
                found = true;
                break;
            }
        }

        if (!found)
        {
            Debug.LogWarning("cargoDemandName '" + cargoDemandName + "' on city " + gameObject.name + " does not match any cargo item name.");
        }
    }

    public void AddCargoCount(int count)
    {
        currentCargoDemandCount += count;
    }

    public GameObject[] GetCityNeighbors()
    {
        return cityNeighbors;
    }

    public int[] GetCityNeighborsDistance()
    {
        return cityNeighborsDistances;
    }

    public GameObject[] GetCityNeighborLines()
    {
        return neighborLines;
    }

    public bool GetIsUnlocked()
    {
        return isUnlocked;
    }

    public string GetCargoDemandName()
    {
        return cargoDemandName;
    }

    public int GetCargoDemandAmount()
    {
        return cargoDemandAmount;
    }

    public int GetCurrentCargoDemandCount()
    {
        return currentCargoDemandCount;
    }

    public void SaveCityCargo()
    {
        if (!cargoHasBeenCreated) return;

        CitySaveData saveData = new CitySaveData();
        saveData.cityName = gameObject.name;
        saveData.cargoDemandCount = currentCargoDemandCount;
        saveData.cargoItems = new List<CargoItemSaveData>();

        // Preserve existing reset time
        CitySaveData existing = SaveSystem.Instance.GetCityData(gameObject.name);
        if (existing != null)
            saveData.resetTime = existing.resetTime;

        Sprite[] sprites = cargoManager.GetCargoItemsSprites();

        for (int i = cargo.Count - 1; i >= 0; i--)
        {
            if (cargo[i] == null)
            {
                cargo.RemoveAt(i);
                continue;
            }

            CargoItem itemScript = cargo[i].GetComponent<CargoItem>();

            CargoItemSaveData itemData = new CargoItemSaveData();
            itemData.itemName = itemScript.GetItemName();
            itemData.itemCount = itemScript.GetItemCount();
            itemData.itemPrice = itemScript.GetItemPrice();
            itemData.purchasePrice = itemScript.GetPurchasePrice();

            for (int j = 0; j < sprites.Length; j++)
            {
                if (itemScript.GetItemIcon().sprite == sprites[j])
                {
                    itemData.spriteIndex = j;
                    break;
                }
            }

            saveData.cargoItems.Add(itemData);
        }

        SaveSystem.Instance.SetCityData(saveData);
        Debug.Log($"[City] {gameObject.name}: saved {saveData.cargoItems.Count} cargo items (demand: {currentCargoDemandCount})");
    }
}
