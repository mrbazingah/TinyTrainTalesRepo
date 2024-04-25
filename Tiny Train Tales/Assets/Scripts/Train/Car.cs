using System.Collections;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] int earning;
    [SerializeField] float time;
    [SerializeField] string carName;
    [SerializeField] GameObject coinButton;
    [Space]
    [SerializeField] float autoCollectDelay;

    bool hasAutoCollected;

    GameManager gameManager;
    Train train;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        train = FindObjectOfType<Train>();

        int i = PlayerPrefs.GetInt(carName);
        if (i == 1)
        {
            coinButton.SetActive(true);
        }
        else
        {
            StartCoroutine(EarningDelay());
        }
    }

    void Update()
    {
        float speed = train.GetSpeed();
        if (speed == 0)
        {
            gameManager.SaveCar(coinButton.activeInHierarchy, carName);
        }

        bool autoCollect = gameManager.GetAutoCollect();
        if (autoCollect && coinButton.activeInHierarchy && !hasAutoCollected)
        {
            StartCoroutine(AutoCollectCoin());
        }
    }

    IEnumerator EarningDelay()
    {
        coinButton.SetActive(false);

        yield return new WaitForSeconds(time);

        coinButton.SetActive(true);
    }

    IEnumerator AutoCollectCoin()
    {
        hasAutoCollected = true;

        yield return new WaitForSeconds(autoCollectDelay);

        CollectCoins();
    }

    public void CollectCoins()
    {
        StartCoroutine(EarningDelay());

        gameManager.AddCoins(earning);
        hasAutoCollected = false;
    }
}
