using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CargoItem : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemCountText;
    [SerializeField] TextMeshProUGUI itemPriceText;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject[] trainUI;

    int itemCount;
    string itemName;
    bool isInCity = true;
    string cityName;
    float itemPrice;

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
        }
        else
        { 
            if (cityName != "")
            {
                GameObject.Find(cityName)?.GetComponent<City>()?.CreateCargoItemForCity();
                Destroy(gameObject);
                gameObject.SetActive(false);
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

    public void SetItemCount(int count)
    {
        itemCount = count;
        itemCountText.text = itemCount.ToString();
    }

    public void SetItemPrice(float price)
    {
        itemPrice = price;
        itemPriceText.text = itemPrice.ToString();
    }

    public void BuyItem()
    {
        cargoManager.AddCargo(gameObject);
    }

    public void AddCount(int count)
    {
        itemCount += count;
        itemCountText.text = itemCount.ToString();
    }

    public void ChangeUI()
    {
        for (int i = 0; i < trainUI.Length; i++)
        {
            trainUI[i].SetActive(!isInCity);
        }
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

    public string GetSaveString()
    {
        return isInCity ? itemName + " " + cityName : itemName;
    }

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
