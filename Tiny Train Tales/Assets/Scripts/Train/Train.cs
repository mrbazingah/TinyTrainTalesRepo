using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] float acceleration;
    [SerializeField] float decelartion;
    [SerializeField] float speed;
    [SerializeField] float interval;

    bool isDriving;

    new Rigidbody2D rigidbody;
    GameManager gameManager;
    Station station;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        FindRigidbody();
    }

    void Start()
    {
        StartTrain();

        if (PlayerPrefs.HasKey("Acceleration"))
        {
            acceleration = PlayerPrefs.GetFloat("Acceleration");
        }
        if (PlayerPrefs.HasKey("Speed"))
        {
            speed = PlayerPrefs.GetFloat("Speed");
        }
    }

    public void FindRigidbody()
    {
        GameObject background = GameObject.FindGameObjectWithTag("Block");
        rigidbody = background.GetComponent<Rigidbody2D>();
    }

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

        if (-rigidbody.velocity.x <= 0)
        {
            rigidbody.velocity = Vector3.zero;
            speed = 0;
        }

        float maxSpeed = gameManager.GetMaxSpeed() / 5f;
        decelartion = maxSpeed * 10f;

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

    public float GetAcceleration()
    {
        return acceleration;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SaveSpeed()
    {
        PlayerPrefs.SetFloat("Speed", speed);
    }
}
