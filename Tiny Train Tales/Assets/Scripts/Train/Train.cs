using UnityEngine;
using System.Collections.Generic;

public class Train : MonoBehaviour
{
    [SerializeField] float acceleration;
    [SerializeField] float decelartion;
    [SerializeField] float speed;
    [SerializeField] float interval;
    [SerializeField] float carSpeedOffset;
    [SerializeField] float carWeightOffset;
    [Space]
    [SerializeField] List<Animator> trainAnimators;
    [SerializeField] List <Animator> carAnimators;
    [SerializeField] float maxAnimationSpeed;
    [SerializeField] float carAnimationSpeedOffset;

    bool isDriving;
    bool hasLoaded;
    bool isDeclerating;
    float highestVelocity;
    float localMaxSpeed;
    bool hasStopped;

    new Rigidbody2D rigidbody;
    GameManager gameManager;
    Station station;
    DynamicSmokeEffect smokeEffect;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        smokeEffect = FindObjectOfType<DynamicSmokeEffect>();

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
        TrainSaveData data = SaveSystem.Instance.GetTrainData();
        if (data != null)
        {
            speed = data.trainSpeed;
            acceleration = data.trainAcceleration;
        }
        hasLoaded = true;
    }

    public void SaveTrain()
    {
        SaveSystem.Instance.SetTrainData(new TrainSaveData
        {
            trainSpeed = speed,
            trainAcceleration = acceleration
        });
    }

    public bool GetHasLoaded()
    {
        return hasLoaded;
    }

    public void AddCarAnimators(Animator newCarAnimator)
    {
        carAnimators.Add(newCarAnimator);
    }

    #region Movement
    void FixedUpdate()
    {
        Movement();
    }

    void Update()
    {
        StopAtStation();
        Animation();
    }

    void Movement()
    {
        if (rigidbody == null) 
        {
            FindRigidbody();
        }

        if (hasStopped) { return; }

        if (-rigidbody.velocity.x <= 0 && !hasLoaded)
        {
            rigidbody.velocity = Vector3.zero;
            speed = 0;
        }

        if (!isDeclerating && highestVelocity != speed)
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
            isDeclerating = true;
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
        averageWeight = 1 - averageWeight - carWeightOffset;

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
                hasStopped = true;

                smokeEffect.StopMode();
            }
        }
    }
    
    void Animation()
    {
        float currentSpeed = speed * 5f;
        float animatorSpeed = (currentSpeed / maxAnimationSpeed) * 2.5f;
        
        for (int i = 0; i < trainAnimators.Count; i++)
        {
            trainAnimators[i].speed = animatorSpeed;
        }

        for (int i = 0; i < carAnimators.Count; i++)
        {
            carAnimators[i].speed = animatorSpeed * carAnimationSpeedOffset;
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
    }
    #endregion

    public Rigidbody2D GetRigidbody()
    {
        return rigidbody;
    }

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

    public List<SpriteRenderer> GetSpriteRenderers()
    {
        List<SpriteRenderer> srs = new List<SpriteRenderer>();

        for (int i = 0; i < carAnimators.Count; i++)
        {
            srs.Add(carAnimators[i].gameObject.GetComponent<SpriteRenderer>());
        }

        srs.Add(GetComponentInChildren<SpriteRenderer>());

        return srs;
    }

    public bool GetHasStopped()
    {
        return hasStopped;
    }
}
