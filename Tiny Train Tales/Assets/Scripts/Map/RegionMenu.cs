using TMPro;
using UnityEngine;

public class RegionMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI regionText;

    bool mouseIsOnMenu; 

    public void SetTexts(string regionName)
    {
        regionText.text = regionName;
    }

    void OnMouseEnter()
    {
        mouseIsOnMenu = true;
    }

    void OnMouseExit()
    {
        mouseIsOnMenu = false;
    }

    public bool GetMouseIsOnMenu()
    {
        return mouseIsOnMenu;
    }
}
