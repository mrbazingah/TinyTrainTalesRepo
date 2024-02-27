using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : MonoBehaviour
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
