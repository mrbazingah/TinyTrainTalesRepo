using System.Collections.Generic;
using UnityEngine;

public class CityMarketMenu : MonoBehaviour
{
    [SerializeField] MenuAnimationY menuAnimY;
    [SerializeField] GameObject cargoItemParent;
    [SerializeField] Vector2 startPos;
    [SerializeField] float yOffset;

    List<GameObject> cargoItemList = new List<GameObject>();

    public void OpenCargoMenu()
    {
        menuAnimY.StartAnimation();
    }

    public void SetCargoList(List<GameObject> newCargoItems)
    {
        cargoItemList = newCargoItems;
        SetUpCargoItems();
    }

    void SetUpCargoItems()
    {
        Vector2 lastPos = Vector2.zero;

        for (int i = 0; i < cargoItemList.Count; i++)
        {
            cargoItemList[i].transform.SetParent(cargoItemParent.transform);
            cargoItemList[i].transform.localPosition = Vector2.zero;

            if (i == 0)
            {
                cargoItemList[i].transform.localPosition = startPos;
            }
            else
            {
                cargoItemList[i].transform.localPosition = new Vector2(lastPos.x, lastPos.y - yOffset);
            }

            lastPos = cargoItemList[i].transform.localPosition;
        }
    }
}
