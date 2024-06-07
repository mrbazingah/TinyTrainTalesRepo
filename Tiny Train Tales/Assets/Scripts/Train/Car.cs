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

    bool hasAutoCollected;
    float time;
    float currentTime;

    GameManager gameManager;
    Train train;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        train = FindObjectOfType<Train>();
    }

    void Start()
    {
        GameObject[] allCars = GameObject.FindGameObjectsWithTag("Car");
        for (int i = 0; i < allCars.Length; i++)
        {
            if (allCars[i] == gameObject)
            {
                gameObject.name = "Car(" + i.ToString() + ")";
            }
        }

        if (PlayerPrefs.HasKey(gameObject.name + "Time"))
        {
            time = PlayerPrefs.GetFloat(gameObject.name + "Time");
            currentTime = PlayerPrefs.GetFloat(gameObject.name + "CurrenTime");

            Debug.Log("GH");
        }
        else
        {
            time = Random.Range(minTime, maxTime);
            currentTime = time;
        }
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
        int earning = (int)Random.Range(minEarning, maxEarning);
        gameManager.AddCoins(earning);
        hasAutoCollected = false;
        currentTime = time;
    }

    public void SaveCar()
    {
        PlayerPrefs.SetFloat(gameObject.name + "Time", time);
        PlayerPrefs.SetFloat(gameObject.name + "CurrenTime", currentTime);
    }
}
