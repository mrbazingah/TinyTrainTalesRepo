using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class Car : MonoBehaviour
{
    [Header("Earning")]
    [SerializeField] int minEarning;
    [SerializeField] int maxEarning;
    [SerializeField] float minTime;
    [SerializeField] float maxTime;
    [SerializeField] GameObject coinButton;
    [Space]
    [SerializeField] float autoCollectDelay;
    [Header("Attributes")]
    [SerializeField] int speed;
    [SerializeField] int weight = 1;
    [SerializeField] int income;
    [Header("Attributes Visual")]
    [SerializeField] List<GameObject> speedList;
    [SerializeField] List<GameObject> weightList;
    [SerializeField] List<GameObject> incomeList;
    [SerializeField] Color attributeColor;
    [Space] 
    [SerializeField] GameObject attributesCanvas;

    bool hasAutoCollected;
    float time;
    float currentTime;
    int earning;

    GameManager gameManager;
    CarManager carManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        carManager = FindObjectOfType<CarManager>();
    }

    void Start()
    {
        if (PlayerPrefs.HasKey(gameObject.name + "Time"))
        {
            time = PlayerPrefs.GetFloat(gameObject.name + "Time");
            currentTime = PlayerPrefs.GetFloat(gameObject.name + "CurrentTime");
            earning = PlayerPrefs.GetInt(gameObject.name + "Earning");
        }
        else
        {
            time = Random.Range(minTime, maxTime);
            currentTime = time;
            earning = 0;
        }

        transform.SetParent(GameObject.Find("Train").transform);
        attributesCanvas.SetActive(false);
    }

    void Update()
    {
        ShouldAutoCollect();
        EarningDelay();
    }

    #region Earning
    void ShouldAutoCollect()
    {
        bool autoCollect = gameManager.GetAutoCollect();
        if (autoCollect && coinButton.activeInHierarchy && !hasAutoCollected)
        {
            StartCoroutine(AutoCollectCoin());
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
            earning = (int)Random.Range(minEarning, maxEarning) * income;
        }

        gameManager.AddCoins(earning);
        hasAutoCollected = false;
        coinButton.SetActive(false);
        currentTime = time;
    }
    #endregion

    #region Attributes
    public void AddAttributes(int addedSpeed, int addedWeight, int addedIncome)
    {
        speed = addedSpeed;
        weight = addedWeight;
        income = addedIncome;

        if (weight <= 0)
        {
            weight = 1;
        }

        SetUpVisual();

        PlayerPrefs.SetInt(gameObject.name + "Weight", weight);
        PlayerPrefs.SetInt(gameObject.name + "Speed", speed);
        PlayerPrefs.SetInt(gameObject.name + "Income", income);
        PlayerPrefs.Save();
    }

    void SetUpVisual()
    {
        for (int i = 0; i < speed; i++)
        {
            speedList[i].GetComponent<Image>().color = attributeColor;
        }

        for (int i = 0; i < weight; i++)
        {
            weightList[i].GetComponent<Image>().color = attributeColor;
        }

        for (int i = 0; i < income; i++)
        {
            incomeList[i].GetComponent<Image>().color = attributeColor;
        }
    }

    public void OpenAndCloseAttributes(bool isButton)
    {
        bool turnOn = isButton ? !attributesCanvas.activeSelf : false;
        attributesCanvas.SetActive(turnOn);

        if (!isButton) { return; }

        List<GameObject> allCars = carManager.GetCars();
        foreach (GameObject car in allCars)
        {
            Car carComponent = car.GetComponent<Car>();

            if (car != gameObject && carComponent != null)
            {
                carComponent.OpenAndCloseAttributes(false);
            }
        }
    }
    #endregion

    #region Gets
    public int GetSpeed()
    {
        return speed;
    }

    public int GetWeight()
    {
        return weight;
    }

    public int GetIncome()
    {
        return income;
    }
    #endregion

    public void SaveCar()
    {
        PlayerPrefs.SetFloat(gameObject.name + "Time", time);
        PlayerPrefs.SetFloat(gameObject.name + "CurrentTime", currentTime);
        PlayerPrefs.SetInt(gameObject.name + "Earning", earning);

        PlayerPrefs.SetInt(gameObject.name + "Weight", weight);
        PlayerPrefs.SetInt(gameObject.name + "Speed", speed);
        PlayerPrefs.SetInt(gameObject.name + "Income", income);

        PlayerPrefs.Save();
    }
}