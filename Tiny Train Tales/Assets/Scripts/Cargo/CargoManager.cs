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
        List<CargoItemSaveData> inventory = SaveSystem.Instance.GetInventory();
        currentCargoCount = SaveSystem.Instance.GetCurrentCargoAmount();
        int savedMaxCargo = SaveSystem.Instance.GetMaxCargoCount();
        if (savedMaxCargo > 0) maxCargoCount = savedMaxCargo;

        Vector2 lastPos = Vector2.zero;
        for (int i = 0; i < inventory.Count; i++)
        {
            CargoItemSaveData itemData = inventory[i];

            GameObject newItem = Instantiate(cargoItemPrefab);
            CargoItem itemScript = newItem.GetComponent<CargoItem>();

            itemScript.SetIsInCity(false, "");
            itemScript.SetItemName(itemData.itemName, "");
            itemScript.SetItemCount(itemData.itemCount);
            itemScript.SetItemPrice(itemData.itemPrice);
            itemScript.SetItemIcon(cargoItemsSprites[itemData.spriteIndex]);
            itemScript.SetPurchasePrice(itemData.purchasePrice);
            itemScript.SetSaveString(itemData.itemName);

            newItem.transform.SetParent(cargoItemParent.transform);
            newItem.transform.localScale = Vector3.one;
            newItem.transform.localPosition = i == 0 ? startPos : new Vector2(lastPos.x, lastPos.y - yOffset);
            lastPos = newItem.transform.localPosition;

            cargoItemList.Add(newItem);
        }

        if (currentCargoCount == 0 && cargoItemList.Count > 0)
        {
            for (int i = 0; i < cargoItemList.Count; i++)
                currentCargoCount += cargoItemList[i].GetComponent<CargoItem>().GetItemCount();
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
            //New Cargo Item Created
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
            cargoItemScript.SetPurchasePrice(price); 

            cargoItemList.Add(newSpawnedItem);
            cargoItemScript.ChangeUI();
        }

        Vector2 lastPos = Vector2.zero;

        for (int i = 0; i < cargoItemList.Count; i++)
        {
            cargoItemList[i].transform.SetParent(cargoItemParent.transform);
            cargoItemList[i].transform.localPosition = Vector2.zero;
            cargoItemList[i].transform.localScale = new Vector3(1, 1, 1);

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

    public GameObject CreateSavedCargoItemForCity(CargoItemSaveData itemData, string cityName)
    {
        GameObject cargoItem = Instantiate(cargoItemPrefab);
        CargoItem cargoItemScript = cargoItem.GetComponent<CargoItem>();

        cargoItemScript.SetIsInCity(true, cityName);
        cargoItemScript.SetItemName(itemData.itemName, cityName);
        cargoItemScript.SetItemCount(itemData.itemCount);
        cargoItemScript.SetItemPrice(itemData.itemPrice);
        cargoItemScript.SetItemIcon(cargoItemsSprites[itemData.spriteIndex]);
        cargoItemScript.SetPurchasePrice(itemData.purchasePrice);
        cargoItemScript.SetSaveString(cityName + " " + itemData.itemName);

        return cargoItem;
    }

    public void AddToMaxCargo(int addedMaxCargo)
    {
        maxCargoCount += addedMaxCargo;
        cargoCountText.text = currentCargoCount.ToString() + maxCargoCount.ToString();
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
        List<CargoItemSaveData> inventory = new List<CargoItemSaveData>();

        for (int i = 0; i < cargoItemList.Count; i++)
        {
            CargoItem itemScript = cargoItemList[i].GetComponent<CargoItem>();

            CargoItemSaveData itemData = new CargoItemSaveData();
            itemData.itemName = itemScript.GetItemName();
            itemData.itemCount = itemScript.GetItemCount();
            itemData.itemPrice = itemScript.GetItemPrice();
            itemData.purchasePrice = itemScript.GetPurchasePrice();

            for (int j = 0; j < cargoItemsSprites.Length; j++)
            {
                if (itemScript.GetItemIcon().sprite == cargoItemsSprites[j])
                {
                    itemData.spriteIndex = j;
                    break;
                }
            }

            inventory.Add(itemData);
        }

        SaveSystem.Instance.SetInventory(inventory, currentCargoCount, maxCargoCount);
    }
}
