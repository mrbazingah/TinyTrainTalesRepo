using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CargoItem : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemCountText;
    [SerializeField] TextMeshProUGUI itemPriceText;
    [SerializeField] TextMeshProUGUI itemPriceLabelText;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject[] trainUI;
    [SerializeField] Color profitColor;
    [SerializeField] Color lossColor;

    int itemCount;
    string itemName;
    bool isInCity = true;
    bool isInMarket;
    string cityName;
    float itemPrice;
    float profit;
    float purchasePrice; 

    string saveString;

    CargoManager cargoManager;
    CityMarketMenu cityMarketMenu;

    void Awake()
    {
        cargoManager = FindObjectOfType<CargoManager>();
        cityMarketMenu = FindObjectOfType<CityMarketMenu>();
    }

    void Start()
    {
        panel.SetActive(false);
    }

    #region Sets
    public void SetSaveString(string newSaveString)
    {
        saveString = newSaveString;
    }

    public void SetIsInCity(bool b, string cityName)
    {
        isInCity = b;
        this.cityName = b ? cityName : "";

        if (!isInCity)
        {
            ChangeUI();
        }
    }

    public void LoadItemPlayerPrefs()
    {
        if (PlayerPrefs.HasKey(saveString + "itemName"))
        {
            SetItemName(PlayerPrefs.GetString(saveString + "itemName"), cityName);
            SetItemCount(PlayerPrefs.GetInt(saveString + "itemCount"));
            SetItemPrice(PlayerPrefs.GetFloat(saveString + "itemPrice"));
            SetItemIcon(cargoManager.GetCargoItemsSprites()[PlayerPrefs.GetInt(saveString + "itemSpriteIndex")]);

            // NEW — load stored purchase price if exists
            if (PlayerPrefs.HasKey(saveString + "purchasePrice"))
            {
                purchasePrice = PlayerPrefs.GetFloat(saveString + "purchasePrice");
            }
            else
            {
                purchasePrice = itemPrice;
            }
        }
        else
        {
            if (cityName != "")
            {
                // Tell the City this item failed to load
                City city = GameObject.Find(cityName)?.GetComponent<City>();
                if (city != null)
                {
                    city.HandleMissingCargo(this.gameObject);
                }
            }
        }
    }

    public void SetItemIcon(Sprite itemSprite)
    {
        itemIcon.sprite = itemSprite;
    }

    public void SetItemName(string name, string nameOfCity)
    {
        itemName = name;
        itemNameText.text = itemName;

        cityName = nameOfCity;

        gameObject.name = itemName + " " + cityName;
    }

    public void SetItemCount(int count, bool selling = false)
    {
        itemCount = count;
        itemCountText.text = itemCount.ToString();

        if (selling)
        {
            if (count <= 0)
            {
                cargoManager.RemoveCargo(itemName);

                Destroy(gameObject);
            }
            else
            {
                int index = cargoManager.FindMatch(itemName);
                cargoManager.GetCargoItemList()[index].GetComponent<CargoItem>().SetItemCount(count);

                cityMarketMenu.ResetInventoryItems();
            }
        }
    }


    public void SetItemPrice(float price)
    {
        itemPrice = price;
        itemPriceText.text = itemPrice.ToString();

        if (!isInCity)
        {
            itemPriceLabelText.text = "Profit:";

            if (profit >= 0)
            {
                itemPriceText.color = profitColor;
            }
            else
            {
                itemPriceText.color = lossColor;
            }
        }

        PlayerPrefs.SetFloat(saveString + "itemPrice", itemPrice);
    }

    public void SetPurchasePrice(float price)
    {
        purchasePrice = price;
    }

    public void SetIsInMarket(bool b)
    {
        isInMarket = b;
    }
    #endregion

    public void AddCount(int count)
    {
        itemCount += count;
        itemCountText.text = itemCount.ToString();
    }

    public void CalculateProfit()
    {
        if (isInCity) { return; }

        GameObject matchingItem = null;

        for (int i = 0; i < cityMarketMenu.GetMarketCargoItems().Count; i++)
        {
            if (cityMarketMenu.GetMarketCargoItems()[i].GetComponent<CargoItem>().GetItemName() == itemName &&
                cityMarketMenu.GetMarketCargoItems()[i].GetComponent<CargoItem>().GetIsInCity())
            {
                matchingItem = cityMarketMenu.GetMarketCargoItems()[i].gameObject;
                break;
            }
        }

        if (matchingItem != null)
        {
            CargoItem matchingItemScript = matchingItem.GetComponent<CargoItem>();
            float sellPrice = matchingItemScript.GetItemPrice();

            // FIXED — use purchasePrice instead of itemPrice
            profit = sellPrice - purchasePrice;
            itemPriceText.text = profit.ToString("0");
        }
        else
        {
            itemPriceText.text = "0";
        }
    }

    public void ChangeUI()
    {
        /*
        for (int i = 0; i < trainUI.Length; i++)
        {
            trainUI[i].SetActive(!isInCity);
        }
        */
    }

    public void OnSelect()
    {
        panel.SetActive(true);

        cityMarketMenu.SwitchBuyAndSell(gameObject);
    }

    public void OnDeselect()
    {
        panel.SetActive(false);
    }

    public void SetTempCountText(string count)
    {
        itemCountText.text = count;
    }

    #region Gets
    public Image GetItemIcon()
    {
        return itemIcon;
    }

    public TextMeshProUGUI GetItemCountText()
    {
        return itemCountText;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public int GetItemCount()
    {
        return itemCount;
    }

    public float GetItemPrice()
    {
        return itemPrice;
    }

    public bool GetIsInCity()
    {
        return isInCity;
    }

    public bool GetIsInMarket()
    {
        return isInMarket;
    }

    public string GetSaveString()
    {
        return isInCity ? itemName + " " + cityName : itemName;
    }

    public float GetPurchasePrice()
    {
        return purchasePrice;
    }

    #endregion

    public void SaveCargoItem()
    {
        saveString = isInCity ? itemName + " " + cityName : itemName;

        PlayerPrefs.SetString(saveString + "itemName", itemName);
        PlayerPrefs.SetInt(saveString + "itemCount", itemCount);

        PlayerPrefs.SetFloat(saveString + "itemPrice", itemPrice);

        Sprite[] spriteArray = cargoManager.GetCargoItemsSprites();
        for (int i = 0; i < spriteArray.Length; i++)
        {
            if (itemIcon.sprite == spriteArray[i])
            {
                PlayerPrefs.SetInt(saveString + "itemSpriteIndex", i);
                break;
            }
        }
    }
}
