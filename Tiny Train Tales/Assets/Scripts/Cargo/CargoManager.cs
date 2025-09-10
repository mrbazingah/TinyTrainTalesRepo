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

    int currentCargoAmount;

    List<GameObject> cargoItems = new List<GameObject>();

    public void AddCargo(GameObject newItem)
    {
        currentCargoAmount++;
        if (currentCargoAmount > maxCargoCount)
        {
            currentCargoAmount = maxCargoCount;
        }

        bool hasItem = false;
        int index = 0;
        for (int i = 0; i < cargoItems.Count; i++)
        {
            for (int ii = 0; ii < cargoItemsSprites.Length; ii++)
            {
                if (cargoItems[i].GetComponent<CargoItem>().GetItemIcon().sprite == cargoItemsSprites[ii])
                {
                    hasItem = true;
                    index = i;
                    break;
                }
            }

            if (hasItem) break;
            hasItem = false;
        }

        if (hasItem)
        {
            CargoItem newItemScript = newItem.GetComponent<CargoItem>();

            cargoItems[index].GetComponent<CargoItem>().AddCount(1);
            newItemScript.AddCount(-1);

            if (newItemScript.GetItemCount() <= 0)
            {
                newItemScript.TurnOffBuyOption();
            }
        }
    }

    public GameObject CreateCargoItem(string cityName)
    {
        GameObject cargoItem = Instantiate(cargoItemPrefab);
        CargoItem cargoItemScript = cargoItem.GetComponent<CargoItem>();

        int randomIndex = Random.Range(0, cargoItemsSprites.Length);
        cargoItemScript.SetItemIcon(cargoItemsSprites[randomIndex]);
        cargoItemScript.SetItemName(cargoItemsNames[randomIndex] + " " + cityName);

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
}
