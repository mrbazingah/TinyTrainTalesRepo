using System.Collections.Generic;
using UnityEngine;

public class CargoManager : MonoBehaviour
{
    [Header("City Cargo Parameters")]
    [SerializeField] int minSpawnCount;
    [SerializeField] int maxSpawnCount;
    [SerializeField] int minCityCargoAmount;
    [SerializeField] int maxCityCargoAmount;
    [SerializeField] float cityCargoResetTime; // in hours
    [Header("Cargo Intiate")]
    [SerializeField] Sprite[] cargoItemsSprites;
    [SerializeField] string[] cargoItemsNames;
    [SerializeField] float[] cargoItemPrices;
    [Space]
    [SerializeField] GameObject cargoItemPrefab;
    [SerializeField] int maxCargoCount;
    [Header("Visuals")]
    [SerializeField] GameObject cargoItemParent;
    [SerializeField] Vector2 startPos;
    [SerializeField] float yOffset;

    int currentCargCount;
    string currentSaveString;

    List<GameObject> cargoItemList = new List<GameObject>();

    void Start()
    {
        LoadCargoItems();
    }

    void LoadCargoItems()
    {
        currentCargCount = PlayerPrefs.GetInt("CurrentCargoAmount");

        if (PlayerPrefs.HasKey("NumberOfCargoItems"))
        {
            int numberOfCargoItems = PlayerPrefs.GetInt("NumberOfCargoItems");
            for (int i = 0; i < numberOfCargoItems; i++)
            {
                currentSaveString = PlayerPrefs.GetString("SaveString" + i.ToString());
                CreateCargoItemForInventory(null);
            }
        }

        if (currentCargCount == 0 && cargoItemList.Count > 0)
        {
            for (int i = 0; i < cargoItemList.Count; i++)
            {
                currentCargCount += cargoItemList[i].GetComponent<CargoItem>().GetItemCount();
            }
        }
    }

    public void AddCargo(GameObject newItem)
    {
        CargoItem newItemScript = newItem.GetComponent<CargoItem>();
        if (newItemScript.GetItemCount() <= 0) { return; }

        currentCargCount++;
        if (currentCargCount > maxCargoCount)
        {
            currentCargCount = maxCargoCount;
        }

        bool hasItem = false;
        int index = 0;
        for (int i = 0; i < cargoItemList.Count; i++)
        {
            if (cargoItemList[i].GetComponent<CargoItem>().GetItemName() == newItem.GetComponent<CargoItem>().GetItemName())
            {
                hasItem = true;
                index = i;
                break;
            }

            hasItem = false;
        }

        if (hasItem)
        {
            cargoItemList[index].GetComponent<CargoItem>().AddCount(1);
            newItemScript.AddCount(-1);

            if (newItemScript.GetItemCount() <= 0)
            {
                newItemScript.TurnOffBuyOption();
            }
        }
        else
        {
            CreateCargoItemForInventory(newItem);
        }
    }

    void CreateCargoItemForInventory(GameObject newItem)
    {
        CargoItem newItemScript;

        if (newItem == null)
        {
            newItem = Instantiate(cargoItemPrefab);
            newItemScript = newItem.GetComponent<CargoItem>();

            newItemScript.SetSaveString(currentSaveString);
            newItemScript.SetIsInCity(false, "");
            newItemScript.LoadItemPlayerPrefs();

            cargoItemList.Add(newItem);
        }
        else
        {
            newItemScript = newItem.GetComponent<CargoItem>();
            newItemScript.AddCount(-1);

            GameObject newSpawnedItem = Instantiate(newItem);
            CargoItem cargoItemScript = newSpawnedItem.GetComponent<CargoItem>();

            cargoItemScript.SetItemCount(1);
            cargoItemScript.SetItemName(newItemScript.GetItemName(), "");
            cargoItemScript.SetIsInCity(false, "");

            cargoItemList.Add(newSpawnedItem);
            cargoItemScript.ChangeUI();
        }

        Vector2 lastPos = Vector2.zero;

        for (int i = 0; i < cargoItemList.Count; i++)
        {
            cargoItemList[i].transform.SetParent(cargoItemParent.transform);
            cargoItemList[i].transform.localPosition = Vector2.zero;
            cargoItemList[i].transform.localScale = new Vector3(2, 2, 1);

            if (i == 0)
            {
                cargoItemList[i].transform.localPosition = startPos;
            }
            else
            {
                cargoItemList[i].transform.localPosition = new Vector2(lastPos.x, lastPos.y - yOffset);
            }

            lastPos = cargoItemList[i].transform.localPosition;
        }

    }

    public GameObject CreateCargoItemForCity(string cityName)
    {
        GameObject cargoItem = Instantiate(cargoItemPrefab);
        CargoItem cargoItemScript = cargoItem.GetComponent<CargoItem>();

        int randomIndex = Random.Range(0, cargoItemsSprites.Length);
        cargoItemScript.SetItemIcon(cargoItemsSprites[randomIndex]);
        cargoItemScript.SetItemName(cargoItemsNames[randomIndex], cityName);
        cargoItemScript.SetItemPrice(cargoItemPrices[randomIndex]);

        int randomCount = Random.Range(minSpawnCount, maxSpawnCount + 1);
        cargoItemScript.SetItemCount(randomCount);

        string saveString = cityName + " " + cargoItemsNames[randomIndex];
        cargoItemScript.SetSaveString(saveString);

        return cargoItem;
    }

    public GameObject CreateSavedCargoItemForCity(string saveString, string cityName)
    {
        GameObject cargoItem = Instantiate(cargoItemPrefab);
        CargoItem cargoItemScript = cargoItem.GetComponent<CargoItem>();

        cargoItemScript.SetSaveString(saveString);
        cargoItemScript.SetIsInCity(true, cityName);
        cargoItemScript.LoadItemPlayerPrefs();

        return cargoItem;
    }

    public int GetCityMinCargoAmount()
    {
        return minCityCargoAmount;
    }

    public int GetCityMaxCargoAmount()
    {
        return maxCityCargoAmount;
    }

    public Sprite[] GetCargoItemsSprites()
    {
        return cargoItemsSprites;
    }

    public void SaveCargo()
    {
        PlayerPrefs.SetInt("NumberOfCargoItems", cargoItemList.Count);
        PlayerPrefs.SetInt("CurrentCargoAmount", currentCargCount);

        for (int i = 0; i < cargoItemList.Count; i++)
        {
            CargoItem cargoItemScript = cargoItemList[i].GetComponent<CargoItem>();
            string saveString = cargoItemScript.GetSaveString();
            PlayerPrefs.SetString("SaveString" + i.ToString(), saveString);

            cargoItemScript.SaveCargoItem();
        }
    }

    public float GetCityCargoResetTime()
    {
        return cityCargoResetTime;
    }
}
