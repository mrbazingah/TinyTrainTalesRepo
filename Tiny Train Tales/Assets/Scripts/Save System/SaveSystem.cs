using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class CargoItemSaveData
{
    public string itemName;
    public int itemCount;
    public float itemPrice;
    public int spriteIndex;
    public float purchasePrice;
}

[Serializable]
public class CitySaveData
{
    public string cityName;
    public int cargoDemandCount;
    public string resetTime;
    public List<CargoItemSaveData> cargoItems = new List<CargoItemSaveData>();
}


[Serializable]
public class TrainSaveData
{
    public float trainSpeed;
    public float trainAcceleration;
}

[Serializable]
public class CurrencySaveData
{
    public float coins;
    public float gems;
    public float networth;
}

[Serializable]
public class UpgradeSaveData
{
    public float maxSpeed;
    public float maxPassangers;
    public float profitMultiplier;
    public float maxSpeedCost;
    public float maxPassangerCost;
    public float accelerationCost;
    public float profitCost;
    public float carsCost;
    public float maxCargoCost;
    public int amountOfCars;
    public int currentMaxSpeedAmount;
    public int currentMaxPassangerAmount;
    public int currentAccelerationAmount;
    public int currentProfitAmount;
    public int currentCarsAmount;
    public int currentMaxCargoAmount;
}

[Serializable]
public class PassangerSaveData
{
    public float passanger;
}

[Serializable]
public class DayNightSaveData
{
    public int currentTime;
    public float currentDayNightDuration;
    public float currentMorningEveningDuration;
}

[Serializable]
public class UpgradeSaveFile
{
    public List<UpgradeSaveData> upgrades = new List<UpgradeSaveData>();
}

[Serializable]
public class CitiesSaveFile
{
    public List<CitySaveData> cities = new List<CitySaveData>();
}

[Serializable]
public class InventorySaveFile
{
    public List<CargoItemSaveData> inventoryItems = new List<CargoItemSaveData>();
    public int currentCargoAmount;
    public int maxCargoCount;
}

[Serializable]
public class TrainSaveFile
{
    public List<TrainSaveData> train = new List<TrainSaveData>();
}

[Serializable]
public class CurrencySaveFile
{
    public List<CurrencySaveData> currency = new List<CurrencySaveData>();
}

[Serializable]
public class PassangerSaveFile
{
    public List<PassangerSaveData> passangers = new List<PassangerSaveData>();
}

