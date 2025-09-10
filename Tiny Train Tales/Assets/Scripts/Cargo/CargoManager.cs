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
    [SerializeField] Sprite[] cargoTypesSprites;
    [SerializeField] string[] cargoTypesNames;
    [Space]
    [SerializeField] GameObject cargoItemPrefab;
    [SerializeField] int maxCargoCount;

    int currentCargoAmount;

    List<GameObject> cargoTypes = new List<GameObject>();

    public void AddCargo()
    {
        currentCargoAmount++;
        if (currentCargoAmount > maxCargoCount)
        {
            currentCargoAmount = maxCargoCount;
        }

        bool hasItem = false;
        int index = 0;
        for (int i = 0; i < cargoTypes.Count; i++)
        {
            for (int ii = 0; ii < cargoTypesSprites.Length; ii++)
            {
                if (cargoTypes[i].GetComponent<CargoItem>().GetItemIcon().sprite == cargoTypesSprites[ii])
                {
                    hasItem = true;
                    index = i;
                    break;
                }
            }

            if (hasItem) break;
            hasItem = false;
        }

        if (!hasItem)
        {
            //Add object that city created           
        }
        else
        {
            //Has Index
        }
    }

    public GameObject CreateCargoItem(string cityName)
    {
        GameObject cargoItem = Instantiate(cargoItemPrefab);
        CargoItem cargoItemScript = cargoItem.GetComponent<CargoItem>();

        int randomIndex = Random.Range(0, cargoTypesSprites.Length);
        cargoItemScript.SetItemIcon(cargoTypesSprites[randomIndex]);
        cargoItemScript.SetItemName(cargoTypesNames[randomIndex] + " " + cityName);

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
