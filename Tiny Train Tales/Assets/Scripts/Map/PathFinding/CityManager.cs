using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CityManager : MonoBehaviour
{
    [SerializeField] GameObject currentCity;
    [SerializeField] GameObject nextCity;
    [SerializeField] GameObject destinationCity;
    [Space]
    [SerializeField] Color pathColor;
    [SerializeField] GameObject linePrefab;
    [Space]
    [SerializeField] List<GameObject> path;

    int destinationDistance;
    GameObject currentCityLine;

    GameManager gameManager;
    PathFinding pathfinding;

    GameObject[] cities;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();   
        pathfinding = FindObjectOfType<PathFinding>();

        //CreateLines();
    }

    void CreateLines()
    {
        City[] citiesScript = FindObjectsOfType<City>();
        cities = new GameObject[citiesScript.Length];
        for (int i = 0; i < citiesScript.Length; i++)
        {
            cities[i] = citiesScript[i].gameObject;
        }

        for (int i = 0; i < cities.Length; i++)
        {
            GameObject[] cityNeighbors = cities[i].GetComponent<City>().GetCityNeighbors();

            for (int j = 0; j < cityNeighbors.Length; j++)
            {
                Instantiate(linePrefab);
            }
        }
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("CurrentCity"))
        {
            string currentCityString = PlayerPrefs.GetString("CurrentCity");
            currentCity = GameObject.Find(currentCityString);
        }
        if (PlayerPrefs.HasKey("DestinationCity"))
        {
            string destinationCityString = PlayerPrefs.GetString("DestinationCity");
            destinationCity = GameObject.Find(destinationCityString);

            FindPathAtStart(currentCity, destinationCity);
        }
        else if (destinationCity != null) 
        {
            FindPathAtStart(currentCity, destinationCity);
        }
        else
        {
            GetRandomCity();
        }
    }

    #region Path Finding
    public void FindPathAtStart(GameObject startCityGameObject, GameObject targetCityGameObject)
    {
        GameObject startCity = startCityGameObject;
        GameObject targetCity = targetCityGameObject;

        if (currentCity == destinationCity)
        {
            GetRandomCity();
            return;
        }

        if (pathfinding != null && startCity != null && targetCity != null)
        {
            if (!PlayerPrefs.HasKey("NextCity"))
            {
                path = pathfinding.FindPath(startCity, targetCity, null);
            }
            else
            {
                GameObject inBetweenCity = GameObject.Find(PlayerPrefs.GetString("NextCity"));
                path = pathfinding.FindPath(startCity, targetCity, inBetweenCity);

                PlayerPrefs.DeleteKey("NextCity");
            }
        }

        GetNextCityInPath();
        gameManager.UpdateCityTexts();
    }

    public void UpdateCityTexts(TextMeshProUGUI currentCityText, TextMeshProUGUI destinationCityText)
    {
        currentCityText.text = currentCity.name;
        destinationCityText.text = nextCity.name;
    }

    void GetNextCityInPath()
    {
        if (path == null)
        {
            GetRandomCity();
            return;
        }

        nextCity = path[0];

        GameObject[] destinationNeighbors = nextCity.GetComponent<City>().GetCityNeighbors();
        for (int i = 0; i < destinationNeighbors.Length; i++)
        {
            if (currentCity == destinationNeighbors[i])
            {
                int[] distances = nextCity.GetComponent<City>().GetCityNeighborsDistance();
                destinationDistance = distances[i];
                break;
            }
        }

        gameManager.SetNewDestination(currentCity.name, nextCity.name, destinationDistance);
        pathfinding.FindPath(currentCity, destinationCity, null);
        ColorAll();
    }
    #endregion

    #region Random City
    public void GetRandomCity()
    {
        City currentCityScript = currentCity.GetComponent<City>();
        GameObject[] currentCityNeighbors = currentCityScript.GetCityNeighbors();

        int nextCityInt = (int)Random.Range(0, currentCityNeighbors.Length);
        destinationCity = currentCityNeighbors[nextCityInt];

        FindPathAtStart(currentCity, destinationCity);
        ColorAll();
    }
    #endregion

    #region Visual Setup
    void ColorAll()
    {
        for (int i = 0; i < path.Count; i++)
        {
            ColorCities(i);
            ColorLines(i);
        }
    }

    void ColorLines(int i)
    {
        City cityScript = path[i].GetComponent<City>();

        GameObject[] cityNeighbors = cityScript.GetCityNeighbors();
        GameObject[] cityNeighborLines = cityScript.GetCityNeighborLines();
        
        for (int j = 0; j < cityNeighbors.Length; j++)
        {
            if (i != path.Count - 1)
            {
                if (cityNeighbors[j] == path[i + 1])
                {
                    cityNeighborLines[j].GetComponent<Image>().color = pathColor;
                }
            }
        }
    }

    void ColorCities(int i)
    {
        path[i].GetComponent<Image>().color = pathColor;
    }
    #endregion

    #region Saves and Gets
    public void SaveOnDeparture()
    {
        PlayerPrefs.SetString("CurrentCity", nextCity.name);
        PlayerPrefs.SetString("DestinationCity", destinationCity.name);
    }

    public void SaveCityOnQuit()
    {
        PlayerPrefs.SetString("DestinationCity", destinationCity.name);
        PlayerPrefs.SetString("CurrentCity", currentCity.name);
        PlayerPrefs.SetString("NextCity", nextCity.name);
    }

    public void ResetPath()
    {
        if (path.Count == 1)
        {
            PlayerPrefs.DeleteKey("DestinationCity");
        }
    }

    public void SetNewDestinationCity(GameObject newDestinationCity)
    {
        PlayerPrefs.SetString("CurrentCity", currentCity.name);
        PlayerPrefs.SetString("NextCity", nextCity.name);
        PlayerPrefs.SetString("DestinationCity", newDestinationCity.name);
    }

    public GameObject GetCurrentCity()
    {
        return currentCity;
    }

    public GameObject GetNextCity()
    {
        return nextCity;
    }

    public GameObject GetDestinationCity() 
    { 
        return destinationCity;
    }

    public List<GameObject> GetPath()
    {
        return path;
    }
    #endregion
}
