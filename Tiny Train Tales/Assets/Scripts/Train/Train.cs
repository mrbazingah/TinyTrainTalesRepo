using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] float acceleration;
    [SerializeField] float decelartion;
    [SerializeField] float speed;
    [SerializeField] float interval;
    [SerializeField] float carSpeedOffset;

    bool isDriving;
    bool hasLoaded;
    float highestVelocity;
    float localMaxSpeed;

    new Rigidbody2D rigidbody;
    GameManager gameManager;
    Station station;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        FindRigidbody();
    }

    public void FindRigidbody()
    {
        GameObject background = GameObject.FindGameObjectWithTag("Block");
        rigidbody = background.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        StartTrain();
    }

    public void LoadTrain()
    {
        //TrainData trainData = SaveSystem.LoadTrain();
        //acceleration = trainData.acceleration;
        //speed = trainData.speed;

        if (PlayerPrefs.HasKey("Acceleration") || PlayerPrefs.HasKey("Speed"))
        {
            speed = PlayerPrefs.GetFloat("Speed");
            acceleration = PlayerPrefs.GetFloat("Acceleration");
        }

        hasLoaded = true;
    }

    public void SaveTrain()
    {
        //SaveSystem.SaveTrain(this);

        PlayerPrefs.SetFloat("Speed", speed);
        PlayerPrefs.SetFloat("Acceleration", acceleration);

    }

    public bool GetHasLoaded()
    {
        return hasLoaded;
    }

    #region Movement
    void FixedUpdate()
    {
        Movement();
    }

    void Update()
    {
        StopAtStation();
    }

    void Movement()
    {
        if (rigidbody == null) 
        {
            FindRigidbody();
        }

        if (-rigidbody.velocity.x <= 0 && !hasLoaded)
        {
            rigidbody.velocity = Vector3.zero;
            speed = 0;
        }

        if (speed > highestVelocity)
        {
            highestVelocity = speed;
        }

        decelartion = highestVelocity / 5;

        CalculateSpeed();

        if (isDriving && localMaxSpeed > -rigidbody.velocity.x)
        {
            if (-rigidbody.velocity.x < localMaxSpeed + interval && -rigidbody.velocity.x > localMaxSpeed - interval) { return; }

            speed += acceleration * Time.fixedDeltaTime;
        }
        else if ((!isDriving && -rigidbody.velocity.x > 0f) || -rigidbody.velocity.x > localMaxSpeed)
        {
            speed -= decelartion * Time.fixedDeltaTime;
        }
    }

    void CalculateSpeed()
    {
        int allweight = 0;
        int allSpeed = 0;

        Car[] allCars = FindObjectsOfType<Car>();
        for (int i = 0;  i < allCars.Length; i++)
        {
            int currentWeight = allCars[i].GetWeight();
            allweight += currentWeight;

            int currentSpeed = allCars[i].GetSpeed();
            allSpeed += currentSpeed;
        }

        float averageWeight = ((allweight / allCars.Length) * 2);
        averageWeight /= 100;
        averageWeight = 1 - averageWeight;

        float averageSpeed = ((allSpeed / allCars.Length) * 2);
        averageSpeed /= 100;
        averageSpeed = 1 + averageSpeed - carSpeedOffset;

        localMaxSpeed = (gameManager.GetMaxSpeed() / 5f) * averageWeight * averageSpeed;
    }

    void StopAtStation()
    {
        bool arrivedAtStation = gameManager.GetDestinationReached();
        if (arrivedAtStation) 
        {
            if (station == null)
            {
                station = FindObjectOfType<Station>();
            }
            else if (station.gameObject.transform.position.x < gameObject.transform.position.x)
            {
                StopTrain();
                speed = 0;

                gameManager.HandleArrival(true);
            }
        }
    }
    
    public void StopTrain()
    {
        isDriving = false;
    }

    public void StartTrain()
    {
        isDriving = true;
    }

    public float GetVelocity()
    {
        if (rigidbody == null)
        {
            FindRigidbody();
        }
        
        float s = -rigidbody.velocity.x;

        if (s <= 0f)
        {
            s = 0f;
        }

        return s;
    }

    public void AddToAcceleration(float amountAdded)
    {
        acceleration += amountAdded;
        PlayerPrefs.SetFloat("Acceleration", acceleration);
    }
    #endregion

    public float GetDecelartion()
    {
        return decelartion;
    }

    public float GetAcceleration()
    {
        return acceleration;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
