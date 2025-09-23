using System.Collections.Generic;
using UnityEngine;

public class CityMarketMenu : MonoBehaviour
{
    [SerializeField] MenuAnimationY menuAnimY;
    [SerializeField] GameObject marketCargoItemParent;
    [SerializeField] GameObject inventoryCargoItemParent;
    [SerializeField] Vector2 startPos;
    [SerializeField] float yOffset;

    List<GameObject> marketCargoItems = new List<GameObject>();
    List<GameObject> inventoryCargoItems = new List<GameObject>();

    CargoManager cargoManager;

    void Awake()
    {
        cargoManager = FindObjectOfType<CargoManager>();
    }

    public void OpenCargoMenu()
    {
        menuAnimY.StartAnimation();
    }

    public void SetCargoList(List<GameObject> newMarketItems)
    {
        marketCargoItems = newMarketItems;

        List<GameObject> newInventoryItems = new List<GameObject>();
        for (int i = 0; i < cargoManager.GetCargoItemList().Count; i++)
        {
            GameObject cargoItem = Instantiate(cargoManager.GetCargoItemList()[i]);
            newInventoryItems.Add(cargoItem);
        }

        inventoryCargoItems = newInventoryItems;

        SetUpCargoItems();
    }

    void SetUpCargoItems()
    {
        Vector2 lastPos = Vector2.zero;

        //Market Cargo
        for (int i = 0; i < marketCargoItems.Count; i++)
        {
            marketCargoItems[i].transform.SetParent(marketCargoItemParent.transform);
            marketCargoItems[i].transform.localPosition = Vector2.zero;

            if (i == 0)
            {
                marketCargoItems[i].transform.localPosition = startPos;
            }
            else
            {
                marketCargoItems[i].transform.localPosition = new Vector2(lastPos.x, lastPos.y - yOffset);
            }

            lastPos = marketCargoItems[i].transform.localPosition;
        }

        //Inventory Cargo
        for (int i = 0; i < inventoryCargoItems.Count; i++)
        {
            inventoryCargoItems[i].transform.SetParent(inventoryCargoItemParent.transform);
            inventoryCargoItems[i].transform.localPosition = Vector2.zero;

            if (i == 0)
            {
                inventoryCargoItems[i].transform.localPosition = startPos;
            }
            else
            {
                inventoryCargoItems[i].transform.localPosition = new Vector2(lastPos.x, lastPos.y - yOffset);
            }

            lastPos = inventoryCargoItems[i].transform.localPosition;
        }
    }
}
