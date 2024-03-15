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
    [SerializeField] float integral;

    bool isDriving;

    new Rigidbody2D rigidbody;
    GameManager gameManager;

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
        StopAtStation();
    }

    void Update()
    {
        MovementInputs();
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
            if (-rigidbody.velocity.x < maxSpeed + integral && -rigidbody.velocity.x > maxSpeed - integral) { return; }

            speed += acceleration * Time.fixedDeltaTime;
        }
        else if ((!isDriving && -rigidbody.velocity.x > 0) || -rigidbody.velocity.x > maxSpeed)
        {
            speed -= decelartion * Time.fixedDeltaTime;
        }
    }

    void StopAtStation()
    {
        bool arrivedAtStation = gameManager.GetArrivedAtStation();
        if (arrivedAtStation) 
        {
            GameObject station = GameObject.FindGameObjectWithTag("Station");
            float distance = Vector2.Distance(gameObject.transform.position, station.transform.position);

            if (distance < 1)
            {
                speed = 0;
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
