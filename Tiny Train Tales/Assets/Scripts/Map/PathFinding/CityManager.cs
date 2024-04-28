using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    [SerializeField] GameObject currentCity;
    [SerializeField] GameObject currentNextCity;
    [SerializeField] GameObject destinationCity;
    [SerializeField] List<GameObject> path;

    int currentCityIndex;
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

        if (PlayerPrefs.HasKey("CurrentNextCity") && PlayerPrefs.HasKey("DestinationCity"))
        {

        }

        GameObject startCity = currentCity;
        GameObject targetCity = destinationCity;

        if (pathfinding != null && startCity != null && targetCity != null)
        {
            path = pathfinding.FindPath(startCity, targetCity);
            if (path != null && path.Count > 0)
            {
                currentCityIndex = 0;
                gameManager.UpdateCityTexts();
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
        currentCityText.text = path[currentCityIndex].name;
        if (currentCityIndex < path.Count - 1)
        {
            destinationCityText.text = path[currentCityIndex + 1].name;
        }
    }

    public void GetNextCity()
    {
        if (pathfinding == null)
        {
            pathfinding = FindObjectOfType<AStarPathfinding>();
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

            gameManager.SetNewDestination(currentCity.name, currentNextCity.name, destinationDistance); 
        }
        else
        {
            Debug.Log("No valid path found!");
        }
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
