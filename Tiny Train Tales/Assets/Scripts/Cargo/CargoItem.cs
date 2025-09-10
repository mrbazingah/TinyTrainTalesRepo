using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CargoItem : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemCountText;

    string itemName;

    public void SetItemIcon(Sprite itemSprite)
    {
        itemIcon.sprite = itemSprite;
    }

    public void SetItemName(string name)
    {
        itemName = name;
        gameObject.name = name + " Item";
    }

    public Image GetItemIcon()
    {
        return itemIcon;
    }

    public TextMeshProUGUI GetItemCountText()
    {
        return itemCountText;
    }
}
