using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CargoItem : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemCountText;

    int itemCount;
    string itemName;

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
}
