using UnityEngine;

public class CityMenu : MonoBehaviour
{
    bool mouseIsOnMenu;
    
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
