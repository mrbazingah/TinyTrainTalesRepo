using UnityEngine;

public class CityManager : MonoBehaviour
{
    [SerializeField] GameObject currentCity;
    
    GameObject destinationCity;
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

    public void GetNextCity()
    {
        if (PlayerPrefs.HasKey("CurrentCity"))
        {
            string currentCityString = PlayerPrefs.GetString("CurrentCity");
            currentCity = GameObject.Find(currentCityString);
        }

        City currentCityScript = currentCity.GetComponent<City>();
        GameObject[] currentCityNeighbors = currentCityScript.GetCityNeighbors();

        int nextCity = (int)Random.Range(0, currentCityNeighbors.Length - 1);
        destinationCity = currentCityNeighbors[nextCity];

        int[] distances = currentCityScript.GetCityNeighborsDistance();

        gameManager.SetNewDestination(currentCity.name,destinationCity.name, distances[nextCity]);
    }

    public void SaveCity()
    {
        PlayerPrefs.SetString("CurrentCity", destinationCity.name);
    }

    public GameObject GetCurrentCity()
    {
        return currentCity;
    }
}
