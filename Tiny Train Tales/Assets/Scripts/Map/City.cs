using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems; // NEW

public class City : MonoBehaviour
{
    [SerializeField] GameObject cityMenu;
    [SerializeField] GameObject[] cityNeighbors = new GameObject[0];
    [SerializeField] int[] cityNeighborsDistances = new int[0];
    [SerializeField] GameObject[] neighborLines = new GameObject[0];

    string cityName;
    string countryName;

    bool isUnlocked = false;

    CityMenu cityMenuScript;

    void Awake()
    {
        cityMenuScript = FindObjectOfType<CityMenu>();
    }

    void Start()
    {
        cityName = gameObject.name;
        countryName = transform.parent.name;

        cityMenu?.SetActive(false);

        if (isUnlocked && cityNeighbors.Length != 0 && neighborLines.Length != 0)
        {
            for (int i = 0; i < cityNeighbors.Length; i++)
            {
                neighborLines[i]?.SetActive(cityNeighbors[i].GetComponent<City>().GetIsUnlocked());
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && cityMenu.activeInHierarchy)
        {
            // Ensure CityMenu reference
            if (cityMenuScript == null)
            {
                cityMenuScript = FindObjectOfType<CityMenu>();
            }

            // Don’t close if mouse is on any UI element
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            // Close if not inside the city menu
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

        if (cityMenuScript == null)
        {
            cityMenuScript = FindObjectOfType<CityMenu>();
        }

        cityMenuScript.SetTexts(cityName, countryName);
    }

    public void CloseMenu()
    {
        cityMenu?.SetActive(false);
    }

    public void SetUnlockedState(bool b)
    {
        isUnlocked = b;
    }

    public GameObject[] GetCityNeighbors()
    {
        if (cityNeighbors.Length == 0)
        {
            Debug.LogError($"{name} has NULL cityNeighbors! Check this object in Inspector.");
            return new GameObject[0]; // prevents crash
        }
        return cityNeighbors;
    }

    public int[] GetCityNeighborsDistance()
    {
        return cityNeighborsDistances;
    }

    public GameObject[] GetCityNeighborLines()
    {
        return neighborLines;
    }

    public bool GetIsUnlocked()
    {
        return isUnlocked;
    }
}
