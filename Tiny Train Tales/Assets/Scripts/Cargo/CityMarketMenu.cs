using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CityMarketMenu : MonoBehaviour
{
    [SerializeField] MenuAnimationY menuAnimY;
    [SerializeField] GameObject marketCargoItemParent;
    [SerializeField] GameObject inventoryCargoItemParent;
    [SerializeField] Vector2 marketStartPos;
    [SerializeField] Vector2 inventoryStartPos;
    [SerializeField] float yOffset;
    [Space]
    [SerializeField] GameObject[] buyAndSellUI;
    [SerializeField] TextMeshProUGUI cityText;
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] TextMeshProUGUI totalText;
    [SerializeField] GameObject resetButton;
    [SerializeField] GameObject allButton;

    List<GameObject> marketCargoItems = new List<GameObject>();
    List<GameObject> inventoryCargoItems = new List<GameObject>();

    List<GameObject> selectedCargoItems = new List<GameObject>();
    List<float> selectedCargoItemCounts = new List<float>();

    GameObject lastSelectedCargoItem;
    float totalCount;
    bool isBuying;
    int discount;

    CargoManager cargoManager;
    CityManager cityManager;
    GameManager gameManager;

    void Awake()
    {
        cargoManager = FindObjectOfType<CargoManager>();
        cityManager = FindObjectOfType<CityManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        resetButton.SetActive(false);
        allButton.SetActive(true);
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

    public void SetDiscount(int newDiscount)
    {
        discount = newDiscount;

        for (int i = 0; i < marketCargoItems.Count; i++)
        {
            CargoItem cargoItemScript = marketCargoItems[i].GetComponent<CargoItem>();
            if (cargoItemScript == null) { continue; }

            float originalPrice = cargoItemScript.GetItemPrice();
            float discountedPrice = originalPrice - (originalPrice * ((float)discount / 100f));
            discountedPrice = Mathf.Round(discountedPrice);

            cargoItemScript.SetItemPrice(discountedPrice);
        }
    }

    public void ResetInventoryItems()
    {
        for (int i = 0; i < inventoryCargoItems.Count; i++)
        {
            inventoryCargoItems[i].SetActive(false);
            Destroy(inventoryCargoItems[i]);
        }

        inventoryCargoItems.Clear();

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
        cityText.text = cityManager.GetNextCity().name;

        Vector2 lastPos = Vector2.zero;

        //Market Cargo
        for (int i = 0; i < marketCargoItems.Count; i++)
        {
            marketCargoItems[i].transform.SetParent(marketCargoItemParent.transform);
            marketCargoItems[i].transform.localPosition = Vector2.zero;

            if (i == 0)
            {
                marketCargoItems[i].transform.localPosition = marketStartPos;
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
            inventoryCargoItems[i].transform.localScale = Vector3.one;

            if (i == 0)
            {
                inventoryCargoItems[i].transform.localPosition = inventoryStartPos;
            }
            else
            {
                inventoryCargoItems[i].transform.localPosition = new Vector2(lastPos.x, lastPos.y - yOffset);
            }

            lastPos = inventoryCargoItems[i].transform.localPosition;

            CargoItem newCargoItemScript = inventoryCargoItems[i].GetComponent<CargoItem>();
            CargoItem cargoItemScript = cargoManager.GetCargoItemList()[i].GetComponent<CargoItem>();

            newCargoItemScript.SetItemCount(cargoItemScript.GetItemCount());
            newCargoItemScript.SetItemName(cargoItemScript.GetItemName(), "");
            newCargoItemScript.SetIsInCity(false, "");
            newCargoItemScript.SetItemPrice(cargoItemScript.GetItemPrice());

            //MINIMAL ADDITION: copy purchase price so profit calc uses correct buy price
            newCargoItemScript.SetPurchasePrice(cargoItemScript.GetPurchasePrice());

            newCargoItemScript.CalculateProfit();
        }
    }

    void Update()
    {
        CheckItems();
    }

    void CheckItems()
    {
        // 1. Remove nulls safely
        marketCargoItems.RemoveAll(item => item == null);

        // 2. Merge duplicates by name
        Dictionary<string, CargoItem> uniqueItems = new Dictionary<string, CargoItem>();

        for (int i = 0; i < marketCargoItems.Count; i++)
        {
            CargoItem cargoItem = marketCargoItems[i].GetComponent<CargoItem>();
            if (cargoItem == null) continue;

            string itemName = cargoItem.GetItemName();

            if (uniqueItems.ContainsKey(itemName))
            {
                // Merge counts into the first one
                uniqueItems[itemName].AddCount(cargoItem.GetItemCount());

                // Destroy the duplicate
                Destroy(cargoItem.gameObject);
                marketCargoItems[i] = null;
            }
            else
            {
                uniqueItems[itemName] = cargoItem;
            }
        }

        // 3. Clean up any destroyed entries
        marketCargoItems.RemoveAll(item => item == null);

        // 4. Re-setup the UI if anything changed
        SetUpCargoItems();
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
            if (isMarketItem != isBuying)
            {
                ResetMarket();
            }

            isBuying = isMarketItem;

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
        if (lastSelectedCargoItem == null)
        {
            selectedCargoItems.Remove(lastSelectedCargoItem);
        }

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
        if (cargoItemScript == null || selectedCargoItemCounts[index] >= cargoItemScript.GetItemCount()) { return; }

        float currentTotal = selectedCargoItemCounts[index] + 1;
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

        resetButton.SetActive(true);
        allButton.SetActive(false);

        countText.text = totalCount.ToString();
    }

    public void CommitAll()
    {
        if (lastSelectedCargoItem == null)
        {
            selectedCargoItems.Remove(lastSelectedCargoItem);
        }

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
        if (cargoItemScript == null || selectedCargoItemCounts[index] >= cargoItemScript.GetItemCount()) { return; }

        float currentTotal = cargoItemScript.GetItemCount();
        selectedCargoItemCounts[index] = currentTotal;

        cargoItemScript.SetTempCountText("0");

        totalCount += currentTotal;

        float totalPrice = 0;
        for (int i = 0; i < selectedCargoItems.Count; i++)
        {
            totalPrice += selectedCargoItemCounts[i] * selectedCargoItems[i].GetComponent<CargoItem>().GetItemPrice();
        }

        totalText.text = totalPrice.ToString();

        resetButton.SetActive(true);
        allButton.SetActive(false);

        countText.text = totalCount.ToString();
    }

    public void CommitEverything()
    {
        List<GameObject> currentList = isBuying ? marketCargoItems : inventoryCargoItems;

        for (int i = 0; i < currentList.Count; i++)
        {
            if (currentList[i] == null)
            {
                currentList.RemoveAt(i);

                Debug.LogWarning("Market Cargo Item was null, removing from list.");
                continue;
            }

            bool found = false;
            int index = 0;
            for (int ii = 0; ii < selectedCargoItems.Count; ii++)
            {
                if (selectedCargoItems[ii] == currentList[i])
                {
                    found = true;
                    index = ii;
                    break;
                }
            }

            if (!found)
            {
                selectedCargoItems.Add(currentList[i]);
                selectedCargoItemCounts.Add(0);
                index = selectedCargoItems.Count - 1;
            }

            CargoItem cargoItemScript = currentList[i].GetComponent<CargoItem>();
            if (cargoItemScript == null || selectedCargoItemCounts[index] >= cargoItemScript.GetItemCount()) { continue; }

            float currentTotal = cargoItemScript.GetItemCount();
            selectedCargoItemCounts[index] = currentTotal;


            float totalPrice = 0;
            for (int ii = 0; ii < selectedCargoItems.Count; ii++)
            {
                totalPrice += selectedCargoItemCounts[ii] * selectedCargoItems[ii].GetComponent<CargoItem>().GetItemPrice();
            }

            totalText.text = totalPrice.ToString();

            cargoItemScript.SetTempCountText("0");
            totalCount += currentTotal;

            resetButton.SetActive(true);
            allButton.SetActive(false);

            countText.text = totalCount.ToString();
        }
    }

    public void ResetMarket()
    {
        for (int i = 0; i < selectedCargoItems.Count; i++)
        {
            CargoItem cargoItemScript = selectedCargoItems[i].GetComponent<CargoItem>();
            cargoItemScript.SetTempCountText(cargoItemScript.GetItemCount().ToString());
        }

        totalCount = 0;
        totalText.text = "0";
        countText.text = "0";

        selectedCargoItems.Clear();
        selectedCargoItemCounts.Clear();

        resetButton.SetActive(false);
        allButton.SetActive(true);
    }

    public void BuySell()
    {
        if (isBuying)
        {
            float totalPrice = 0;
            for (int i = 0; i < selectedCargoItems.Count; i++)
            {
                totalPrice += selectedCargoItemCounts[i] * selectedCargoItems[i].GetComponent<CargoItem>().GetItemPrice();
            }

            if (gameManager.GetCoins() < totalPrice) { return; }
            gameManager.AddCoins(-totalPrice);

            for (int i = 0; i < selectedCargoItems.Count; i++)
            {
                CargoItem cargoItemScript = selectedCargoItems[i].GetComponent<CargoItem>();
                if (cargoItemScript == null) { continue; }

                int buyAmount = (int)selectedCargoItemCounts[i];
                int currentStock = cargoItemScript.GetItemCount();

                // Prevent buying more than available
                int finalStock = Mathf.Max(0, currentStock - buyAmount);

                // Add cargo to player
                cargoManager.AddCargo(selectedCargoItems[i], buyAmount, cargoItemScript.GetItemPrice());

                // Update real stock
                cargoItemScript.SetItemCount(finalStock);
            }
        }
        else
        {
            float totalPrice = 0;
            for (int i = 0; i < selectedCargoItems.Count; i++)
            {
                totalPrice += selectedCargoItemCounts[i] * selectedCargoItems[i].GetComponent<CargoItem>().GetPurchasePrice();

                CargoItem cargoItemScript = selectedCargoItems[i].GetComponent<CargoItem>();
                if (cargoItemScript == null) { continue; }

                int sellAmount = (int)selectedCargoItemCounts[i];
                int currentStock = cargoItemScript.GetItemCount();

                int finalStock = Mathf.Max(0, currentStock - sellAmount);

                cargoItemScript.SetItemCount(finalStock, true);
            }

            gameManager.AddCoins(totalPrice);
        }

        ResetMarket();
    }

    public void RemoveInventoryItem(int index)
    {
        Destroy(inventoryCargoItems[index]);
        inventoryCargoItems.RemoveAt(index);
    }

    public List<GameObject> GetMarketCargoItems()
    {
        return marketCargoItems;
    }
}
