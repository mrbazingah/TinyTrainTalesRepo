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

    float velocty;

    Train train;

    void Awake()
    {
        train = FindObjectOfType<Train>();
    }

    void Update()
    {
        velocty = train.GetVelocity();
        float speed = Mathf.Floor(velocty);
        speedText.text = speed.ToString() + " km/h";
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
