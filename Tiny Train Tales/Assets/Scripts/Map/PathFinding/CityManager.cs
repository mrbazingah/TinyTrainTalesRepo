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
    PathFinding pathfinding;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();   
        pathfinding = FindObjectOfType<PathFinding>();
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("CurrentNextCity"))
        {
            string currentCityString = PlayerPrefs.GetString("CurrentNextCity");
            currentCity = GameObject.Find(currentCityString);
        }
        if (PlayerPrefs.HasKey("DestinationCity"))
        {
            string destinationCityString = PlayerPrefs.GetString("DestinationCity");
            destinationCity = GameObject.Find(destinationCityString);
        }

        if (currentCity != null && destinationCity != null)
        {
            FindPathStart();
        }
        else
        {
            GetRandomCity();
        }
    }

    #region Path Finding
    void FindPathStart()
    {
        GameObject startCity = currentCity;
        GameObject targetCity = destinationCity;

        if (pathfinding != null && startCity != null && targetCity != null)
        {
            path = pathfinding.FindPath(startCity, targetCity);
        }

        if (currentCity == destinationCity)
        {
            GetRandomCity();
            return;
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
        pathfinding.FindPath(currentCity, destinationCity);
    }
    #endregion

    #region Random City
    public void GetRandomCity()
    {
        City currentCityScript = currentCity.GetComponent<City>();
        GameObject[] currentCityNeighbors = currentCityScript.GetCityNeighbors();

        int nextCity = (int)Random.Range(0, currentCityNeighbors.Length);
        destinationCity = currentCityNeighbors[nextCity];

        FindPathStart();
    }
    #endregion

    #region Saves and Gets
    public void SaveCity()
    {
        PlayerPrefs.SetString("DestinationCity", destinationCity.name);
        PlayerPrefs.SetString("CurrentNextCity", currentNextCity.name);
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
        destinationCity = newDestinationCity;
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
