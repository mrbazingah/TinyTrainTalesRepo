using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    [SerializeField] GameObject currentCity;
    [SerializeField] GameObject currentNextCity;
    [SerializeField] GameObject destinationCity;
    [SerializeField] List<GameObject> path;
    [SerializeField] int currentCityIndex;

    int destinationDistance;

    GameManager gameManager;
    AStarPathfinding pathfinding;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();   
        pathfinding = FindObjectOfType<AStarPathfinding>();
    }

    void Start()
    {
        int numberOfCityManagers = FindObjectsOfType<CityManager>().Length;
        if (numberOfCityManagers > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        GameObject startCity = currentCity;
        GameObject targetCity = destinationCity;

        if (pathfinding != null && startCity != null && targetCity != null)
        {
            path = pathfinding.FindPath(startCity, targetCity);
            if (path != null && path.Count > 0)
            {
                currentCityIndex = 0;
            }
            else
            {
                Debug.LogError("No valid path found!");
            }
        }
        else
        {
            Debug.LogError("AStarPathfinding script or start/target cities are null.");
        }
    }

    public void UpdateCityTexts(TextMeshProUGUI currentCityText, TextMeshProUGUI destinationCityText)
    {
        pathfinding = FindObjectOfType<AStarPathfinding>();

        if (path == null)
        {
            path = pathfinding.FindPath(currentCity, destinationCity);
        }

        currentCityText.text = path[currentCityIndex].name;
        if (currentCityIndex < path.Count - 1)
        {
            destinationCityText.text = path[currentCityIndex + 1].name;
        }
    }

    public void GetNextCity()
    {
        pathfinding = FindObjectOfType<AStarPathfinding>();

        if (PlayerPrefs.HasKey("CurrentNextCity") && PlayerPrefs.HasKey("DestinationCity"))
        {
            currentCity = GameObject.Find(PlayerPrefs.GetString("CurrentNextCity"));
            currentNextCity = GameObject.Find(PlayerPrefs.GetString("CurrentNextCity"));
            destinationCity = GameObject.Find(PlayerPrefs.GetString("DestinationCity"));

            currentCityIndex++;
        }

        List<GameObject> path = pathfinding.FindPath(currentCity, destinationCity);
        if (path != null && path.Count > 1)
        {
            GameObject nextCity = path[0];
            currentNextCity = nextCity;

            GameObject[] destinationNeighbors = currentNextCity.GetComponent<City>().GetCityNeighbors();
            for (int i = 0; i < destinationNeighbors.Length; i++)
            {
                if (currentCity == destinationNeighbors[i])
                {
                    int[] distances = currentNextCity.GetComponent<City>().GetCityNeighborsDistance();
                    destinationDistance = distances[i];
                    break;
                }
            }

            gameManager.UpdateCityTexts();
            gameManager.SetNewDestination(currentCity.name, currentNextCity.name, destinationDistance); 
        }
        else
        {
            pathfinding.FindPath(currentCity, destinationCity);
        }
    }

    public void SaveCurrentCity()
    {
        PlayerPrefs.SetString("CurrentCity", currentCity.name);
    }

    public void SaveCurrentNextCity()
    {
        PlayerPrefs.SetString("CurrentNextCity", currentNextCity.name);
    }

    public void SaveDestinationCity()
    {
        PlayerPrefs.SetString("DestinationCity", destinationCity.name);
    }

    public GameObject GetCurrentCity()
    {
        return currentCity;
    }

    public GameObject GetCurrentNextCity()
    {
        return currentNextCity;
    }

    public GameObject GetDestinationCity() 
    { 
        return destinationCity;
    }
}
