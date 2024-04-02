using System.Collections;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] int earning;
    [SerializeField] float time;
    [Space]
    [SerializeField] GameObject coinButton;

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        StartCoroutine(EarningDelay());
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
