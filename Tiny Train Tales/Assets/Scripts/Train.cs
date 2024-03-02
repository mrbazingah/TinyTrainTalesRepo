using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] float acceleration;
    [SerializeField] float decelartion;
    [SerializeField] float speed;
    [Space]
    [SerializeField] Rigidbody2D[] backgroundRigidbody;

    bool isDriving;

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        FindRigidbodies();
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

    public void StopTrain()
    {
        isDriving = false;
    }

    public void StartTrain()
    {
        isDriving = true;
    }

    void Movement()
    {
        if (backgroundRigidbody[0].velocity.x <= 0)
        {
            for (int i = 0; i < backgroundRigidbody.Length; i++)
            {
                backgroundRigidbody[0].velocity = Vector3.zero;
            }

            speed = 0;
        }

        float maxSpeed = gameManager.GetMaxSpeed();

        if (isDriving && maxSpeed > backgroundRigidbody[0].velocity.x)
        {
            speed += acceleration * Time.fixedDeltaTime;
        }
        else if ((!isDriving && backgroundRigidbody[0].velocity.x > 0) || backgroundRigidbody[0].velocity.x > maxSpeed)
        {
            speed -= decelartion * Time.fixedDeltaTime;
        }

        for (int i = 0; i < backgroundRigidbody.Length; i++)
        {
            backgroundRigidbody[i].velocity = new Vector2(speed * Time.fixedDeltaTime, backgroundRigidbody[i].velocity.y);
        }
    }

    public float GetVelocity()
    {
        float s = backgroundRigidbody[0].velocity.x * 5;

        if (s <= 0)
        {
            s = 0;
        }

        return s;
    }

    public void FindRigidbodies()
    {
        GameObject[] allBackgrounds = GameObject.FindGameObjectsWithTag("Block");

        backgroundRigidbody = new Rigidbody2D[allBackgrounds.Length];

        for (int i = 0; i < allBackgrounds.Length; i++)
        {
            backgroundRigidbody[i] = allBackgrounds[i].GetComponent<Rigidbody2D>();
        }
    }
}
