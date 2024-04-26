using TMPro;
using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] GameObject cityMenu;
    [SerializeField] GameObject[] cityNeighbors;
    [SerializeField] int[] cityNeighborsDistances;

    string cityName;

    CityMenu cityMenuScript;

    void Awake()
    {
        cityMenuScript = FindObjectOfType<CityMenu>();
    }

    void Start()
    {
        cityName = gameObject.name;
        cityMenu?.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && cityMenu.activeInHierarchy)
        {
            if (cityMenuScript == null)
            {
                cityMenuScript = FindObjectOfType<CityMenu>();
            }

            bool mouseIsOnMenu = cityMenuScript.GetMouseIsOnMenu();
            if (!mouseIsOnMenu)
            {
                CloseMenu();
            }
        }
    }

    public void OpenMenu()
    {
        cityMenu?.SetActive(true);
        TextMeshProUGUI nameText = cityMenu.GetComponentInChildren<TextMeshProUGUI>();
        nameText.text = cityName;
    }

    public void CloseMenu()
    {
        cityMenu?.SetActive(false);
    }

    public GameObject[] GetCityNeighbors()
    {
        return cityNeighbors;
    }

    public int[] GetCityNeighborsDistance()
    {
        return cityNeighborsDistances;
    }
}
