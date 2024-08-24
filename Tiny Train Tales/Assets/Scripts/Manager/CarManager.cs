using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CarManager : MonoBehaviour
{
    [SerializeField] GameObject carPrefab;
    [SerializeField] List<GameObject> currentCars;
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
    }

    void Start()
    {
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
                lastSpawned.transform.parent = GameObject.Find("Train").transform;
            }
            else
            {
                GameObject currentlySpawned = Instantiate(carPrefab, new Vector2(lastSpawned.transform.position.x - spawnOffset, lastSpawned.transform.position.y), Quaternion.identity);
                lastSpawned = currentlySpawned;
            }

            lastSpawned.name = "Car " + i.ToString();
            if (PlayerPrefs.HasKey(lastSpawned.name + "Weight"))
            {
                int weight = PlayerPrefs.GetInt(lastSpawned.name + "Weight");
                int speed = PlayerPrefs.GetInt(lastSpawned.name + "Speed");
                int income = PlayerPrefs.GetInt(lastSpawned.name + "Income");

                lastSpawned.GetComponent<Car>().AddAttributes(weight, speed, income);
            }
            else
            {
                lastSpawned.GetComponent<Car>().AddAttributes(1, 1, 1);
            }

            allCars.Add(lastSpawned);
        }

        return allCars;
    }

    public void BuyNewCar(int weight, int speed, int income)
    {
        length++;
        GameObject lastSpawned = currentCars[currentCars.Count - 1];

        GameObject currentlySpawned = Instantiate(carPrefab, new Vector2(lastSpawned.transform.position.x - spawnOffset, lastSpawned.transform.position.y), Quaternion.identity);
        lastSpawned = currentlySpawned;

        lastSpawned.name = "Car " + length.ToString();
        lastSpawned.GetComponent<Car>().AddAttributes(weight, speed, income);
        currentCars.Add(lastSpawned);

        PlayerPrefs.SetInt("Cars", length);
    }

    public void SaveCars()
    {
        Car[] cars = FindObjectsOfType<Car>();
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].SaveCar();
        }
    }

    public int GetLength()
    {
        return length;
    }
}
