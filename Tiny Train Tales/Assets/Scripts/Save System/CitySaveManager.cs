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
    public List<CargoItemSaveData> inventoryItems = new List<CargoItemSaveData>();
    public int currentCargoAmount;
}

public class CitySaveManager : MonoBehaviour
{
    public static CitySaveManager Instance { get; private set; }

    AllCitiesSaveFile saveFile = new AllCitiesSaveFile();
    Dictionary<string, CitySaveData> cityDict = new Dictionary<string, CitySaveData>();
    string savePath;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        savePath = Path.Combine(Application.persistentDataPath, "cities.json");
        LoadFromDisk();
    }

    void LoadFromDisk()
    {
        cityDict.Clear();
        saveFile = new AllCitiesSaveFile();

        if (!File.Exists(savePath))
        {
            Debug.Log("[CitySaveManager] No save file found, starting fresh.");
            return;
        }

        string json = File.ReadAllText(savePath);
        AllCitiesSaveFile loaded = JsonUtility.FromJson<AllCitiesSaveFile>(json);
        if (loaded == null) return;

        saveFile = loaded;

        if (saveFile.cities != null)
        {
            foreach (CitySaveData city in saveFile.cities)
                cityDict[city.cityName] = city;
        }

        Debug.Log($"[CitySaveManager] Loaded {cityDict.Count} cities and {saveFile.inventoryItems?.Count ?? 0} inventory items from {savePath}");
    }

    public void SaveToDisk()
    {
        saveFile.cities = new List<CitySaveData>(cityDict.Values);
        File.WriteAllText(savePath, JsonUtility.ToJson(saveFile, true));
        Debug.Log($"[CitySaveManager] Saved {saveFile.cities.Count} cities and {saveFile.inventoryItems?.Count ?? 0} inventory items to {savePath}");
    }

    public void DeleteSaveFile()
    {
        cityDict.Clear();
        saveFile = new AllCitiesSaveFile();
        if (File.Exists(savePath))
            File.Delete(savePath);
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
        return saveFile.inventoryItems ?? new List<CargoItemSaveData>();
    }

    public int GetCurrentCargoAmount()
    {
        return saveFile.currentCargoAmount;
    }

    public void SetInventory(List<CargoItemSaveData> items, int cargoAmount)
    {
        saveFile.inventoryItems = items;
        saveFile.currentCargoAmount = cargoAmount;
    }
}
