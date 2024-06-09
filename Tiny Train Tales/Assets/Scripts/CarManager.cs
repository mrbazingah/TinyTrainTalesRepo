using TMPro;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    [SerializeField] GameObject carPrefab;
    [SerializeField] GameObject[] currentCars;
    [SerializeField] float spawnOffset;
    [Space]
    [SerializeField] BoxCollider2D trainCollider;
    [SerializeField] float colliderOffset;
    [SerializeField] Vector2 startPos;
    [Space]
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] float cost;
    [SerializeField] float costIncrease;

    int length;

    GameManager gameManager;
    UpgradeManager upgradeManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        upgradeManager = FindObjectOfType<UpgradeManager>();
    }

    void Start()
    {
        currentCars = GameObject.FindGameObjectsWithTag("Car");

        if (PlayerPrefs.HasKey("Cars"))
        {
            length = PlayerPrefs.GetInt("Cars");

            AddCar(true);
        }
        if (PlayerPrefs.HasKey("CostPerCar"))
        {
            cost = PlayerPrefs.GetFloat("CostPerCar");
        }

        costText.text = cost.ToString();
    }

    public void AddCar(bool isStart)
    {
        float coins = gameManager.GetCoins();
        if (coins < cost && upgradeManager.GetAmountOfCars() <= length) { return; }

        gameManager.BuyWithCoins(cost);
        cost += costIncrease;
        cost = Mathf.Round(cost);

        costText.text = cost.ToString();

        if (!isStart)
        {
            length = currentCars.Length + 1;
        }
      
        for (int i = 0; i < currentCars.Length; i++)
        {
            Destroy(currentCars[i]);
        }

        currentCars = new GameObject[length];
        GameObject lastSpawned = null;

        for (int i = 0; i < length; i++)
        {
            if (i == 0)
            {
                lastSpawned = Instantiate(carPrefab, startPos, Quaternion.identity);
            }
            else
            {
                Vector2 spawnPos = new Vector2(lastSpawned.transform.position.x - spawnOffset, lastSpawned.transform.position.y);
                GameObject currentlySpanwed = Instantiate(carPrefab, spawnPos, Quaternion.identity);
                lastSpawned = currentlySpanwed;
            }

            currentCars[i] = lastSpawned;
        }

        if (length == 1)
        {
            trainCollider.size = new Vector2(length * colliderOffset * 2, trainCollider.size.y);
        }
        else
        {
            trainCollider.size = new Vector2(length * colliderOffset, trainCollider.size.y);
        }
    }

    public void RemoveCar()
    {
        gameManager.BuyWithCoins(-cost / 2);
        cost -= costIncrease;
        cost = Mathf.Round(cost);
        costText.text = cost.ToString();

        length = currentCars.Length - 1;

        for (int i = 0; i < currentCars.Length; i++)
        {
            if (i == 0)
            {
                startPos = currentCars[i].transform.position;
            }

            Destroy(currentCars[i]);
        }

        currentCars = new GameObject[length];
        GameObject lastSpawned = null;

        for (int i = 0; i < length; i++)
        {
            if (i == 0)
            {
                lastSpawned = Instantiate(carPrefab, startPos, Quaternion.identity);
            }
            else
            {
                Vector2 spawnPos = new Vector2(lastSpawned.transform.position.x - spawnOffset, lastSpawned.transform.position.y);
                GameObject currentlySpanwed = Instantiate(carPrefab, spawnPos, Quaternion.identity);
                lastSpawned = currentlySpanwed;
            }

            currentCars[i] = lastSpawned;
        }

        trainCollider.size = new Vector2(length * colliderOffset, trainCollider.size.y);
    }

    public void SaveCars()
    {
        PlayerPrefs.SetInt("Cars", currentCars.Length);

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
