using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CargoItem : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemCountText;
    [SerializeField] GameObject[] cityUI;
    [SerializeField] GameObject[] trainUI;
    [Space]
    [SerializeField] GameObject buyButton;
    [SerializeField] GameObject sellButton;
    [SerializeField] Color originalColor;
    [SerializeField] Color cantAffordColor;

    int itemCount;
    string itemName;
    bool isInCity = true;
    string cityName;
    float itemPrice;

    string saveString;

    bool turnOffBuyOption = false;
    bool turnOffSellButton = false;

    ColorBlock buyButtonColorBlock;
    ColorBlock sellButtonColorBlock;

    CargoManager cargoManager;
    Button buyButtonComp;
    Button sellButtonComp;

    void Awake()
    {
        cargoManager = FindObjectOfType<CargoManager>();
        buyButtonComp = buyButton.GetComponent<Button>();
        sellButtonComp = sellButton.GetComponent<Button>();
    }

    public void SetSaveString(string newSaveString)
    {
        saveString = newSaveString;
    }

    public void SetIsInCity(bool b, string cityName)
    {
        isInCity = b;
        this.cityName = b ? cityName : "";
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

    void Update()
    {
        if (turnOffSellButton)
        {
            sellButtonColorBlock = sellButtonComp.colors;
            sellButtonColorBlock.normalColor = cantAffordColor;
            sellButtonColorBlock.highlightedColor = cantAffordColor;
            sellButtonColorBlock.pressedColor = cantAffordColor;
            sellButtonComp.colors = sellButtonColorBlock;
        }

        if (turnOffBuyOption)
        {
            buyButtonColorBlock = buyButtonComp.colors;
            buyButtonColorBlock.normalColor = cantAffordColor;
            buyButtonColorBlock.highlightedColor = cantAffordColor;
            buyButtonColorBlock.pressedColor = cantAffordColor;
            buyButtonComp.colors = buyButtonColorBlock;
        }
    }

    public void TurnOffBuyOption()
    {
        turnOffBuyOption = true;
    }

    public void TurnOffSellButton()
    {
        turnOffSellButton = true;
    }

    public void ChangeUI()
    {
        for (int i = 0; i < cityUI.Length; i++)
        {
            cityUI[i].SetActive(isInCity);
        }

        for (int i = 0; i < trainUI.Length; i++)
        {
            trainUI[i].SetActive(isInCity);
        }

        isInCity = !isInCity;   
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
