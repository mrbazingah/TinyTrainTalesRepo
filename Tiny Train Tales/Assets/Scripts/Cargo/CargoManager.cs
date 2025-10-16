using System.Collections.Generic;
using TMPro;
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
    [SerializeField] TextMeshProUGUI cargoCountText;
    [SerializeField] GameObject cargoItemParent;
    [SerializeField] Vector2 startPos;
    [SerializeField] float yOffset;

    int currentCargoCount;
    string currentSaveString;

    List<GameObject> cargoItemList = new List<GameObject>();

    CityMarketMenu cityMarketMenu;

    void Awake()
    {
        cityMarketMenu = FindObjectOfType<CityMarketMenu>();
    }

    void Start()
    {
        LoadCargoItems();
    }

    void LoadCargoItems()
    {
        currentCargoCount = PlayerPrefs.GetInt("CurrentCargoAmount");

        if (PlayerPrefs.HasKey("NumberOfCargoItems"))
        {
            int numberOfCargoItems = PlayerPrefs.GetInt("NumberOfCargoItems");
            for (int i = 0; i < numberOfCargoItems; i++)
            {
                currentSaveString = PlayerPrefs.GetString("SaveString" + i.ToString());
                CreateCargoItemForInventory(null, 0, 0);
            }
        }

        if (currentCargoCount == 0 && cargoItemList.Count > 0)
        {
            for (int i = 0; i < cargoItemList.Count; i++)
            {
                currentCargoCount += cargoItemList[i].GetComponent<CargoItem>().GetItemCount();
            }
        }
    }

    public void AddCargo(GameObject newItem, int count, float price)
    {
        CargoItem newItemScript = newItem.GetComponent<CargoItem>();
        if (newItemScript.GetItemCount() <= 0) { return; }

        currentCargoCount += count;
        if (currentCargoCount > maxCargoCount)
        {
            currentCargoCount = maxCargoCount;
        }

        bool hasItem = false;
        int index = 0;
        for (int i = 0; i < cargoItemList.Count; i++)
        {
            if (cargoItemList[i].GetComponent<CargoItem>().GetItemName() == newItemScript.GetItemName())
            {
                hasItem = true;
                index = i;
                break;
            }
        }

        if (hasItem)
        {
            CargoItem invItem = cargoItemList[index].GetComponent<CargoItem>();
            invItem.AddCount(count);

            invItem.SetItemPrice(price);
            invItem.SetPurchasePrice(price); 

            newItemScript.AddCount(-count);
        }
        else
        {
            CreateCargoItemForInventory(newItem, count, price);
        }

        cityMarketMenu.ResetInventoryItems();
    }

    public int FindMatch(string itemName)
    {
        int index = -1;
        for (int i = 0; i < cargoItemList.Count; i++)
        {
            if (cargoItemList[i].GetComponent<CargoItem>().GetItemName() == itemName)
            {
                index = i;
            }
        }

        return index;
    }

    public void RemoveCargo(string itemName)
    {
        int index = FindMatch(itemName);

        currentCargoCount -= cargoItemList[index].GetComponent<CargoItem>().GetItemCount();

        Destroy(cargoItemList[index]);
        cargoItemList.RemoveAt(index);

        cityMarketMenu.RemoveInventoryItem(index);
    }

    void CreateCargoItemForInventory(GameObject newItem, int count, float price)
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
            newItemScript.AddCount(count);

            GameObject newSpawnedItem = Instantiate(newItem);
            CargoItem cargoItemScript = newSpawnedItem.GetComponent<CargoItem>();

            cargoItemScript.SetItemCount(count);
            cargoItemScript.SetItemName(newItemScript.GetItemName(), "");
            cargoItemScript.SetIsInCity(false, "");

            cargoItemScript.SetItemPrice(price);
            cargoItemScript.SetPurchasePrice(price); // NEW — track actual price paid

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

    void Update()
    {
        cargoCountText.text = currentCargoCount.ToString() + "/" + maxCargoCount.ToString();
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

    public string[] GetCargoItemsNames()
    {
        return cargoItemsNames;
    }

    public float GetCityCargoResetTime()
    {
        return cityCargoResetTime;
    }

    public List<GameObject> GetCargoItemList()
    {
        return cargoItemList;
    }

    public int GetCurrentCargoCount()
    {
        return currentCargoCount;
    }

    public int GetMaxCargoCount()
    {
        return maxCargoCount;
    }

    public void SaveCargo()
    {
        PlayerPrefs.SetInt("NumberOfCargoItems", cargoItemList.Count);
        PlayerPrefs.SetInt("CurrentCargoAmount", currentCargoCount);

        for (int i = 0; i < cargoItemList.Count; i++)
        {
            CargoItem cargoItemScript = cargoItemList[i].GetComponent<CargoItem>();
            string saveString = cargoItemScript.GetSaveString();
            PlayerPrefs.SetString("SaveString" + i.ToString(), saveString);

            cargoItemScript.SaveCargoItem();
        }
    }
}
