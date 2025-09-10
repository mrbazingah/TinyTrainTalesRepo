using System.Collections.Generic;
using UnityEngine;

public class CargoManager : MonoBehaviour
{
    [SerializeField] int maxCragoTypesPerCity;
    [SerializeField] int maxCargoAmount;
    [Space]
    [SerializeField] Sprite[] cargoTypesSprites;
    [SerializeField] string[] cargoTypesNames;
    [Space]
    [SerializeField] GameObject cargoItemPrefab;

    int currentCargoAmount;

    List<GameObject> cargoTypes = new List<GameObject>();

    public void AddCargo()
    {
        currentCargoAmount++;
        if (currentCargoAmount > maxCargoAmount)
        {
            currentCargoAmount = maxCargoAmount;
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

    public GameObject CreateCargoItem()
    {
        GameObject cargoItem = Instantiate(cargoItemPrefab);
        CargoItem cargoItemScript = cargoItem.GetComponent<CargoItem>();

        int randomIndex = Random.Range(0, cargoTypesSprites.Length);
        cargoItemScript.SetItemIcon(cargoTypesSprites[randomIndex]);
        cargoItemScript.SetItemName(cargoTypesNames[randomIndex]);

        return cargoItem;
    }
}
