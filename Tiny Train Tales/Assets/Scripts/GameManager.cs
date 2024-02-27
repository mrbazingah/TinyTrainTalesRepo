using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Coins")]
    [SerializeField] int coins;
    [SerializeField] TextMeshProUGUI cointext;
    [Header("Speed")]
    [SerializeField] float maxSpeed;
    [SerializeField] TextMeshProUGUI speedText;

    float speed;

    Train train;

    void Awake()
    {
        train = FindObjectOfType<Train>();
    }

    void Update()
    {
        speed = train.GetSpeed();
        float s = Mathf.Floor(speed);
        speedText.text = s.ToString() + " km/h";
    }

    public void AddCoins(int amountAdded)
    {
        coins += amountAdded;
        cointext.text = coins.ToString();
    }

    public float GetMaxSpeed()
    {
        return maxSpeed / 5;
    }
}