[Serializable]
public class DayNightSaveFile
{
    public List<DayNightSaveData> dayNight = new List<DayNightSaveData>();
}

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    CitiesSaveFile cityFile = new CitiesSaveFile();
    InventorySaveFile inventoryFile = new InventorySaveFile();
    TrainSaveFile trainFile = new TrainSaveFile();
    CurrencySaveFile currencyFile = new CurrencySaveFile();
    UpgradeSaveFile upgradeFile = new UpgradeSaveFile();
    PassangerSaveFile passangerFile = new PassangerSaveFile();
    DayNightSaveFile dayNightFile = new DayNightSaveFile();
    Dictionary<string, CitySaveData> cityDict = new Dictionary<string, CitySaveData>();
    string citiesPath;
    string inventoryPath;
    string trainPath;
    string currencyPath;
    string upgradePath;
    string passangerPath;
    string dayNightPath;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        citiesPath = Path.Combine(Application.persistentDataPath, "cities.json");
        inventoryPath = Path.Combine(Application.persistentDataPath, "inventory.json");
        trainPath = Path.Combine(Application.persistentDataPath, "train.json");
        currencyPath = Path.Combine(Application.persistentDataPath, "currency.json");
        upgradePath = Path.Combine(Application.persistentDataPath, "upgrades.json");
        passangerPath = Path.Combine(Application.persistentDataPath, "passanger.json");
        dayNightPath = Path.Combine(Application.persistentDataPath, "daynight.json");
        LoadFromDisk();
    }

    void LoadFromDisk()
    {
        cityDict.Clear();
        cityFile = new CitiesSaveFile();
        inventoryFile = new InventorySaveFile();
        trainFile = new TrainSaveFile();
        currencyFile = new CurrencySaveFile();
        upgradeFile = new UpgradeSaveFile();
        passangerFile = new PassangerSaveFile();
        dayNightFile = new DayNightSaveFile();

        if (File.Exists(citiesPath))
        {
            string json = File.ReadAllText(citiesPath);
            CitiesSaveFile loaded = JsonUtility.FromJson<CitiesSaveFile>(json);
            if (loaded != null)
            {
                cityFile = loaded;
                if (cityFile.cities != null)
                {
                    foreach (CitySaveData city in cityFile.cities)
                        cityDict[city.cityName] = city;
                }
            }
        }
        else
        {
            Debug.Log("[SaveSystem] No cities save file found, starting fresh.");
        }

        if (File.Exists(inventoryPath))
        {
            string json = File.ReadAllText(inventoryPath);
            InventorySaveFile loaded = JsonUtility.FromJson<InventorySaveFile>(json);
            if (loaded != null)
                inventoryFile = loaded;
        }
        else
        {
            Debug.Log("[SaveSystem] No inventory save file found, starting fresh.");
        }

        if (File.Exists(trainPath))
        {
            string json = File.ReadAllText(trainPath);
            TrainSaveFile loaded = JsonUtility.FromJson<TrainSaveFile>(json);
            if (loaded != null)
                trainFile = loaded;
        }
        else
        {
            Debug.Log("[SaveSystem] No train save file found, starting fresh.");
        }

        if (File.Exists(currencyPath))
        {
            string json = File.ReadAllText(currencyPath);
            CurrencySaveFile loaded = JsonUtility.FromJson<CurrencySaveFile>(json);
            if (loaded != null)
                currencyFile = loaded;
        }
        else
        {
            Debug.Log("[SaveSystem] No currency save file found, starting fresh.");
        }

        if (File.Exists(upgradePath))
        {
            string json = File.ReadAllText(upgradePath);
            UpgradeSaveFile loaded = JsonUtility.FromJson<UpgradeSaveFile>(json);
            if (loaded != null)
                upgradeFile = loaded;
        }
        else
        {
            Debug.Log("[SaveSystem] No upgrade save file found, starting fresh.");
        }

        if (File.Exists(passangerPath))
        {
            string json = File.ReadAllText(passangerPath);
            PassangerSaveFile loaded = JsonUtility.FromJson<PassangerSaveFile>(json);
            if (loaded != null)
                passangerFile = loaded;
        }
        else
        {
            Debug.Log("[SaveSystem] No passanger save file found, starting fresh.");
        }

        if (File.Exists(dayNightPath))
        {
            string json = File.ReadAllText(dayNightPath);
            DayNightSaveFile loaded = JsonUtility.FromJson<DayNightSaveFile>(json);
            if (loaded != null)
                dayNightFile = loaded;
        }
        else
        {
            Debug.Log("[SaveSystem] No day night save file found, starting fresh.");
        }
    }

    public void SaveToDisk()
    {
        cityFile.cities = new List<CitySaveData>(cityDict.Values);
        File.WriteAllText(citiesPath, JsonUtility.ToJson(cityFile, true));
        File.WriteAllText(inventoryPath, JsonUtility.ToJson(inventoryFile, true));
        File.WriteAllText(trainPath, JsonUtility.ToJson(trainFile, true));
        File.WriteAllText(currencyPath, JsonUtility.ToJson(currencyFile, true));
        File.WriteAllText(upgradePath, JsonUtility.ToJson(upgradeFile, true));
        File.WriteAllText(passangerPath, JsonUtility.ToJson(passangerFile, true));
        File.WriteAllText(dayNightPath, JsonUtility.ToJson(dayNightFile, true));
    }

    public void DeleteSaveFile()
    {
        cityDict.Clear();
        cityFile = new CitiesSaveFile();
        inventoryFile = new InventorySaveFile();
        trainFile = new TrainSaveFile();
        currencyFile = new CurrencySaveFile();
        upgradeFile = new UpgradeSaveFile();
        passangerFile = new PassangerSaveFile();
        dayNightFile = new DayNightSaveFile();

        if (File.Exists(citiesPath))
            File.Delete(citiesPath);
        if (File.Exists(inventoryPath))
            File.Delete(inventoryPath);
        if (File.Exists(trainPath))
            File.Delete(trainPath);
        if (File.Exists(currencyPath))
            File.Delete(currencyPath);
        if (File.Exists(upgradePath))
            File.Delete(upgradePath);
        if (File.Exists(passangerPath))
            File.Delete(passangerPath);
        if (File.Exists(dayNightPath))
            File.Delete(dayNightPath);
    }

    // --- City ---

    public CitySaveData GetCityData(string cityName)
    {
        cityDict.TryGetValue(cityName, out CitySaveData data);
        return data;
    }

    public void SetCityData(CitySaveData data)
    {
        cityDict[data.cityName] = data;
    }

    public bool HasCityData(string cityName)
    {
        return cityDict.ContainsKey(cityName);
    }

    public string GetResetTime(string cityName)
    {
        if (cityDict.TryGetValue(cityName, out CitySaveData data))
            return data.resetTime;
        return null;
    }

    public void SetResetTime(string cityName, string resetTime)
    {
        if (!cityDict.ContainsKey(cityName))
            cityDict[cityName] = new CitySaveData { cityName = cityName };

        cityDict[cityName].resetTime = resetTime;
    }

    public void DeleteCityData(string cityName)
    {
        cityDict.Remove(cityName);
    }

    // --- Inventory ---

    public List<CargoItemSaveData> GetInventory()
    {
        return inventoryFile.inventoryItems ?? new List<CargoItemSaveData>();
    }

    public int GetCurrentCargoAmount()
    {
        return inventoryFile.currentCargoAmount;
    }

    public int GetMaxCargoCount()
    {
        return inventoryFile.maxCargoCount;
    }

    public void SetInventory(List<CargoItemSaveData> items, int cargoAmount, int maxCargoCount)
    {
        inventoryFile.inventoryItems = items;
        inventoryFile.currentCargoAmount = cargoAmount;
        inventoryFile.maxCargoCount = maxCargoCount;
    }

    // --- Train ---

    public TrainSaveData GetTrainData()
    {
        return trainFile.train.Count > 0 ? trainFile.train[0] : null;
    }

    public void SetTrainData(TrainSaveData data)
    {
        trainFile.train.Clear();
        trainFile.train.Add(data);
    }

    // --- Currency ---

    public CurrencySaveData GetCurrencyData()
    {
        return currencyFile.currency.Count > 0 ? currencyFile.currency[0] : null;
    }

    public void SetCurrencyData(CurrencySaveData data)
    {
        currencyFile.currency.Clear();
        currencyFile.currency.Add(data);
    }

    // --- Upgrades ---

    public UpgradeSaveData GetUpgradeData()
    {
        return upgradeFile.upgrades.Count > 0 ? upgradeFile.upgrades[0] : null;
    }

    public void SetUpgradeData(UpgradeSaveData data)
    {
        upgradeFile.upgrades.Clear();
        upgradeFile.upgrades.Add(data);
    }

    // --- Passangers ---

    public PassangerSaveData GetPassangerData()
    {
        return passangerFile.passangers.Count > 0 ? passangerFile.passangers[0] : null;
    }

    public void SetPassangerData(PassangerSaveData data)
    {
        passangerFile.passangers.Clear();
        passangerFile.passangers.Add(data);
    }

    // --- Day Night ---

    public DayNightSaveData GetDayNightData()
    {
        return dayNightFile.dayNight.Count > 0 ? dayNightFile.dayNight[0] : null;
    }

    public void SetDayNightData(DayNightSaveData data)
    {
        dayNightFile.dayNight.Clear();
        dayNightFile.dayNight.Add(data);
    }
}
