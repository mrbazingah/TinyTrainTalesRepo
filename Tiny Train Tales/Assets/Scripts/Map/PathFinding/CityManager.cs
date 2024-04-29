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
        if (PlayerPrefs.HasKey("CurrentNextCity") && PlayerPrefs.HasKey("DestinationCity"))
        {
            currentCity = GameObject.Find(PlayerPrefs.GetString("CurrentNextCity"));
            destinationCity = GameObject.Find(PlayerPrefs.GetString("DestinationCity"));
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

        if (path.Count == 1)
        {
            currentNextCity = GameObject.Find(PlayerPrefs.GetString("DestinationCity"));
        }

        gameManager.UpdateCityTexts();
    }

    public void UpdateCityTexts(TextMeshProUGUI currentCityText, TextMeshProUGUI destinationCityText)
    {
        currentCityText.text = currentCity.name;
        destinationCityText.text = currentNextCity.name;
    }

    public void GetNextCity()
    {
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
            pathfinding.FindPath(currentCity, destinationCity);
        }
    }

    public void DestinationFinished()
    {
        if (path.Count == 1)
        {
            PlayerPrefs.SetString("FinishedDestination", "Has Finished");
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
