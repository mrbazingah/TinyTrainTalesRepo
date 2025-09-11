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
        if (!turnOffBuyOption)
        {
            sellButtonColorBlock = buyButtonComp.colors;
            sellButtonColorBlock.normalColor = cantAffordColor;
            sellButtonColorBlock.highlightedColor = cantAffordColor;
            sellButtonColorBlock.pressedColor = cantAffordColor;
            buyButtonComp.colors = sellButtonColorBlock;
        }

        if (!turnOffSellButton)
        {
            buyButtonColorBlock = sellButtonComp.colors;
            buyButtonColorBlock.normalColor = cantAffordColor;
            buyButtonColorBlock.highlightedColor = cantAffordColor;
            buyButtonColorBlock.pressedColor = cantAffordColor;
            sellButtonComp.colors = buyButtonColorBlock;
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

    public void SaveCargoItem()
    {
        string saveString = isInCity ? itemName + " " + cityName : itemName;

        PlayerPrefs.SetString(saveString + "itemName", itemName);
        PlayerPrefs.SetString(saveString + "cityName", cityName);
        PlayerPrefs.SetInt(saveString + "itemCount", itemCount);
        PlayerPrefs.SetFloat(saveString + "itemPrice", itemPrice);
    }
}
