using TMPro;
using UnityEngine;

public class Buy : MonoBehaviour
{
    [SerializeField] float cost;
    [SerializeField] float costIncrease;
    [SerializeField] TextMeshProUGUI costText;
    
    GameManager gameManager;
    float coins;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();


    }

    void Update()
    {
        coins = gameManager.GetCoins();
    }

    public void Buying()
    {
        if (coins < cost) { return; }
        gameManager.Buy(cost);
        cost *= costIncrease;
        cost = Mathf.Floor(cost);
        costText.text = cost.ToString();

        PlayerPrefs.SetFloat("MaxSpeedCost", cost);
    }
}
