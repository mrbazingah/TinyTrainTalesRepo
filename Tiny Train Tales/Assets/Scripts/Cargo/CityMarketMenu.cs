using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CityMarketMenu : MonoBehaviour
{
    [SerializeField] MenuAnimationY menuAnimY;
    [SerializeField] GameObject marketCargoItemParent;
    [SerializeField] GameObject inventoryCargoItemParent;
    [SerializeField] Vector2 startPos;
    [SerializeField] float yOffset;
    [Space]
    [SerializeField] GameObject[] buyAndSellUI;
    [SerializeField] TextMeshProUGUI totalText;

    List<GameObject> marketCargoItems = new List<GameObject>();
    List<GameObject> inventoryCargoItems = new List<GameObject>();

    List<GameObject> selectedCargoItems = new List<GameObject>();
    List<float> selectedCargoItemCounts = new List<float>();

    GameObject lastSelectedCargoItem;
    float totalCount;

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

    public void SwitchBuyAndSell(GameObject cargoItem)
    {
        if (lastSelectedCargoItem != null && lastSelectedCargoItem != cargoItem)
        {
            lastSelectedCargoItem.GetComponent<CargoItem>().OnDeselect();
        }

        for (int i = 0; i < buyAndSellUI.Length; i++)
        {
            bool isMarketItem = cargoItem.GetComponent<CargoItem>().GetIsInCity();

            TextMeshProUGUI text = buyAndSellUI[i].GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = isMarketItem ? "Buy" : "Sell";
            }
            else
            {
                if ((!isMarketItem && buyAndSellUI[i].transform.localScale.x == 1f) || (isMarketItem && buyAndSellUI[i].transform.localScale.x == -1f)) { continue; }

                buyAndSellUI[i].transform.localPosition = new Vector2(buyAndSellUI[i].transform.localPosition.x * -1, buyAndSellUI[i].transform.localPosition.y);
                buyAndSellUI[i].transform.localScale = new Vector3(buyAndSellUI[i].transform.localScale.x * -1, buyAndSellUI[i].transform.localScale.y, buyAndSellUI[i].transform.localScale.z);
            }
        }

        lastSelectedCargoItem = cargoItem;
    }

    public void CommitOne()
    {
        bool found = false;
        int index = 0;
        for (int i = 0; i < selectedCargoItems.Count; i++)
        {
            if (selectedCargoItems[i] == lastSelectedCargoItem)
            {
                found = true;
                index = i;
                break;
            }
        }

        if (!found)
        {
            selectedCargoItems.Add(lastSelectedCargoItem);
            selectedCargoItemCounts.Add(0);
            index = selectedCargoItems.Count - 1;
        }

        CargoItem cargoItemScript = lastSelectedCargoItem.GetComponent<CargoItem>();
        if (cargoItemScript == null || cargoItemScript.GetItemCount() == 0) { return; }

        int currentTotal = cargoItemScript.GetItemCount() - int.Parse(cargoItemScript.GetItemCountText().text);
        selectedCargoItemCounts[index] = currentTotal;

        string tempCount = (cargoItemScript.GetItemCount() - currentTotal).ToString();
        cargoItemScript.SetTempCountText(tempCount);

        totalCount++;

        float totalPrice = 0;
        for (int i = 0; i < selectedCargoItems.Count; i++)
        {
            totalPrice += selectedCargoItemCounts[i] * selectedCargoItems[i].GetComponent<CargoItem>().GetItemPrice();
        }

        totalText.text = totalPrice.ToString();
    }

    public void CommitAll()
    {
        bool found = false;
        int index = 0;
        for (int i = 0; i < selectedCargoItems.Count; i++)
        {
            if (selectedCargoItems[i] == lastSelectedCargoItem)
            {
                found = true;
                index = i;
                break;
            }
        }

        if (!found)
        {
            selectedCargoItems.Add(lastSelectedCargoItem);
            index = selectedCargoItems.Count - 1;
        }

        CargoItem cargoItemScript = lastSelectedCargoItem.GetComponent<CargoItem>();
        if (cargoItemScript == null || cargoItemScript.GetItemCount() == 0) { return; }

        int currentTotal = cargoItemScript.GetItemCount();
        selectedCargoItemCounts[index] = currentTotal;

        string tempCount = currentTotal.ToString();
        cargoItemScript.SetTempCountText(tempCount);

        totalCount += currentTotal;

        float totalPrice = 0;
        for (int i = 0; i < selectedCargoItems.Count; i++)
        {
            totalPrice += selectedCargoItemCounts[i] * selectedCargoItems[i].GetComponent<CargoItem>().GetItemPrice();
        }

        totalText.text = totalPrice.ToString();
    }
}
