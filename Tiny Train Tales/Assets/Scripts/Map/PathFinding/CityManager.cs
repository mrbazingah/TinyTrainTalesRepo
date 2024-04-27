using TMPro;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    [SerializeField] GameObject currentCity;
    [SerializeField] GameObject destinationCity;

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();   
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
    }

    public void UpdateCityTexts(TextMeshProUGUI currentCityText, TextMeshProUGUI destinationCityText)
    {
        currentCityText.text = currentCity.name;
        destinationCityText.text = destinationCity.name;
    }

    public void GetNextCity()
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

        int[] distances = currentCityScript.GetCityNeighborsDistance();
        int currentDistance = distances[nextCity];

        if (currentCity == destinationCity)
        {
            GetNextCity();
            return;
        }

        gameManager.SetNewDestination(currentCity.name,destinationCity.name, currentDistance);
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
