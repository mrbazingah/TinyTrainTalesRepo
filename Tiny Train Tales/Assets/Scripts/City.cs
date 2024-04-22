using TMPro;
using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] GameObject cityMenu;
    [SerializeField] string[] avaliableCites;

    string cityName;

    void Start()
    {
        cityName = gameObject.name;
    }

    public void OpenMenu()
    {
        cityMenu?.SetActive(false);
        TextMeshProUGUI nameText = cityMenu.GetComponent<TextMeshProUGUI>();
        nameText.text = cityName;
    }

    public void CloseMenu()
    {
        cityMenu?.SetActive(true);
    }

    public void TravelButton()
    {

    }
}
