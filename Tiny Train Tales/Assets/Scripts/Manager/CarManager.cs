using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    [SerializeField] GameObject carPrefab;
    [SerializeField] List<GameObject> currentCars = new List<GameObject>();
    [SerializeField] float spawnOffset;
    [Space]
    [SerializeField] BoxCollider2D trainCollider;
    [SerializeField] float colliderOffset;
    [SerializeField] Vector2 startPos;

    int length = 1;
    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    void Start()
    {
        if (carPrefab == null)
        {
            Debug.LogError("Car Prefab is not assigned!");
            return;
        }

        currentCars = SetUpCars();
    }

    public List<GameObject> SetUpCars()
    {
        List<GameObject> allCars = new List<GameObject>();

        if (PlayerPrefs.HasKey("Cars"))
        {
            length = PlayerPrefs.GetInt("Cars");
        }

        GameObject lastSpawned = null;
        for (int i = 0; i < length; i++)
        {
            if (i == 0)
            {
                lastSpawned = Instantiate(carPrefab, startPos, Quaternion.identity);
            }
            else
            {
                lastSpawned = Instantiate(carPrefab, new Vector2(allCars[i - 1].transform.position.x - spawnOffset, allCars[i - 1].transform.position.y), Quaternion.identity);
            }

            lastSpawned.transform.parent = GameObject.Find("Train").transform;
            lastSpawned.name = "Car " + (i + 1).ToString();

            if (PlayerPrefs.HasKey(lastSpawned.name + "Weight"))
            {
                int weight = PlayerPrefs.GetInt(lastSpawned.name + "Weight");
                int speed = PlayerPrefs.GetInt(lastSpawned.name + "Speed");
                int income = PlayerPrefs.GetInt(lastSpawned.name + "Income");

                lastSpawned.GetComponent<Car>().AddAttributes(speed, weight, income);
            }
            else
            {
                lastSpawned.GetComponent<Car>().AddAttributes(1, 1, 1);
            }

            allCars.Add(lastSpawned);
        }

        return allCars;
    }

    void Update()
    {
        UpdateTrainCollider();
    }

    void UpdateTrainCollider()
    {
        trainCollider.size = new Vector2(startPos.x + colliderOffset * currentCars.Count, 1);
    }

    public void BuyNewCar(int weight, int speed, int income)
    {
        length++;
        GameObject lastSpawned = currentCars.Count > 0 ? currentCars[currentCars.Count - 1] : null;

        GameObject currentlySpawned = Instantiate(carPrefab, new Vector2(lastSpawned != null ? lastSpawned.transform.position.x - spawnOffset : startPos.x, startPos.y), Quaternion.identity);
        currentlySpawned.transform.parent = GameObject.Find("Train").transform;

        currentlySpawned.name = "Car " + length.ToString();
        currentlySpawned.GetComponent<Car>().AddAttributes(speed, weight, income);
        currentCars.Add(currentlySpawned);

        PlayerPrefs.SetInt("Cars", length);
        PlayerPrefs.Save();
    }

    public void SaveCars()
    {
        foreach (var car in currentCars)
        {
            if (car != null)
            {
                car.GetComponent<Car>().SaveCar();
            }
        }
    }

    public int GetLength()
    {
        return length;
    }

    public List<GameObject> GetCars()
    {
        return currentCars;
    }
}