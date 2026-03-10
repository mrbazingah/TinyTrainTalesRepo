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
public class AllCitiesSaveFile
{
    public List<CitySaveData> cities = new List<CitySaveData>();
}

[Serializable]
public class InventorySaveFile
{
    public List<CargoItemSaveData> inventoryItems = new List<CargoItemSaveData>();
    public int currentCargoAmount;
}

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    AllCitiesSaveFile saveFile = new AllCitiesSaveFile();
    InventorySaveFile inventoryFile = new InventorySaveFile();
    Dictionary<string, CitySaveData> cityDict = new Dictionary<string, CitySaveData>();
    string savePath;
    string inventoryPath;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        savePath = Path.Combine(Application.persistentDataPath, "cities.json");
        inventoryPath = Path.Combine(Application.persistentDataPath, "inventory.json");
        LoadFromDisk();
    }

    void LoadFromDisk()
    {
        cityDict.Clear();
        saveFile = new AllCitiesSaveFile();
        inventoryFile = new InventorySaveFile();

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            AllCitiesSaveFile loaded = JsonUtility.FromJson<AllCitiesSaveFile>(json);
            if (loaded != null)
            {
                saveFile = loaded;
                if (saveFile.cities != null)
                {
                    foreach (CitySaveData city in saveFile.cities)
                        cityDict[city.cityName] = city;
                }
            }
        }
        else
        {
            Debug.Log("[CitySaveManager] No cities save file found, starting fresh.");
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
            Debug.Log("[CitySaveManager] No inventory save file found, starting fresh.");
        }

        Debug.Log($"[CitySaveManager] Loaded {cityDict.Count} cities and {inventoryFile.inventoryItems?.Count ?? 0} inventory items.");
    }

    public void SaveToDisk()
    {
        saveFile.cities = new List<CitySaveData>(cityDict.Values);
        File.WriteAllText(savePath, JsonUtility.ToJson(saveFile, true));
        File.WriteAllText(inventoryPath, JsonUtility.ToJson(inventoryFile, true));
        Debug.Log($"[CitySaveManager] Saved {saveFile.cities.Count} cities and {inventoryFile.inventoryItems?.Count ?? 0} inventory items.");
    }

    public void DeleteSaveFile()
    {
        cityDict.Clear();
        saveFile = new AllCitiesSaveFile();
        inventoryFile = new InventorySaveFile();
        if (File.Exists(savePath))
            File.Delete(savePath);
        if (File.Exists(inventoryPath))
            File.Delete(inventoryPath);
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
}
