using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    [SerializeField] GameObject currentCity;
    [SerializeField] GameObject currentNextCity;
    [SerializeField] GameObject destinationCity;
    [SerializeField] List<GameObject> path;

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
        if (PlayerPrefs.GetInt("Finished") == 1)
        {
            GetRandomCity();
        }
        else
        {
            FindPathStart();
        }
    }

    #region Path Finding
    void FindPathStart()
    {
        if (PlayerPrefs.HasKey("CurrentNextCity") && PlayerPrefs.HasKey("DestinationCity"))
        {
            string currentCityString = PlayerPrefs.GetString("CurrentNextCity");
            currentCity = GameObject.Find(currentCityString);

            string destinationCityString = PlayerPrefs.GetString("DestinationCity");
            destinationCity = GameObject.Find(destinationCityString);
        }

        GameObject startCity = currentCity;
        GameObject targetCity = destinationCity;

        if (pathfinding != null && startCity != null && targetCity != null)
        {
            path = pathfinding.FindPath(startCity, targetCity);
        }

        if (path.Count == 1)
        {
            currentNextCity = destinationCity;
            PlayerPrefs.SetInt("Finished", 1);
        }

        GetNextCity();

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
            currentNextCity = path[0];

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
    #endregion

    #region Random City
    public void GetRandomCity()
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
            return;
        }

        City currentCityScript = currentCity.GetComponent<City>();
        GameObject[] currentCityNeighbors = currentCityScript.GetCityNeighbors();

        int nextCity = (int)Random.Range(0, currentCityNeighbors.Length);
        destinationCity = currentCityNeighbors[nextCity];

        currentNextCity = destinationCity;

        FindPathStart();
    }
    #endregion

    #region Saves and Gets
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
    #endregion
}
