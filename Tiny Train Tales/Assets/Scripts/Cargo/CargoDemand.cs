using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CargoDemand : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemCountText;
    [SerializeField] Slider slider;

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
        itemMaxCount = maxCount;
        itemCount = Mathf.Clamp(count, 0, itemMaxCount);
        itemCountText.text = itemCount.ToString() + "/" + itemMaxCount.ToString();

        slider.minValue = 0;
        slider.maxValue = itemMaxCount;
        slider.value = itemCount;
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
        int previous = itemCount;
        itemCount = Mathf.Clamp(itemCount + count, 0, itemMaxCount);
        int actualAdded = itemCount - previous;

        itemCountText.text = itemCount.ToString() + "/" + itemMaxCount.ToString();
        slider.value = itemCount;

        currentCity.AddCargoCount(actualAdded);
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
