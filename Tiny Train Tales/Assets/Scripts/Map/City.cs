using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] GameObject cityMenu;
    [SerializeField] GameObject[] cityNeighbors = new GameObject[0];
    [SerializeField] int[] cityNeighborsDistances = new int[0];
    [SerializeField] GameObject[] neighborLines = new GameObject[0];

    string cityName;
    string countryName;

    bool isUnlocked = false;

    List<GameObject> cargo = new List<GameObject>();

    CityMenu cityMenuScript;
    CargoManager cargoManager;
    CityMarketMenu cityCargoMenu;

    void Awake()
    {
        cityMenuScript = FindObjectOfType<CityMenu>();
        cargoManager = FindObjectOfType<CargoManager>();
        cityCargoMenu = FindObjectOfType<CityMarketMenu>();
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

    public void CreateCargoItemForCity()
    {
        int index = 0;
        if (PlayerPrefs.HasKey("ItemCount" + gameObject.name))
        {
            index = PlayerPrefs.GetInt("ItemCount" + gameObject.name);
        }
        else
        {
            index = Random.Range(cargoManager.GetCityMinCargoAmount(), cargoManager.GetCityMaxCargoAmount() + 1);

            PlayerPrefs.SetInt("ItemCount" + gameObject.name, index);
        }

        for (int i = 0; i < index; i++)
        {
            if (cargoManager != null)
            {
                GameObject newCargoItem = cargoManager.CreateCargoItem(gameObject.name);
                if (!CheckForDuplicates(newCargoItem))
                {
                    cargo.Add(newCargoItem);
                }
                else
                {
                    newCargoItem.SetActive(false);
                    Destroy(newCargoItem);

                    continue;
                }
            }
        }

        cityCargoMenu.SetCargoList(cargo);
    }

    bool CheckForDuplicates(GameObject newCargo)
    {
        for (int i = 0; i < cargo.Count; i++)
        {
            if (cargo[i].GetComponent<CargoItem>().GetItemIcon().sprite == newCargo.GetComponent<CargoItem>().GetItemIcon().sprite)
            {
                return true;
            }
        }

        return false;
    }

    public GameObject[] GetCityNeighbors()
    {
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

    public void SaveCityCargo()
    {

    }
}
