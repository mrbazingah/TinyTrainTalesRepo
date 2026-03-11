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
    public float savedCoins;
    public float savedGems;
    public float savedNetworth;
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

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    CitiesSaveFile cityFile = new CitiesSaveFile();
    InventorySaveFile inventoryFile = new InventorySaveFile();
    TrainSaveFile trainFile = new TrainSaveFile();
    CurrencySaveFile currencyFile = new CurrencySaveFile();
    Dictionary<string, CitySaveData> cityDict = new Dictionary<string, CitySaveData>();
    string citiesPath;
    string inventoryPath;
    string trainPath;
    string currencyPath;

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
        LoadFromDisk();
    }

    void LoadFromDisk()
    {
        cityDict.Clear();
        cityFile = new CitiesSaveFile();
        inventoryFile = new InventorySaveFile();
        trainFile = new TrainSaveFile();
        currencyFile = new CurrencySaveFile();

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

        Debug.Log($"[SaveSystem] Loaded {cityDict.Count} cities, {inventoryFile.inventoryItems?.Count ?? 0} inventory items, {trainFile.train?.Count ?? 0} train values, {currencyFile.currency?.Count ?? 0} currency values");
    }

    public void SaveToDisk()
    {
        cityFile.cities = new List<CitySaveData>(cityDict.Values);
        File.WriteAllText(citiesPath, JsonUtility.ToJson(cityFile, true));
        File.WriteAllText(inventoryPath, JsonUtility.ToJson(inventoryFile, true));
        File.WriteAllText(trainPath, JsonUtility.ToJson(trainFile, true));
        File.WriteAllText(currencyPath, JsonUtility.ToJson(currencyFile, true));
        Debug.Log($"[SaveSystem] Saved {cityDict.Count} cities, {inventoryFile.inventoryItems?.Count ?? 0} inventory items, {trainFile.train?.Count ?? 0} train values, {currencyFile.currency?.Count ?? 0} currency values");
    }

    public void DeleteSaveFile()
    {
        cityDict.Clear();
        cityFile = new CitiesSaveFile();
        inventoryFile = new InventorySaveFile();
        trainFile = new TrainSaveFile();
        currencyFile = new CurrencySaveFile();

        if (File.Exists(citiesPath))
            File.Delete(citiesPath);
        if (File.Exists(inventoryPath))
            File.Delete(inventoryPath);
        if (File.Exists(trainPath))
            File.Delete(trainPath);
        if (File.Exists(currencyPath))
            File.Delete(currencyPath);
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

    public void SetInventory(List<CargoItemSaveData> items, int cargoAmount)
    {
        inventoryFile.inventoryItems = items;
        inventoryFile.currentCargoAmount = cargoAmount;
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
}
