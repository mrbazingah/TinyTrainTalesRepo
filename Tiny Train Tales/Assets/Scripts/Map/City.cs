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
    int iterations;

    int currentCargoDemandCount;

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
        timeUntilReset = timeManager.GetTimeUntilReset(cargoResetTime, "CityTime" + gameObject.name);

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
        resetTimerText = cityMarketMenu.GetResetTimerText();

        if (timeManager.GetCurrentTime(cargoResetTime, "CityTime" + gameObject.name))
        {
            PlayerPrefs.DeleteKey("CargoItemAmount" + gameObject.name);
        }

        int index = 0;
        if (PlayerPrefs.HasKey("CargoItemAmount" + gameObject.name))
        {
            index = PlayerPrefs.GetInt("CargoItemAmount" + gameObject.name);

            for (int i = 0; i < index; i++)
            {
                iterations++;
                if (iterations > 2) { break; }

                string saveString = PlayerPrefs.GetString("CitySaveString" + i.ToString() + gameObject.name);
                GameObject newCargoItem = cargoManager.CreateSavedCargoItemForCity(saveString, gameObject.name);
                cargo.Add(newCargoItem);
            }
        }
        else
        {
            index = Random.Range(cargoManager.GetCityMinCargoAmount(), cargoManager.GetCityMaxCargoAmount() + 1);

            PlayerPrefs.SetInt("CargoItemAmount" + gameObject.name, index);

            for (int i = 0; i < index; i++)
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

            timeManager.SaveCurrentTime("CityTime" + gameObject.name);
        }

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
        currentCargoDemandCount = PlayerPrefs.GetInt("CargoDemandCount" + gameObject.name);
        for (int i = 0; i < cargoManager.GetCargoItemsNames().Length; i++)
        {
            if (cargoManager.GetCargoItemsNames()[i] == cargoDemandName)
            {
                cargoDemand.SetItemIcon(cargoManager.GetCargoItemsSprites()[i]);
                cargoDemand.SetItemCount(currentCargoDemandCount, cargoDemandAmount);
                cargoDemand.SetItemName(cargoDemandName);
                cargoDemand.SetCity(this);
                break;
            }
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
        PlayerPrefs.SetInt("CargoItemAmount" + gameObject.name, cargo.Count);
        PlayerPrefs.SetInt("CargoDemandCount" + gameObject.name, currentCargoDemandCount);

        for (int i = 0; i < cargo.Count; i++)
        {
            if (cargo[i] == null)
            {
                cargo.RemoveAt(i);
                continue;
            }

            CargoItem cargoItemScript = cargo[i].GetComponent<CargoItem>();
            string saveString = cargoItemScript.GetSaveString();
            PlayerPrefs.SetString("CitySaveString" + i.ToString() + gameObject.name, saveString);

            cargoItemScript.SaveCargoItem();
        }
    }
}
