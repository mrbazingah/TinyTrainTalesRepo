using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] GameObject cityMenu;
    [SerializeField] GameObject[] cityNeighbors = new GameObject[0];
    [SerializeField] int[] cityNeighborsDistances = new int[0];
    [SerializeField] GameObject[] neighborLines = new GameObject[0];
    
    float cargoResetTime; 
    string cityName;
    string countryName;
    int iterations;

    bool isUnlocked = false;

    List<GameObject> cargo = new List<GameObject>();

    CityMenu cityMenuScript;
    CargoManager cargoManager;
    CityMarketMenu cityMarketMenu;
    TimeManager timeManager;

    void Awake()
    {
        cityMenuScript = FindObjectOfType<CityMenu>();
        cargoManager = FindObjectOfType<CargoManager>();
        cityMarketMenu = FindObjectOfType<CityMarketMenu>();
        timeManager = FindObjectOfType<TimeManager>();
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

        cargoResetTime = cargoManager.GetCityCargoResetTime();
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
        if (timeManager.GetCurrentTime(cargoResetTime, "CityTime"))
        {
            PlayerPrefs.DeleteKey("CargoItemAmount" + gameObject.name);
        }

        int index = 0;
        if (PlayerPrefs.HasKey("CargoItemAmount" + gameObject.name))
        {
            index = PlayerPrefs.GetInt("CargoItemAmount" + gameObject.name);

            for (int i = 0; i < index; i++)
            {
                iterations++;
                if (iterations > 2) { break; }

                string saveString = PlayerPrefs.GetString("CitySaveString" + i.ToString() + gameObject.name);
                GameObject newCargoItem = cargoManager.CreateSavedCargoItemForCity(saveString, gameObject.name);
                cargo.Add(newCargoItem);
            }
        }
        else
        {
            index = Random.Range(cargoManager.GetCityMinCargoAmount(), cargoManager.GetCityMaxCargoAmount() + 1);

            PlayerPrefs.SetInt("CargoItemAmount" + gameObject.name, index);

            for (int i = 0; i < index; i++)
            {
                if (cargoManager == null) { return; }

                GameObject newCargoItem = cargoManager.CreateCargoItemForCity(gameObject.name);
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

            timeManager.SaveCurrentTime("Time" + gameObject.name);
        }

        cityMarketMenu.SetCargoList(cargo);
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

    public void HandleMissingCargo(GameObject brokenItem)
    {
        // Try to replace with a new one instead of regenerating everything
        GameObject newCargoItem = cargoManager.CreateCargoItemForCity(gameObject.name);

        int index = cargo.IndexOf(brokenItem);
        if (index != -1)
        {
            cargo[index] = newCargoItem;
        }
        else
        {
            cargo.Add(newCargoItem);
        }

        Destroy(brokenItem);
        cityMarketMenu.SetCargoList(cargo);
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
        PlayerPrefs.SetInt("CargoItemAmount" + gameObject.name, cargo.Count);

        for (int i = 0; i < cargo.Count; i++)
        {
            if (cargo[i] == null)
            {
                cargo.RemoveAt(i);
                continue;
            }

            CargoItem cargoItemScript = cargo[i].GetComponent<CargoItem>();
            string saveString = cargoItemScript.GetSaveString();
            PlayerPrefs.SetString("CitySaveString" + i.ToString() + gameObject.name, saveString);

            cargoItemScript.SaveCargoItem();
        }
    }
}
