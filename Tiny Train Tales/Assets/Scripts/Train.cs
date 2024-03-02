using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] float acceleration;
    [SerializeField] float decelartion;
    [SerializeField] float speed;

    bool isDriving;

    Rigidbody2D myRigidbody;
    GameManager gameManager;
    BackgroundGenerator background;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
        background = FindObjectOfType<BackgroundGenerator>();
    }

    void FixedUpdate()
    {
        Movement();
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
        if (myRigidbody.velocity.x <= 0)
        {
            myRigidbody.velocity = Vector3.zero;
            speed = 0;
        }

        float maxSpeed = gameManager.GetMaxSpeed();

        if (isDriving && maxSpeed > myRigidbody.velocity.x)
        {
            speed += acceleration * Time.fixedDeltaTime;
        }
        else if ((!isDriving && myRigidbody.velocity.x > 0) || myRigidbody.velocity.x > maxSpeed)
        {
            speed -= decelartion * Time.fixedDeltaTime;
        }

        myRigidbody.velocity = new Vector2(speed * Time.fixedDeltaTime, myRigidbody.velocity.y);
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
        float s = myRigidbody.velocity.x;

        if (s <= 0)
        {
            s = 0;
        }

        return s;
    }

    public float GetAcceleration()
    {
        return acceleration;
    }

    public float GetDecelartion()
    {
        return decelartion;
    }

    public bool GetIsDrving()
    {
        return isDriving;
    }
}
