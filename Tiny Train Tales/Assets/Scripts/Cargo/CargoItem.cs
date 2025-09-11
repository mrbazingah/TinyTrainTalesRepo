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

    ColorBlock buyButtonColorBlock;
    ColorBlock sellButtonColorBlock;

    CargoManager cargoManager;

    void Awake()
    {
        cargoManager = FindObjectOfType<CargoManager>();
    }

    public void SetItemIcon(Sprite itemSprite)
    {
        itemIcon.sprite = itemSprite;
    }

    public void SetItemName(string name)
    {
        itemName = name;
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
    }

    public void TurnOffBuyOption()
    {
        Button buttonComp = buyButton.GetComponent<Button>();
        sellButtonColorBlock = buttonComp.colors;
        sellButtonColorBlock.normalColor = cantAffordColor;
        sellButtonColorBlock.highlightedColor = cantAffordColor;
        sellButtonColorBlock.pressedColor = cantAffordColor;
        buttonComp.colors = sellButtonColorBlock;
    }

    public void TurnOffSellButton()
    {
        Button buttonComp = sellButton.GetComponent<Button>();
        buyButtonColorBlock = buttonComp.colors;
        buyButtonColorBlock.normalColor = cantAffordColor;
        buyButtonColorBlock.highlightedColor = cantAffordColor;
        buyButtonColorBlock.pressedColor = cantAffordColor;
        buttonComp.colors = buyButtonColorBlock;
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
}
