using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] string cityName;
    [SerializeField] GameObject cityMenu;

    public void OpenMenu()
    {
        cityMenu?.SetActive(false);
    }

    public void CloseMenu()
    {
        cityMenu?.SetActive(true);
    }

    public void TravelButton()
    {

    }
}
