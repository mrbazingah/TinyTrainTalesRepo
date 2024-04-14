using System.Collections;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] int earning;
    [SerializeField] float time;
    [SerializeField] string name;
    [SerializeField] GameObject coinButton;

    GameManager gameManager;
    Train train;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        train = FindObjectOfType<Train>();

        int i = PlayerPrefs.GetInt(name);
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
            gameManager.SaveCar(coinButton.activeInHierarchy, name);
        }
    }

    IEnumerator EarningDelay()
    {
        coinButton.SetActive(false);

        yield return new WaitForSeconds(time);

        coinButton.SetActive(true);
    }

    public void CollectCoins()
    {
        StartCoroutine(EarningDelay());

        gameManager.AddCoins(earning);
    }
}
