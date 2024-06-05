using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] float acceleration;
    [SerializeField] float decelartion;
    [SerializeField] float speed;
    [SerializeField] float interval;

    bool isDriving;
    bool hasLoaded;
    float highestVelocity;

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

        if (rigidbody.velocity.x > highestVelocity)
        {
            highestVelocity = rigidbody.velocity.x;
        }

        decelartion = highestVelocity * 10f;
        float maxSpeed = gameManager.GetMaxSpeed() / 5f;

        if (isDriving && maxSpeed > -rigidbody.velocity.x)
        {
            if (-rigidbody.velocity.x < maxSpeed + interval && -rigidbody.velocity.x > maxSpeed - interval) { return; }

            speed += acceleration * Time.fixedDeltaTime;
        }
        else if ((!isDriving && -rigidbody.velocity.x > 0f) || -rigidbody.velocity.x > maxSpeed)
        {
            speed -= decelartion * Time.fixedDeltaTime;
        }
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
