using UnityEngine;

public class MapTrain : MonoBehaviour
{
    [SerializeField] GameObject endCity;
    [SerializeField] float speedOffset;

    float speed;

    Train train;
    CityManager cityManager;

    void Awake()
    {
        train = FindObjectOfType<Train>();
        cityManager = FindObjectOfType<CityManager>();
    }

    void FixedUpdate()
    {
        if (cityManager == null)
        {
            cityManager = FindObjectOfType<CityManager>();
        }

        transform.position = cityManager.GetCurrentCity().transform.position;
        speed = train.GetVelocity() / speedOffset / 4f;
        transform.position = Vector3.MoveTowards(transform.position, endCity.transform.position, speed);
    }
}
