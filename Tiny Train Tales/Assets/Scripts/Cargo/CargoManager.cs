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
        if (newItem == null)
        {
            newItem = Instantiate(cargoItemPrefab);
            newItem.GetComponent<CargoItem>().LoadItemPlayerPrefs();
            newItem.GetComponent<CargoItem>().SetIsInCity(false);
        }

        CargoItem newItemScript = newItem.GetComponent<CargoItem>();
        newItemScript.AddCount(-1);

        GameObject newSpawnedItem = Instantiate(newItem);
        CargoItem cargoItemScript = newSpawnedItem.GetComponent<CargoItem>();
        cargoItemList.Add(newSpawnedItem);

        cargoItemScript.SetItemCount(1);
        cargoItemScript.SetItemName(newItemScript.GetItemName(), "");

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

        cargoItemScript.ChangeUI();
    }

    public GameObject CreateCargoItem(string cityName)
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
            cargoItemList[i].GetComponent<CargoItem>().SaveCargoItem();
        }
    }
}
