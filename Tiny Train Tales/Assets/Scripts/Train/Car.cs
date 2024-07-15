using System.Collections;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] int minEarning;
    [SerializeField] int maxEarning;
    [SerializeField] float minTime;
    [SerializeField] float maxTime;
    [SerializeField] GameObject coinButton;
    [Space]
    [SerializeField] float autoCollectDelay;
    [Header("Attributes")]
    [SerializeField] int weight;
    [SerializeField] int speed;
    [SerializeField] int income;

    bool hasAutoCollected;
    float time;
    float currentTime;
    int earning;

    GameManager gameManager;
    Train train;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        train = FindObjectOfType<Train>();
    }

    void Start()
    {
        if (PlayerPrefs.HasKey(gameObject.name + "Time"))
        {
            time = PlayerPrefs.GetFloat(gameObject.name + "Time");
            currentTime = PlayerPrefs.GetFloat(gameObject.name + "CurrenTime");
            earning = PlayerPrefs.GetInt(gameObject.name + "Earning");
        }
        else
        {
            time = Random.Range(minTime, maxTime);
            currentTime = time;
        }

        transform.SetParent(GameObject.Find("Train").transform);
    }

    public void AddAttributes(int addedWeight, int addedSpeed, int addedIncome)
    {
        weight = addedWeight;
        speed = addedSpeed;
        income = addedIncome;

        PlayerPrefs.SetInt(gameObject.name + "Weight", weight);
        PlayerPrefs.SetInt(gameObject.name + "Speed", speed);
        PlayerPrefs.SetInt(gameObject.name + "Income", income);
    }

    void Update()
    {
        float speed = train.GetSpeed();
        if (speed == 0)
        {
            gameManager.SaveCar(currentTime, time, gameObject.name);
        }

        bool autoCollect = gameManager.GetAutoCollect();
        if (autoCollect && coinButton.activeInHierarchy && !hasAutoCollected)
        {
            StartCoroutine(AutoCollectCoin());
        }

        EarningDelay();

        if (weight == 0)
        {
            weight = 1;
        }
    }

    void EarningDelay()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            coinButton.SetActive(false);
        }
        else
        {
            coinButton.SetActive(true);
        }
    }

    IEnumerator AutoCollectCoin()
    {
        hasAutoCollected = true;

        yield return new WaitForSeconds(autoCollectDelay);

        CollectCoins();
    }

    public void CollectCoins()
    {
        if (earning <= 0)
        {
            earning = (int)Random.Range(minEarning, maxEarning);
        }

        gameManager.AddCoins(earning);
        hasAutoCollected = false;
        currentTime = time;
    }

    public void SaveCar()
    {
        PlayerPrefs.SetFloat(gameObject.name + "Time", time);
        PlayerPrefs.SetFloat(gameObject.name + "CurrenTime", currentTime);
        PlayerPrefs.SetInt(gameObject.name + "Earning", earning);
    }

    public int GetWeight()
    {
        return weight;
    }

    public int GetSpeed()
    {
        return speed;
    }

    public int GetIncome()
    {
        return income;
    }
}
