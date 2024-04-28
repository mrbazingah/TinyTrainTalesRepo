using UnityEngine;

public class MapTrain : MonoBehaviour
{
    [SerializeField] GameObject endCity;
    [SerializeField] float speedOffset;

    float speed;
    bool hasFoundCity;

    Train train;
    CityManager cityManager;

    void Awake()
    {
        train = FindObjectOfType<Train>();
        cityManager = FindObjectOfType<CityManager>();
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("MapTrainPosX"))
        {
            float xPos = PlayerPrefs.GetFloat("MapTrainPosX");
            float yPos = PlayerPrefs.GetFloat("MapTrainPosY");

            transform.position = new Vector3(xPos, yPos, 0);
        }
    }

    void FixedUpdate()
    {
        if (!hasFoundCity)
        {
            if (cityManager == null)
            {
                cityManager = FindObjectOfType<CityManager>();
            }

            endCity = cityManager.GetDestinationCity();
            transform.position = cityManager.GetCurrentCity().transform.position;
            hasFoundCity = true;
        }

        speed = train.GetVelocity() / speedOffset / 4f;
        transform.position = Vector3.MoveTowards(transform.position, endCity.transform.position, speed);
    }

    public void SaveMapTrainPos()
    {
        PlayerPrefs.SetFloat("MapTrainX", transform.position.x);
        PlayerPrefs.SetFloat("MapTrainY", transform.position.y);
    }
}
