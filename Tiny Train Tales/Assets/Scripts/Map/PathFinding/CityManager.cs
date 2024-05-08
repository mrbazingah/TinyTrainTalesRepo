using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CityManager : MonoBehaviour
{
    [SerializeField] GameObject currentCity;
    [SerializeField] GameObject currentNextCity;
    [SerializeField] GameObject destinationCity;
    [Space]
    [SerializeField] Color pathColor;
    [Space]
    [SerializeField] GameObject pathTextPrefab;
    [SerializeField] GameObject pathTextParent;
    [SerializeField] Vector2 pathTextSpawnPos;
    [SerializeField] float pathTextSpawnOffset;
    [SerializeField] List<GameObject> path;

    int destinationDistance;
    bool hasColored;

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
            FindPathAtStart(currentCity.name, destinationCity.name);
        }
        else
        {
            GetRandomCity();
        }
    }

    #region Path Finding
    public void FindPathAtStart(string startCityString, string targetCityString)
    {
        GameObject startCity = GameObject.Find(startCityString);
        GameObject targetCity = GameObject.Find(targetCityString);

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
        if (path == null)
        {
            GetRandomCity();
        }

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
        CreatePathText();
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

        FindPathAtStart(currentCity.name, destinationCity.name);
    }
    #endregion

    #region Visual Setup
    void CreatePathText()
    {
        GameObject previousText = null;

        for (int i = 0; i < path.Count; i++)
        {
            GameObject spawnedTextObject = Instantiate(pathTextPrefab);
            spawnedTextObject.transform.SetParent(pathTextParent.transform);

            if (previousText == null)
            {
                spawnedTextObject.transform.localPosition = pathTextSpawnPos;
            }
            else
            {
                spawnedTextObject.transform.localPosition = new Vector2(pathTextSpawnPos.x, pathTextSpawnPos.y - (pathTextSpawnOffset * i));
            }

            spawnedTextObject.transform.localScale = new Vector3(1f, 1f, 1f);
            previousText = spawnedTextObject;

            TextMeshProUGUI spawnedText = spawnedTextObject.GetComponent<TextMeshProUGUI>();
            spawnedText.text = path[i].name;

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
            if (path[i + 1] != null)
            {
                if (cityNeighbors[j] == path[i + 1])
                {
                    cityNeighborLines[j].GetComponent<Image>().color = pathColor;
                    Debug.Log("Painted Path");
                }
            }

            if (currentCity == cityNeighbors[j])
            {
                cityNeighborLines[j].GetComponent<Image>().color = pathColor;
            }
        }
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
