using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    [SerializeField] GameObject currentCity;
    [SerializeField] GameObject inBetweenCity;
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

        if (PlayerPrefs.HasKey("CurrentCity") && PlayerPrefs.HasKey("DestinationCity"))
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
        List<GameObject> path = pathfinding.FindPath(currentCity, destinationCity);
        if (path != null && path.Count > 1)
        {
            // Next city is the second city in the path
            GameObject nextCity = path[0];
            destinationCity = nextCity;

            GameObject[] destinationNeighbors = destinationCity.GetComponent<City>().GetCityNeighbors();
            for (int i = 0; i < destinationNeighbors.Length; i++)
            {
                if (currentCity == destinationNeighbors[i])
                {
                    int[] distances = destinationCity.GetComponent<City>().GetCityNeighborsDistance();
                    destinationDistance = distances[i];
                    break;
                }
            }

            gameManager.SetNewDestination(currentCity.name, destinationCity.name, destinationDistance); // Change the last parameter accordingly
        }
        else
        {
            Debug.Log("No valid path found!");
        }
    }

    public void SaveCurrentCity()
    {
        PlayerPrefs.SetString("CurrentCity", destinationCity.name);
    }

    public void SaveDestinationCity()
    {
        PlayerPrefs.SetString("DestinationCity", destinationCity.name);
    }

    public GameObject GetCurrentCity()
    {
        return currentCity;
    }

    public GameObject GetDestinationCity() 
    { 
        return destinationCity;
    }
}
