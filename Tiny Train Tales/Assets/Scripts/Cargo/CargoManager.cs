using System.Collections.Generic;
using UnityEngine;

public class CargoManager : MonoBehaviour
{
    [Header("City Cargo Parameters")]
    [SerializeField] int minSpawnCount;
    [SerializeField] int maxSpawnCount;
    [SerializeField] int minCityCargoAmount;
    [SerializeField] int maxCityCargoAmount;
    [Header("Cargo Intiate")]
    [SerializeField] Sprite[] cargoItemsSprites;
    [SerializeField] string[] cargoItemsNames;
    [Space]
    [SerializeField] GameObject cargoItemPrefab;
    [SerializeField] int maxCargoCount;
    [Header("Visuals")]
    [SerializeField] GameObject cargoItemParent;
    [SerializeField] Vector2 startPos;
    [SerializeField] float yOffset;

    int currentCargoAmount;
    string currentSaveString;

    List<GameObject> cargoItemList = new List<GameObject>();

    void Start()
    {
        LoadCargoItems();
    }

    void LoadCargoItems()
    {
        if (PlayerPrefs.HasKey("NumberOfCargoItems"))
        {
            int numberOfCargoItems = PlayerPrefs.GetInt("NumberOfCargoItems");
            for (int i = 0; i < numberOfCargoItems; i++)
            {
                currentSaveString = PlayerPrefs.GetString("SaveString" + i.ToString());
                CreateCargoItemForInventory(null);
            }
        }
    }

    public void AddCargo(GameObject newItem)
    {
        currentCargoAmount++;
        if (currentCargoAmount > maxCargoCount)
        {
            currentCargoAmount = maxCargoCount;
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
            CargoItem newItemScript = newItem.GetComponent<CargoItem>();

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
            newItemScript.SetIsInCity(false);
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

        int randomCount = Random.Range(minSpawnCount, maxSpawnCount + 1);
        cargoItemScript.SetItemCount(randomCount);

        return cargoItem;
    }

    public GameObject CreateSavedCargoItemForCity(string saveString, string cityName)
    {
        GameObject cargoItem = Instantiate(cargoItemPrefab);
        CargoItem cargoItemScript = cargoItem.GetComponent<CargoItem>();

        cargoItemScript.SetSaveString(saveString);
        cargoItemScript.SetIsInCity(true);
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

    public void SaveCargo()
    {
        PlayerPrefs.SetInt("NumberOfCargoItems", cargoItemList.Count);

        for (int i = 0; i < cargoItemList.Count; i++)
        {
            CargoItem cargoItemScript = cargoItemList[i].GetComponent<CargoItem>();
            string saveString = cargoItemScript.GetSaveString();
            PlayerPrefs.SetString("SaveString" + i.ToString(), saveString);
        }
    }
}
