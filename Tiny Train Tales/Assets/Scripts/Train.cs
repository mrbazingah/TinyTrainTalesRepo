using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] float acceleration;
    [SerializeField] float decelartion;
    [SerializeField] float speed;
    [SerializeField] float interval;

    bool isDriving;
    bool hasFoundStation;

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

    void FixedUpdate()
    {
        Movement();
    }

    void Update()
    {
        MovementInputs();
        StopAtStation();
    }

    void MovementInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isDriving)
            {
                StartTrain();
            }
            else
            {
                StopTrain();
            }
        }
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

        float maxSpeed = gameManager.GetMaxSpeed() / 5;
        decelartion = maxSpeed * 50 / 3;

        if (isDriving && maxSpeed > -rigidbody.velocity.x)
        {
            if (-rigidbody.velocity.x < maxSpeed + interval && -rigidbody.velocity.x > maxSpeed - interval) { return; }

            speed += acceleration * Time.fixedDeltaTime;
        }
        else if ((!isDriving && -rigidbody.velocity.x > 0) || -rigidbody.velocity.x > maxSpeed)
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

        if (s <= 0)
        {
            s = 0;
        }

        return s;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
