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

    GameManager gameManager;
    PathFinding pathfinding;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        pathfinding = FindObjectOfType<PathFinding>();
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
        }

        if (PlayerPrefs.HasKey("NextCity"))
        {
            string nextCityString = PlayerPrefs.GetString("NextCity");
            nextCity = GameObject.Find(nextCityString);
        }

        if (currentCity != null && destinationCity != null)
        {
            FindPathAtStart(currentCity, destinationCity);
        }
        else
        {
            GetRandomCity();
        }
    }

    public void FindPathAtStart(GameObject startCityGameObject, GameObject targetCityGameObject)
    {
        if (startCityGameObject == null || targetCityGameObject == null)
        {
            Debug.LogError("StartCity or TargetCity is null! Cannot find path.");
            return;
        }

        currentCity = startCityGameObject;
        destinationCity = targetCityGameObject;

        // If player tries to set destination to the same as current
        if (currentCity == destinationCity)
        {
            if (nextCity != null && nextCity != currentCity)
            {
                // Continue traveling to nextCity, then return to currentCity
                path = pathfinding.FindPath(nextCity, currentCity, null);
                path.Insert(0, nextCity); // ensure nextCity is included as first step
                GetNextCityInPath();
                gameManager.UpdateCityTexts();
                return;
            }
            else
            {
                GetRandomCity();
                return;
            }
        }

        if (pathfinding != null)
        {
            path = pathfinding.FindPath(currentCity, destinationCity, nextCity);
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
        if (path == null || path.Count == 0)
        {
            GetRandomCity();
            return;
        }

        if (path[0] == currentCity && path.Count > 1)
        {
            path.RemoveAt(0);
        }

        if (path.Count > 0)
        {
            nextCity = path[0];
        }
        else
        {
            GetRandomCity();
            return;
        }

        GameObject[] destinationNeighbors = nextCity.GetComponent<City>().GetCityNeighbors();
        if (destinationNeighbors == null)
        {
            Debug.LogError("destinationNeighbors is NULL on " + nextCity.name);
            return;
        }

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

    public void GetRandomCity()
    {
        City currentCityScript = currentCity.GetComponent<City>();
        GameObject[] currentCityNeighbors = currentCityScript.GetCityNeighbors();

        List<GameObject> unlockedNeighbors = new List<GameObject>();
        for (int i = 0; i < currentCityNeighbors.Length; i++)
        {
            if (currentCityNeighbors[i].GetComponent<City>().GetIsUnlocked())
                unlockedNeighbors.Add(currentCityNeighbors[i]);
        }

        if (unlockedNeighbors.Count == 0)
        {
            Debug.LogWarning("No unlocked neighbors available from the current city!");
            return;
        }

        int nextCityInt = Random.Range(0, unlockedNeighbors.Count);
        destinationCity = unlockedNeighbors[nextCityInt];

        path = new List<GameObject> { destinationCity };

        GetNextCityInPath();
        ColorAll();
    }

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

    public void SaveOnDeparture()
    {
        PlayerPrefs.SetString("CurrentCity", nextCity.name);
        PlayerPrefs.SetString("DestinationCity", destinationCity.name);
        PlayerPrefs.DeleteKey("NextCity");
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
}
