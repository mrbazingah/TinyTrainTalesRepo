using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] float acceleration;
    [SerializeField] float decelartion;

    float speed;

    Rigidbody2D myRigidbody;
    GameManager gameManager;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void FixedUpdate()
    {
        if (myRigidbody.velocity.x <= 0)
        {
            myRigidbody.velocity = Vector3.zero;
            speed = 0;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            float maxSpeed = gameManager.GetMaxSpeed();
            if (myRigidbody.velocity.x > maxSpeed) { return; }
            
            speed += acceleration * Time.fixedDeltaTime;
        }
        else if (!Input.GetKey(KeyCode.Space) && myRigidbody.velocity.x > 0)
        {
            speed -= decelartion * Time.fixedDeltaTime;
        }

        myRigidbody.velocity = new Vector2(speed * Time.fixedDeltaTime, myRigidbody.velocity.y);
    }

    public float GetSpeed()
    {
        float s = myRigidbody.velocity.x * 5;

        if (s <= 0)
        {
            s = 0;
        }

        return s;
    }
}
