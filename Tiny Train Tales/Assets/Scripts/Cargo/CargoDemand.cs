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

    #region Sets
    public void SetItemIcon(Sprite itemSprite)
    {
        itemIcon.sprite = itemSprite;
    }

    public void SetItemCount(int count)
    {
        itemCount = count;
        itemCountText.text = itemCount.ToString() + "/" + itemMaxCount.ToString();
    }
    #endregion

    public void AddCount(int count)
    {
        itemCount += count;
        itemCountText.text = itemCount.ToString() + "/" + itemMaxCount.ToString();
    }

    #region Gets
    public string GetItemName()
    {
        return itemName;
    }

    public int GetItemCount()
    {
        return itemCount;
    }
    #endregion
}
