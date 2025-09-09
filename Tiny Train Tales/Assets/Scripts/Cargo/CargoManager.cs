using System.Collections.Generic;
using UnityEngine;

public class CargoManager : MonoBehaviour
{
    [SerializeField] int maxCargoAmount;
    [SerializeField] Sprite[] cargoTypesSprites;

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
}
