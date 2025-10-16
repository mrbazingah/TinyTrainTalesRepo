using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CargoDemand : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemCountText;

    int itemCount;
    int itemMaxCount;
    string itemName;

    City currentCity;

    #region Sets
    public void SetItemIcon(Sprite itemSprite)
    {
        itemIcon.sprite = itemSprite;
    }

    public void SetItemCount(int count, int maxCount)
    {
        itemCount = count;
        itemMaxCount = maxCount;
        itemCountText.text = itemCount.ToString() + "/" + itemMaxCount.ToString();
    }

    public void SetItemName(string newName)
    {
        itemName = newName;
    }

    public void SetCity(City city)
    {
        currentCity = city;
    }
    #endregion

    public void AddCount(int count)
    {
        itemCount += count;
        itemCountText.text = itemCount.ToString() + "/" + itemMaxCount.ToString();

        currentCity.AddCargoCount(count);
    }

    #region Gets
    public int GetItemCount()
    {
        return itemCount;
    }

    public string GetItemName()
    {
        return itemName;
    }
    #endregion
}
