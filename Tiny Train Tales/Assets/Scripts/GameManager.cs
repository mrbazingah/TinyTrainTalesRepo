using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Coins")]
    [SerializeField] int coins;
    [SerializeField] TextMeshProUGUI cointext;
    [Header("Speed")]
    [SerializeField] float maxSpeed;
    [SerializeField] TextMeshProUGUI speedText;
    [Header("Destination")]
    [SerializeField] float distance;
    [SerializeField] Slider distanceSlider;
    [SerializeField] TextMeshProUGUI remainingDistanceText;

    float remainingDistance;
    float velocity;

    Train train;

    void Awake()
    {
        train = FindObjectOfType<Train>();
        distanceSlider.maxValue = distance;

        remainingDistance = distance;
        remainingDistanceText.text = distance.ToString() + "km";
    }

    void Update()
    {
        HandleMaxSpeed();
        HandleDestionationDistance();
    }

    void HandleMaxSpeed()
    {
        velocity = train.GetVelocity();
        if (velocity <= 0)
        {
            velocity *= 5;
        }
        else
        {
            velocity = velocity * 5 + 1;
        }

        velocity = Mathf.Floor(velocity);
        speedText.text = velocity.ToString() + " km/h";
    }

    void HandleDestionationDistance()
    {
        remainingDistance -= velocity * Time.deltaTime / 60f;
        distanceSlider.value = distance - remainingDistance;

        if ((int)remainingDistance < (int)(remainingDistance + velocity * Time.deltaTime / 60f))
        {
            remainingDistance = Mathf.Round(remainingDistance);
            remainingDistanceText.text = remainingDistance.ToString() + "km";
        }

        if (remainingDistance <= 0)
        {
            remainingDistance = 0;
            train.StopTrain();
        }
    }

    public void AddCoins(int amountAdded)
    {
        coins += amountAdded;
        cointext.text = coins.ToString();
    }

    public void Buy(int cost)
    {
        if (coins < cost) { return; }

        coins -= cost;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public int GetCoins()
    {
        return coins;
    }
}
